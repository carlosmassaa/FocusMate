using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BL;
using Servicioss;
using BE;
using System.Drawing;

namespace UI
{
    public partial class ListadoTareas : Form, IIdiomaObserver
    {
        private readonly TareaBL servicioTarea = new TareaBL();
        private readonly UsuarioBL usuarioBL = new UsuarioBL();
        private readonly AuthManager authManager;

        private const string PermisoAdminHistorial = "Adminstrar_Historial_Tareas";

        public ListadoTareas()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            ConfigurarGrid();
            AplicarTraduccionesEstaticas();
            ConfigurarUsuariosSegunPermiso();
            CargarTareas();

            this.Activated += (object sender, EventArgs args) =>
            {
                CargarTareas();
                RefrescarPermisosAcciones();
            };
        }

        public ListadoTareas(AuthManager auth) : this()
        {
            authManager = auth;
            ConfigurarUsuariosSegunPermiso();
            RefrescarPermisosAcciones();
            CargarTareas();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IdiomaService.Instancia.Suscribir(this);
            Dictionary<string, string> traduccionesActuales = IdiomaService.Instancia.ObtenerTraduccionesActuales();
            if (traduccionesActuales != null && traduccionesActuales.Count > 0)
            {
                ActualizarTraducciones(traduccionesActuales);
            }

            RefrescarPermisosAcciones();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private bool TienePermisoAdminHistorial()
        {
            if (authManager == null)
            {
                return false;
            }

            return authManager.ValidarPermiso(PermisoAdminHistorial);
        }

        private void ConfigurarUsuariosSegunPermiso()
        {
            bool puedeVerCombo = TienePermisoAdminHistorial();
            comboBoxSeleccionarUsuarios.Visible = puedeVerCombo;

            comboBoxSeleccionarUsuarios.SelectedIndexChanged -= comboBoxSeleccionarUsuarios_SelectedIndexChanged;
            if (puedeVerCombo)
            {
                List<Usuario> usuarios = usuarioBL.Listar() ?? new List<Usuario>();
                comboBoxSeleccionarUsuarios.DisplayMember = "NombreUsuario";
                comboBoxSeleccionarUsuarios.ValueMember = "Id";
                comboBoxSeleccionarUsuarios.DataSource = usuarios;

                int actualId = SesionActual.Instance.UsuarioId;
                if (actualId > 0)
                {
                    comboBoxSeleccionarUsuarios.SelectedValue = actualId;
                }

                comboBoxSeleccionarUsuarios.SelectedIndexChanged += comboBoxSeleccionarUsuarios_SelectedIndexChanged;
            }
            else
            {
                comboBoxSeleccionarUsuarios.DataSource = null;
            }
        }

        private void RefrescarPermisosAcciones()
        {
            bool puedeEliminar = authManager != null && authManager.EstaAutenticado && authManager.ValidarPermiso("TAREA_ELIMINAR");
            btnEliminar.Enabled = puedeEliminar;
        }

        private void comboBoxSeleccionarUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!TienePermisoAdminHistorial())
            {
                return;
            }

