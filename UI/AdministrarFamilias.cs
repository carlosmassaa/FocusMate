using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BL;
using BE;
using System.Drawing;
using Servicioss;

namespace UI
{
    public partial class AdministrarFamilias : Form, IIdiomaObserver
    {
        private readonly AutorizacionService _autorizacionService = new AutorizacionService();
        private readonly UsuarioBL _usuarioBl = new UsuarioBL();
        private readonly AuthManager _authManager;

        private bool _permFamiliaCrear;
        private bool _permFamiliaEditar;
        private bool _permFamiliaEliminar;

        private bool _permPatenteCrear;
        private bool _permFamiliaAgregarPermiso;
        private bool _permFamiliaQuitarPermiso;

        private sealed class ComponenteGridItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
        }

        public AdministrarFamilias()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            ConfigurarGrids();
            CargarFamilias();
            CargarPermisos();

            dataGridViewFamiliadePermisos.SelectionChanged += (sender, eventArgs) => CargarPermisosDeFamiliaSeleccionada();
            dataGridViewPermisosdeFamilia.CellDoubleClick += dataGridViewPermisosdeFamilia_CellDoubleClick;
            dataGridViewPermisosdeFamilia.KeyDown += dataGridViewPermisosdeFamilia_KeyDown;
        }

        public AdministrarFamilias(AuthManager authManager) : this()
        {
            _authManager = authManager;
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

            AplicarPermisosUI();
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void AplicarPermisosUI()
        {
            _permFamiliaCrear = false;
            _permFamiliaEditar = false;
            _permFamiliaEliminar = false;
            _permPatenteCrear = false;
            _permFamiliaAgregarPermiso = false;
            _permFamiliaQuitarPermiso = false;

            try
            {
                if (_authManager != null && _authManager.EstaAutenticado)
                {
                    _permFamiliaCrear = _authManager.ValidarPermiso("FAMILIA_CREAR");
                    _permFamiliaEditar = _authManager.ValidarPermiso("FAMILIA_EDITAR");
                    _permFamiliaEliminar = _authManager.ValidarPermiso("FAMILIA_ELIMINAR");

                    _permPatenteCrear = _authManager.ValidarPermiso("PATENTE_CREAR");
                    _permFamiliaAgregarPermiso = _authManager.ValidarPermiso("FAMILIA_AGREGAR_PATENTE");
                    _permFamiliaQuitarPermiso = _authManager.ValidarPermiso("FAMILIA_QUITAR_PATENTE");
                }
                else
                {
                    int usuarioId = SesionActual.Instance?.UsuarioId ?? 0;
                    if (usuarioId > 0)
                    {
                        Usuario usuario = _usuarioBl.Obtener(usuarioId);
                        if (usuario != null)
                        {
                            _autorizacionService.CargarPermisosEnUsuario(usuario);
                            _permFamiliaCrear = _autorizacionService.TienePermiso(usuario, "FAMILIA_CREAR");
                            _permFamiliaEditar = _autorizacionService.TienePermiso(usuario, "FAMILIA_EDITAR");
                            _permFamiliaEliminar = _autorizacionService.TienePermiso(usuario, "FAMILIA_ELIMINAR");

                            _permPatenteCrear = _autorizacionService.TienePermiso(usuario, "PATENTE_CREAR");
                            _permFamiliaAgregarPermiso = _autorizacionService.TienePermiso(usuario, "FAMILIA_AGREGAR_PATENTE");
                            _permFamiliaQuitarPermiso = _autorizacionService.TienePermiso(usuario, "FAMILIA_QUITAR_PATENTE");
                        }
                    }
                }
            }
            catch
            {
            }

            if (buttonNuevoFamilia != null)
            {
                buttonNuevoFamilia.Enabled = _permFamiliaCrear;
            }

            if (buttonEditarFamilia != null)
            {
                buttonEditarFamilia.Enabled = _permFamiliaEditar;
            }

            if (buttonEliminarFamilia != null)
            {
                buttonEliminarFamilia.Enabled = _permFamiliaEliminar;
            }

            if (buttonNuevoPermiso != null)
            {
                buttonNuevoPermiso.Enabled = _permPatenteCrear;
            }

            if (buttonAgregarPermiso != null)
            {
                buttonAgregarPermiso.Enabled = _permFamiliaAgregarPermiso;
            }

            if (buttonEliminarPermiso != null)
            {
                buttonEliminarPermiso.Enabled = _permFamiliaQuitarPermiso;
            }
        }

        private void ConfigurarGrids()
        {
            dataGridViewFamiliadePermisos.AutoGenerateColumns = false;
            dataGridViewFamiliadePermisos.Columns.Clear();
            dataGridViewFamiliadePermisos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", Name = "colFamilias_Id", Visible = false });
            dataGridViewFamiliadePermisos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", Name = "colFamilias_Nombre", HeaderText = Trad("AdminFam_Col_Familia", "Familia"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dataGridViewPermisosdeFamilia.AutoGenerateColumns = false;
            dataGridViewPermisosdeFamilia.Columns.Clear();
            dataGridViewPermisosdeFamilia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", Name = "colPF_Id", Visible = false });
            dataGridViewPermisosdeFamilia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", Name = "colPF_Nombre", HeaderText = Trad("AdminFam_Col_Componente", "Componente"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewPermisosdeFamilia.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tipo", Name = "colPF_Tipo", HeaderText = Trad("AdminFam_Col_Tipo", "Tipo"), AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

            dataGridViewPermisos.AutoGenerateColumns = false;
            dataGridViewPermisos.Columns.Clear();
            dataGridViewPermisos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", Name = "colP_Id", Visible = false });
            dataGridViewPermisos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", Name = "colP_Nombre", HeaderText = Trad("AdminFam_Col_Componente", "Componente"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewPermisos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tipo", Name = "colP_Tipo", HeaderText = Trad("AdminFam_Col_Tipo", "Tipo"), AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        private void CargarFamilias()
        {
            List<Familia> familias = new List<Familia>();
            List<Componente> componentesFamilia = _autorizacionService.ListarTodasFamilias();
            foreach (Componente componente in componentesFamilia)
            {
                if (componente is Familia)
                {
                    familias.Add((Familia)componente);
                }
            }

            dataGridViewFamiliadePermisos.DataSource = familias;
            dataGridViewFamiliadePermisos.ClearSelection();
        }

        private void CargarPermisos()
        {
            List<Familia> familias = new List<Familia>();
            List<Componente> componentesFamilia = _autorizacionService.ListarTodasFamilias();
            foreach (Componente componente in componentesFamilia)
            {
                if (componente is Familia)
                {
                    familias.Add((Familia)componente);
                }
            }

            List<Patente> patentes = new List<Patente>();
            List<Componente> componentesPatente = _autorizacionService.ListarTodasPatentes();
            foreach (Componente componente in componentesPatente)
            {
                if (componente is Patente)
                {
                    patentes.Add((Patente)componente);
                }
            }

            List<ComponenteGridItem> componentes = familias.Cast<Componente>().Concat(patentes).Select(component =>
            {
                string tipoComponente;
                if (component is Familia)
                {
                    tipoComponente = "Familia";
                }
                else
                {
                    tipoComponente = "Patente";
                }

                ComponenteGridItem componenteGridItem = new ComponenteGridItem
                {
                    Id = component.Id,
                    Nombre = component.Nombre,
                    Tipo = tipoComponente
                };
                return componenteGridItem;
            }).OrderBy(component => component.Tipo).ThenBy(component => component.Nombre).ToList();

            dataGridViewPermisos.DataSource = componentes;
            dataGridViewPermisos.ClearSelection();
        }

        private int? FamiliaSeleccionadaId()
        {
            if (dataGridViewFamiliadePermisos.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell idCell = dataGridViewFamiliadePermisos.CurrentRow.Cells["colFamilias_Id"];
            if (idCell?.Value == null)
            {
                return null;
            }

            int familiaId;
            if (!int.TryParse(idCell.Value.ToString(), out familiaId))
            {
                return null;
            }

            return familiaId;
        }

        private string FamiliaSeleccionadaNombre()
        {
            if (dataGridViewFamiliadePermisos.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell nombreCell = dataGridViewFamiliadePermisos.CurrentRow.Cells["colFamilias_Nombre"];
            return nombreCell?.Value?.ToString();
        }

        private int? PermisoSeleccionadoIdEnPermisos()
        {
            if (dataGridViewPermisos.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell idCell = dataGridViewPermisos.CurrentRow.Cells["colP_Id"];
            if (idCell?.Value == null)
            {
                return null;
            }

            int componenteId;
            if (!int.TryParse(idCell.Value.ToString(), out componenteId))
            {
                return null;
            }

            return componenteId;
        }

        private int? PermisoSeleccionadoIdEnPermisosDeFamilia()
        {
            if (dataGridViewPermisosdeFamilia.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell idCell = dataGridViewPermisosdeFamilia.CurrentRow.Cells["colPF_Id"];
            if (idCell?.Value == null)
            {
                return null;
            }

            int componenteId;
            if (!int.TryParse(idCell.Value.ToString(), out componenteId))
            {
                return null;
            }

            return componenteId;
        }

        private void CargarPermisosDeFamiliaSeleccionada()
        {
            int? familiaId = FamiliaSeleccionadaId();
            if (!familiaId.HasValue)
            {
                dataGridViewPermisosdeFamilia.DataSource = null;
                return;
            }

            List<ComponenteGridItem> hijos = _autorizacionService.ObtenerHijosFamilia(familiaId.Value).Select(component =>
            {
                string tipoComponente;
                if (component is Familia)
                {
                    tipoComponente = "Familia";
                }
                else
                {
                    tipoComponente = "Patente";
                }

                ComponenteGridItem componenteGridItem = new ComponenteGridItem
                {
                    Id = component.Id,
                    Nombre = component.Nombre,
                    Tipo = tipoComponente
                };
                return componenteGridItem;
            }).OrderBy(component => component.Tipo).ThenBy(component => component.Nombre).ToList();

            dataGridViewPermisosdeFamilia.DataSource = hijos;
            dataGridViewPermisosdeFamilia.ClearSelection();
        }

        private void buttonAgregarPermiso_Click(object sender, EventArgs eventArgs)
        {
            if (!_permFamiliaAgregarPermiso)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_AgregarPermiso", "No tiene permiso (FAMILIA_AGREGAR_PATENTE)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? familiaId = FamiliaSeleccionadaId();
            if (!familiaId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminFam_Msg_SeleccioneFamilia", "Seleccione una familia."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int? componenteId = PermisoSeleccionadoIdEnPermisos();
            if (!componenteId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminFam_Msg_SeleccioneComponente", "Seleccione un componente (Familia o Patente) a agregar."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _autorizacionService.AgregarHijoAFamilia(familiaId.Value, componenteId.Value);
                CargarPermisosDeFamiliaSeleccionada();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("AdminFam_Msg_NoSePudoAgregar", "No se pudo agregar: {0}"), exception.Message), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEliminarPermiso_Click(object sender, EventArgs eventArgs)
        {
            if (!_permFamiliaQuitarPermiso)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_QuitarPermiso", "No tiene permiso (FAMILIA_QUITAR_PATENTE)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            EliminarPermisoDeFamiliaSeleccionado();
        }

        private void dataGridViewPermisosdeFamilia_CellDoubleClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.RowIndex < 0)
            {
                return;
            }

            EliminarPermisoDeFamiliaSeleccionado();
        }

        private void dataGridViewPermisosdeFamilia_KeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.KeyCode == Keys.Delete)
            {
                EliminarPermisoDeFamiliaSeleccionado();
                eventArgs.Handled = true;
            }
        }

        private void EliminarPermisoDeFamiliaSeleccionado()
        {
            if (!_permFamiliaQuitarPermiso)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_QuitarPermiso", "No tiene permiso para eliminar permisos de familias (FAMILIA_QUITAR_PERMISO)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? familiaId = FamiliaSeleccionadaId();
            int? componenteId = PermisoSeleccionadoIdEnPermisosDeFamilia();
            if (!familiaId.HasValue || !componenteId.HasValue)
            {
                return;
            }

            DialogResult respuesta = MessageBox.Show(this, Trad("AdminFam_Msg_QuitarPregunta", "¿Quitar el componente (Familia/Patente) de la familia?"), Trad("AdminFam_Msg_Confirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _autorizacionService.QuitarHijoDeFamilia(familiaId.Value, componenteId.Value);
                CargarPermisosDeFamiliaSeleccionada();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("AdminFam_Msg_NoSePudoQuitar", "No se pudo quitar el componente: {0}"), exception.Message), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonNuevoFamilia_Click(object sender, EventArgs eventArgs)
        {
            if (!_permFamiliaCrear)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_Requerido_Crear", "No tiene permiso para crear familias (FAMILIA_CREAR)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (EditarFamiliaPermiso formulario = new EditarFamiliaPermiso(EditarFamiliaPermiso.ModoEdicion.Agregar, null))
            {
                formulario.Text = Trad("PermFam_Titulo_Agregar", "Agregar familia de permisos");
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    string nombreIngresado = formulario.NombreIngresado?.Trim();
                    if (string.IsNullOrWhiteSpace(nombreIngresado))
                    {
                        MessageBox.Show(this, Trad("AdminFam_Msg_IngreseNombreValido", "Ingrese un nombre válido."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        _autorizacionService.CrearFamilia(nombreIngresado);
                        CargarFamilias();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, string.Format(Trad("AdminFam_Msg_NoSePudoCrear", "No se pudo crear la familia: {0}"), exception.Message), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonEditarFamilia_Click(object sender, EventArgs eventArgs)
        {
            if (!_permFamiliaEditar)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_Requerido_Editar", "No tiene permiso para editar familias (FAMILIA_EDITAR)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? familiaId = FamiliaSeleccionadaId();
            string nombreFamilia = FamiliaSeleccionadaNombre();
            if (!familiaId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminFam_Msg_SeleccioneFamilia", "Seleccione una familia."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (EditarFamiliaPermiso formulario = new EditarFamiliaPermiso(EditarFamiliaPermiso.ModoEdicion.Editar, nombreFamilia))
            {
                formulario.Text = Trad("PermFam_Titulo_Editar", "Editar familia de permisos");
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    string nombreActualizado = formulario.NombreIngresado?.Trim();
                    if (string.IsNullOrWhiteSpace(nombreActualizado))
                    {
                        MessageBox.Show(this, Trad("AdminFam_Msg_IngreseNombreValido", "Ingrese un nombre válido."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        _autorizacionService.ActualizarFamilia(familiaId.Value, nombreActualizado);
                        CargarFamilias();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, string.Format(Trad("AdminFam_Msg_NoSePudoActualizar", "No se pudo actualizar la familia: {0}"), exception.Message), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonEliminarFamilia_Click(object sender, EventArgs eventArgs)
        {
            if (!_permFamiliaEliminar)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_Requerido_Eliminar", "No tiene permiso para eliminar familias (FAMILIA_ELIMINAR)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? familiaId = FamiliaSeleccionadaId();
            string nombreFamilia = FamiliaSeleccionadaNombre();
            if (!familiaId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminFam_Msg_SeleccioneFamilia", "Seleccione una familia."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (EditarFamiliaPermiso formulario = new EditarFamiliaPermiso(EditarFamiliaPermiso.ModoEdicion.Eliminar, nombreFamilia))
            {
                formulario.Text = Trad("PermFam_Titulo_Eliminar", "Eliminar familia de permisos");
                if (formulario.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        _autorizacionService.EliminarFamiliaConAsociaciones(familiaId.Value);
                        CargarFamilias();
                        dataGridViewPermisosdeFamilia.DataSource = null;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(this, string.Format(Trad("AdminFam_Msg_NoSePudoEliminar", "No se pudo eliminar la familia: {0}"), exception.Message), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
            Text = Trad("AdminFam_Titulo", "Administrar Familia de Permisos");
            if (labelTituloAdministrarFamilia != null)
            {
                labelTituloAdministrarFamilia.Text = Trad("AdminFam_Label_Titulo", "Administrar Familia de Permisos");
            }

            if (LabelFamiliadePermisos != null)
            {
                LabelFamiliadePermisos.Text = Trad("AdminFam_Label_Familias", "Familias de permisos");
            }

            if (LabelPermisosUsuario != null)
            {
                LabelPermisosUsuario.Text = Trad("AdminFam_Label_PermisosFamilia", "Permisos Familia");
            }

            if (labelPermisos != null)
            {
                labelPermisos.Text = Trad("AdminFam_Label_Permisos", "Permisos");
            }

            if (buttonAgregarPermiso != null)
            {
                buttonAgregarPermiso.Text = Trad("AdminFam_Boton_AgregarPermiso", "<<< Agregar");
            }

            if (buttonEliminarPermiso != null)
            {
                buttonEliminarPermiso.Text = Trad("AdminFam_Boton_EliminarPermiso", "Eliminar >>>");
            }

            if (buttonNuevoFamilia != null)
            {
                buttonNuevoFamilia.Text = Trad("AdminFam_Boton_Agregar", "Agregar");
            }

            if (buttonEditarFamilia != null)
            {
                buttonEditarFamilia.Text = Trad("AdminFam_Boton_Editar", "Editar");
            }

            if (buttonEliminarFamilia != null)
            {
                buttonEliminarFamilia.Text = Trad("AdminFam_Boton_Eliminar", "Eliminar");
            }

            if (buttonNuevoPermiso != null)
            {
                buttonNuevoPermiso.Text = Trad("AdminFam_Boton_NuevoPermiso", "Nuevo Permiso");
            }

            SetHeader(dataGridViewFamiliadePermisos, "colFamilias_Nombre", "AdminFam_Col_Familia", "Familia");
            SetHeader(dataGridViewPermisosdeFamilia, "colPF_Nombre", "AdminFam_Col_Componente", "Componente");
            SetHeader(dataGridViewPermisosdeFamilia, "colPF_Tipo", "AdminFam_Col_Tipo", "Tipo");
            SetHeader(dataGridViewPermisos, "colP_Nombre", "AdminFam_Col_Componente", "Componente");
            SetHeader(dataGridViewPermisos, "colP_Tipo", "AdminFam_Col_Tipo", "Tipo");
        }

        private void SetHeader(DataGridView grid, string colName, string key, string fallback)
        {
            if (grid == null)
            {
                return;
            }

            DataGridViewColumn column = grid.Columns[colName];
            if (column == null)
            {
                return;
            }

            column.HeaderText = Trad(key, fallback);
        }

        private string Trad(string key, string fallback)
        {
            string textoTraducido = IdiomaService.Instancia.Traducir(key);
            if (string.IsNullOrWhiteSpace(textoTraducido) || textoTraducido == key)
            {
                return fallback;
            }

            return textoTraducido;
        }

        private void buttonNuevoPermiso_Click(object sender, EventArgs eventArgs)
        {
            if (!_permPatenteCrear)
            {
                MessageBox.Show(this, Trad("AdminFam_Permiso_PatenteCrear", "No tiene permiso para crear permisos (PATENTE_CREAR)."), Trad("AdminFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (CrearPermiso formularioPermiso = new CrearPermiso())
            {
                formularioPermiso.StartPosition = FormStartPosition.CenterParent;
                if (formularioPermiso.ShowDialog(this) == DialogResult.OK)
                {
                    CargarPermisos();
                }
            }
        }

        private void AdministrarFamilias_Load(object sender, EventArgs e)
        {

        }
    }
}
