using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class GenerarPlanificacion : Form, IIdiomaObserver
    {
        private readonly PlanificacionBL planificacionBL;
        private readonly UsuarioBL usuarioBL;
        private readonly AuthManager authManager;
        private readonly TareaBL tareaBL = new TareaBL();

        private const string PermisoGenerarPlanificacionUsuarios = "GENERAR_PLANIFICACION_USUARIOS";

        public GenerarPlanificacion()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            planificacionBL = new PlanificacionBL();
            usuarioBL = new UsuarioBL();

            ConfigurarGrid();
            ConfigurarUsuarios();
        }

        public GenerarPlanificacion(AuthManager auth) : this()
        {
            authManager = auth;
            ConfigurarUsuarios();
        }

        protected override void OnLoad(EventArgs eventArgs)
        {
            base.OnLoad(eventArgs);

            IdiomaService.Instancia.Suscribir(this);

            Dictionary<string, string> traduccionesActuales = IdiomaService.Instancia.ObtenerTraduccionesActuales();

            if (traduccionesActuales != null && traduccionesActuales.Count > 0)
            {
                ActualizarTraducciones(traduccionesActuales);
            }
            else
            {
                AplicarTraduccionesEstaticas();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private bool PuedeSeleccionarUsuarios()
        {
            if (authManager == null)
            {
                return false;
            }

            return authManager.ValidarPermiso(PermisoGenerarPlanificacionUsuarios);
        }

        private void ConfigurarUsuarios()
        {
            bool puedeSeleccionarUsuario = PuedeSeleccionarUsuarios();
            comboBoxSeleccionarUsuarios.Visible = puedeSeleccionarUsuario;

            if (puedeSeleccionarUsuario)
            {
                List<Usuario> usuarios = usuarioBL.Listar() ?? new List<Usuario>();

                comboBoxSeleccionarUsuarios.DisplayMember = "NombreUsuario";
                comboBoxSeleccionarUsuarios.ValueMember = "Id";
                comboBoxSeleccionarUsuarios.DataSource = usuarios;

                int usuarioSesionId = SesionActual.Instance.UsuarioId;

                if (usuarioSesionId > 0)
                {
                    comboBoxSeleccionarUsuarios.SelectedValue = usuarioSesionId;
                }
            }
            else
            {
                comboBoxSeleccionarUsuarios.DataSource = null;
            }
        }

        private int ResolverUsuarioParaPlanificacion()
        {
            int usuarioSesionId = SesionActual.Instance.UsuarioId;

            if (PuedeSeleccionarUsuarios())
            {
                if (comboBoxSeleccionarUsuarios.SelectedValue is int usuarioSeleccionadoId && usuarioSeleccionadoId > 0)
                {
                    return usuarioSeleccionadoId;
                }
            }

            return usuarioSesionId;
        }

        private void ConfigurarGrid()
        {
            dataGridViewDetallePlanificacion.AutoGenerateColumns = false;
            dataGridViewDetallePlanificacion.AllowUserToAddRows = false;
            dataGridViewDetallePlanificacion.AllowUserToDeleteRows = false;
            dataGridViewDetallePlanificacion.ReadOnly = true;
            dataGridViewDetallePlanificacion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDetallePlanificacion.MultiSelect = false;
            dataGridViewDetallePlanificacion.Columns.Clear();

            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName = "Orden",Name = "colOrden",HeaderText = Trad("GenPlan_Col_Orden", "Orden")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName = "TareaId",Name = "colTareaId",HeaderText = "TareaId",Visible = false});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Titulo",Name = "colTitulo",HeaderText = Trad("GenPlan_Col_Titulo", "Título"),AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "FechaLimite",Name = "colFechaLimite",HeaderText = Trad("GenPlan_Col_FechaLimite", "Fecha límite")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Importancia",Name = "colImportancia",HeaderText = Trad("GenPlan_Col_Importancia", "Importancia")});  
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName = "EnergiaRequerida",Name = "colEnergia",HeaderText = Trad("GenPlan_Col_Energia", "Energía")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "DuracionEstimadaMin",Name = "colDuracion",HeaderText = Trad("GenPlan_Col_Duracion", "Duración")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "ScorePrioridad",Name = "colScore",HeaderText = Trad("GenPlan_Col_Score", "Score")});
        }

        private void btnGenerarPlanificacion_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioId = ResolverUsuarioParaPlanificacion();

                if (usuarioId <= 0)
                {
                    MessageBox.Show(Trad("GenPlan_Msg_SinSesion", "No hay sesión activa."),Trad("GenPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                Planificacion planificacion = planificacionBL.GenerarPlanificacion(usuarioId);

                List<object> filas = new List<object>();

                foreach (PlanificacionDetalle detalle in planificacion.Detalles)
                {
                    if (detalle.Tarea != null)
                    {
                        filas.Add(new {Orden = detalle.Orden,TareaId = detalle.TareaId,Titulo = detalle.Tarea.Titulo,FechaLimite = detalle.Tarea.FechaLimite,Importancia = detalle.Tarea.Importancia,EnergiaRequerida = detalle.Tarea.EnergiaRequerida,DuracionEstimadaMin = detalle.Tarea.DuracionEstimadaMin,ScorePrioridad = detalle.ScorePrioridad});
                    }
                }

                dataGridViewDetallePlanificacion.DataSource = null;
                dataGridViewDetallePlanificacion.DataSource = filas;
                dataGridViewDetallePlanificacion.AutoResizeColumns();

                MessageBox.Show(Trad("GenPlan_Msg_GeneradaOk", "La planificación fue generada correctamente."),Trad("GenPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("fecha límite anterior a hoy"))
                {
                    DialogResult respuesta = MessageBox.Show(Trad("GenPlan_Msg_FechasVencidas", "Existen tareas con fecha límite anterior a hoy.\n\nSí: actualizar automáticamente esas fechas a hoy.\nNo: abrir el listado de tareas para editarlas manualmente.\nCancelar: no realizar cambios."), Trad("GenPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning);

                    if (respuesta == DialogResult.Yes)
                    {
                        try
                        {
                            int usuarioId = ResolverUsuarioParaPlanificacion();
                            int cantidadActualizada = tareaBL.ActualizarFechasVencidasAHoy(usuarioId);

                            MessageBox.Show(string.Format(Trad("GenPlan_Msg_FechasActualizadas", "Se actualizaron {0} tareas vencidas a la fecha de hoy. Ahora puede generar la planificación nuevamente."),cantidadActualizada),Trad("GenPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        catch (Exception excepcionActualizacion)
                        {
                            MessageBox.Show(excepcionActualizacion.Message,Trad("GenPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Error);
                        }

                        return;
                    }

                    if (respuesta == DialogResult.No)
                    {
                        using (ListadoTareas formulario = new ListadoTareas(authManager))
                        {
                            formulario.StartPosition = FormStartPosition.CenterParent;
                            formulario.ShowDialog(this);
                        }

                        return;
                    }

                    return;
                }

                MessageBox.Show(exception.Message,Trad("GenPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("GenPlan_Titulo", "Generar Planificación");

            if (lblTitulo != null)
            {
                lblTitulo.Text = Trad("GenPlan_Label_Titulo", "Generar planificación priorizada");
            }

            if (btnGenerarPlanificacion != null)
            {
                btnGenerarPlanificacion.Text = Trad("GenPlan_Boton_Generar", "Generar planificación");
            }

            if (btnCerrar != null)
            {
                btnCerrar.Text = Trad("GenPlan_Boton_Cerrar", "Cerrar");
            }

            SetHeader(dataGridViewDetallePlanificacion, "colOrden", "GenPlan_Col_Orden", "Orden");
            SetHeader(dataGridViewDetallePlanificacion, "colTitulo", "GenPlan_Col_Titulo", "Título");
            SetHeader(dataGridViewDetallePlanificacion, "colFechaLimite", "GenPlan_Col_FechaLimite", "Fecha límite");
            SetHeader(dataGridViewDetallePlanificacion, "colImportancia", "GenPlan_Col_Importancia", "Importancia");
            SetHeader(dataGridViewDetallePlanificacion, "colEnergia", "GenPlan_Col_Energia", "Energía");
            SetHeader(dataGridViewDetallePlanificacion, "colDuracion", "GenPlan_Col_Duracion", "Duración");
            SetHeader(dataGridViewDetallePlanificacion, "colScore", "GenPlan_Col_Score", "Score");
        }

        private void SetHeader(DataGridView grid, string nombreColumna, string clave, string textoPredeterminado)
        {
            if (grid == null)
            {
                return;
            }

            if (!grid.Columns.Contains(nombreColumna))
            {
                return;
            }

            grid.Columns[nombreColumna].HeaderText = Trad(clave, textoPredeterminado);
        }

        private string Trad(string clave, string textoPredeterminado)
        {
            string textoTraducido = IdiomaService.Instancia.Traducir(clave);

            if (string.IsNullOrWhiteSpace(textoTraducido) || textoTraducido == clave)
            {
                return textoPredeterminado;
            }

            return textoTraducido;
        }

        private void comboBoxSeleccionarUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewDetallePlanificacion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {

        }

        private void GenerarPlanificacion_Load(object sender, EventArgs e)
        {

        }
    }
}