            CargarTareas();
        }

        private int ResolverUsuarioParaListado()
        {
            int usuarioSesion = SesionActual.Instance.UsuarioId;
            if (TienePermisoAdminHistorial())
            {
                if (comboBoxSeleccionarUsuarios.SelectedValue is int usuarioSeleccionadoId && usuarioSeleccionadoId > 0)
                {
                    return usuarioSeleccionadoId;
                }
            }

            return usuarioSesion;
        }

        private void ConfigurarGrid()
        {
            dataGridViewTareas.AutoGenerateColumns = false;
            dataGridViewTareas.AllowUserToAddRows = false;
            dataGridViewTareas.AllowUserToDeleteRows = false;
            dataGridViewTareas.ReadOnly = true;
            dataGridViewTareas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTareas.MultiSelect = false;
            dataGridViewTareas.Columns.Clear();

            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TareaId", Name = "TareaId", Visible = false });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ScorePrioridad", Name = "ScorePrioridad", Visible = false });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", Name = "colTitulo", HeaderText = Trad("ListTareas_Col_Titulo", "Título"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Descripcion", Name = "colDescripcion", HeaderText = Trad("ListTareas_Col_Descripcion", "Descripción"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaLimite", Name = "colFechaLimite", HeaderText = Trad("ListTareas_Col_FechaLimite", "Fecha límite") });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Importancia", Name = "colImportancia", HeaderText = Trad("ListTareas_Col_Importancia", "Importancia") });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EnergiaRequerida", Name = "colEnergia", HeaderText = Trad("ListTareas_Col_Energia", "Energía") });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DuracionEstimadaMin", Name = "colDuracion", HeaderText = Trad("ListTareas_Col_Duracion", "Duración (min)") });
            dataGridViewTareas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", Name = "colEstado", HeaderText = Trad("ListTareas_Col_Estado", "Estado") });
        }

        private void CargarTareas()
        {
            int usuarioId = ResolverUsuarioParaListado();
            if (usuarioId <= 0)
            {
                MessageBox.Show(Trad("ListTareas_Msg_SinSesion", "No hay sesión activa. Inicie sesión para ver tareas."), Trad("ListTareas_Msg_Titulo", "Tareas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Tarea> tareas = servicioTarea.ListarPorUsuario(usuarioId);
            dataGridViewTareas.DataSource = tareas.ToList();
            dataGridViewTareas.AutoResizeColumns();
            dataGridViewTareas.ClearSelection();
        }

        private int? TareaSeleccionadaId()
        {
            if (dataGridViewTareas.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell celda = dataGridViewTareas.CurrentRow.Cells["TareaId"];
            if (celda == null || celda.Value == null)
            {
                return null;
            }

            int idTarea;
            if (!int.TryParse(celda.Value.ToString(), out idTarea))
            {
                return null;
            }

            return idTarea;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? idSeleccionado = TareaSeleccionadaId();
            if (!idSeleccionado.HasValue)
            {
                MostrarMensajeSeleccioneTarea();
                return;
            }

            Tarea tareaSeleccionada = servicioTarea.Obtener(idSeleccionado.Value);
            if (tareaSeleccionada == null)
            {
                MessageBox.Show(Trad("ListTareas_Msg_NoEncontrada", "No se encontró la tarea."), Trad("ListTareas_Msg_Titulo", "Tareas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AbrirFormularioEdicion(tareaSeleccionada);
        }

        private void MostrarMensajeSeleccioneTarea()
        {
            MessageBox.Show(Trad("ListTareas_Msg_Seleccione", "Seleccione una tarea."), Trad("ListTareas_Msg_Titulo", "Tareas"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AbrirFormularioEdicion(Tarea tareaSeleccionada)
        {
            using (RegistrarTarea formulario = new RegistrarTarea(tareaSeleccionada, RegistrarTarea.ModoFormulario.Editar))
            {
                formulario.StartPosition = FormStartPosition.CenterParent;
                formulario.TopMost = true;
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    CargarTareas();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (authManager == null || !authManager.EstaAutenticado || !authManager.ValidarPermiso("TAREA_ELIMINAR"))
            {
                MessageBox.Show(this, Trad("ListTareas_Permiso_Eliminar", "No tiene permiso para eliminar tareas (TAREA_ELIMINAR)."), Trad("ListTareas_Msg_Titulo", "Tareas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? idSeleccionado = TareaSeleccionadaId();
            if (!idSeleccionado.HasValue)
            {
                MostrarMensajeSeleccioneTarea();
                return;
            }

            Tarea tareaSeleccionada = servicioTarea.Obtener(idSeleccionado.Value);
            if (tareaSeleccionada == null)
            {
                MessageBox.Show(Trad("ListTareas_Msg_NoEncontrada", "No se encontró la tarea."), Trad("ListTareas_Msg_Titulo", "Tareas"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AbrirFormularioEliminacion(tareaSeleccionada);
        }

        private void AbrirFormularioEliminacion(Tarea tareaSeleccionada)
        {
            using (RegistrarTarea formulario = new RegistrarTarea(tareaSeleccionada, RegistrarTarea.ModoFormulario.Eliminar))
            {
                formulario.StartPosition = FormStartPosition.CenterParent;
                formulario.TopMost = true;
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    CargarTareas();
                }
            }
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            int? idSeleccionado = TareaSeleccionadaId();
            if (!idSeleccionado.HasValue)
            {
                MessageBox.Show(Trad("ListTareas_Msg_Seleccione", "Seleccione una tarea."), Trad("ListTareas_Msg_Titulo", "Tareas"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (HistorialListadoTarea formulario = new HistorialListadoTarea(idSeleccionado.Value))
            {
                formulario.StartPosition = FormStartPosition.CenterParent;
                formulario.TopMost = true;
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    CargarTareas();
                }
            }
        }

        private void buttonOrdenarPorScore_Click(object sender, EventArgs e)
        {
            object dataSource = dataGridViewTareas.DataSource;
            List<Tarea> tareasActuales;
            if (dataSource is List<Tarea>)
            {
                tareasActuales = (List<Tarea>)dataSource;
            }
            else
            {
                int usuarioId = ResolverUsuarioParaListado();
                tareasActuales = servicioTarea.ListarPorUsuario(usuarioId);
            }

            IEnumerable<Tarea> consulta = tareasActuales.OrderByDescending(tarea => tarea.ScorePrioridad).ThenBy(tarea => tarea.FechaLimite).ThenByDescending(tarea => tarea.Importancia);

            List<Tarea> tareasOrdenadas = consulta.ToList();

            dataGridViewTareas.DataSource = null;
            dataGridViewTareas.DataSource = tareasOrdenadas;
            dataGridViewTareas.ClearSelection();
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            string titulo = IdiomaService.Instancia.Traducir("ListTareas_Titulo");
            if (!string.IsNullOrWhiteSpace(titulo) && titulo != "ListTareas_Titulo")
            {
                this.Text = titulo;
            }
            else
            {
                this.Text = "Tareas";
            }

            string textoEditar = IdiomaService.Instancia.Traducir("ListTareas_Boton_Editar");
            if (!string.IsNullOrWhiteSpace(textoEditar) && textoEditar != "ListTareas_Boton_Editar")
            {
                btnEditar.Text = textoEditar;
            }

            string textoEliminar = IdiomaService.Instancia.Traducir("ListTareas_Boton_Eliminar");
            if (!string.IsNullOrWhiteSpace(textoEliminar) && textoEliminar != "ListTareas_Boton_Eliminar")
            {
                btnEliminar.Text = textoEliminar;
            }

            string textoHistorial = IdiomaService.Instancia.Traducir("ListTareas_Boton_Historial");
            if (!string.IsNullOrWhiteSpace(textoHistorial) && textoHistorial != "ListTareas_Boton_Historial")
            {
                btnHistorial.Text = textoHistorial;
            }

            string textoOrdenar = IdiomaService.Instancia.Traducir("ListTareas_Boton_OrdenarScore");
            if (!string.IsNullOrWhiteSpace(textoOrdenar) && textoOrdenar != "ListTareas_Boton_OrdenarScore")
            {
                buttonOrdenarPorScore.Text = textoOrdenar;
            }

            SetHeader("colTitulo", "ListTareas_Col_Titulo", "Título");
            SetHeader("colDescripcion", "ListTareas_Col_Descripcion", "Descripción");
            SetHeader("colFechaLimite", "ListTareas_Col_FechaLimite", "Fecha límite");
            SetHeader("colImportancia", "ListTareas_Col_Importancia", "Importancia");
            SetHeader("colEnergia", "ListTareas_Col_Energia", "Energía");
            SetHeader("colDuracion", "ListTareas_Col_Duracion", "Duración (min)");
            SetHeader("colEstado", "ListTareas_Col_Estado", "Estado");
        }

        private void SetHeader(string columnName, string translationKey, string fallbackText)
        {
            DataGridViewColumn columna = dataGridViewTareas.Columns[columnName];
            if (columna == null)
            {
                return;
            }

            string textoTraducido = IdiomaService.Instancia.Traducir(translationKey);
            if (!string.IsNullOrWhiteSpace(textoTraducido) && textoTraducido != translationKey)
            {
                columna.HeaderText = textoTraducido;
            }
            else
            {
                columna.HeaderText = fallbackText;
            }
        }

        private string Trad(string translationKey, string fallbackText)
        {
            string texto = IdiomaService.Instancia.Traducir(translationKey);
            if (string.IsNullOrWhiteSpace(texto) || texto == translationKey)
            {
                return fallbackText;
            }

            return texto;
        }

        private void comboBoxSeleccionarUsuarios_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridViewTareas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}