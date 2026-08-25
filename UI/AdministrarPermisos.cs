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
    public partial class AdministrarPermisos : Form, IIdiomaObserver
    {
        private readonly AutorizacionService _svc = new AutorizacionService();
        private readonly UsuarioBL _usuarioSrv = new UsuarioBL();
        private readonly AuthManager _authManager;

        private bool _permPatenteCrear;
        private bool _permUsuarioAsignar;
        private bool _permUsuarioQuitar;

        private sealed class ComponenteGridItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
        }

        public AdministrarPermisos()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ConfigurarGrids();
            CargarUsuarios();
            CargarPermisos();

            dataGridViewUsuarios.SelectionChanged += (object sender, EventArgs args) =>
            {
                CargarPermisosDeUsuarioSeleccionado();
            };

            dataGridViewPermiossUsuario.CellDoubleClick += dataGridViewPermiossUsuario_CellDoubleClick;
            dataGridViewPermiossUsuario.KeyDown += dataGridViewPermiossUsuario_KeyDown;
            dataGridViewPermisos.SelectionChanged += dataGridViewPermisos_SelectionChanged;
        }

        public AdministrarPermisos(AuthManager authManager) : this()
        {
            _authManager = authManager;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IdiomaService.Instancia.Suscribir(this);
            Dictionary<string, string> actuales = IdiomaService.Instancia.ObtenerTraduccionesActuales();

            if (actuales != null && actuales.Count > 0)
            {
                ActualizarTraducciones(actuales);
            }
            else
            {
                AplicarTraduccionesEstaticas();
            }

            AplicarPermisosUI();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void AplicarPermisosUI()
        {
            _permPatenteCrear = false;
            _permUsuarioAsignar = false;
            _permUsuarioQuitar = false;

            try
            {
                if (_authManager != null && _authManager.EstaAutenticado)
                {
                    _permPatenteCrear = _authManager.ValidarPermiso("PATENTE_CREAR");
                    _permUsuarioAsignar = _authManager.ValidarPermiso("USUARIO_ASIGNAR_PATENTE_FAMILIAS");
                    _permUsuarioQuitar = _authManager.ValidarPermiso("USUARIO_QUITAR_PATENTE_FAMILIAS");
                }
                else
                {
                    int usuarioId = SesionActual.Instance?.UsuarioId ?? 0;

                    if (usuarioId > 0)
                    {
                        Usuario usuario = _usuarioSrv.Obtener(usuarioId);

                        if (usuario != null)
                        {
                            _svc.CargarPermisosEnUsuario(usuario);
                            _permPatenteCrear = _svc.TienePermiso(usuario, "PATENTE_CREAR");
                            _permUsuarioAsignar = _svc.TienePermiso(usuario, "USUARIO_ASIGNAR_PATENTE_FAMILIAS");
                            _permUsuarioQuitar = _svc.TienePermiso(usuario, "USUARIO_QUITAR_PATENTE_FAMILIAS");
                        }
                    }
                }
            }
            catch
            {
            }

            if (buttonNuevoPermiso != null)
            {
                buttonNuevoPermiso.Enabled = _permPatenteCrear;
            }

            if (buttonAgregarPermiso != null)
            {
                buttonAgregarPermiso.Enabled = _permUsuarioAsignar;
            }

            if (buttonEliminarPermiso != null)
            {
                buttonEliminarPermiso.Enabled = _permUsuarioQuitar;
            }
        }

        private void ConfigurarGrids()
        {
            dataGridViewUsuarios.AutoGenerateColumns = false;
            dataGridViewUsuarios.Columns.Clear();
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "colUsr_Id",
                Visible = false
            });
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NombreUsuario",
                Name = "colUsr_Nombre",
                HeaderText = Trad("AdminPerm_Col_Usuario", "Usuario"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dataGridViewPermiossUsuario.AutoGenerateColumns = false;
            dataGridViewPermiossUsuario.Columns.Clear();
            dataGridViewPermiossUsuario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "colPU_Id",
                Visible = false
            });
            dataGridViewPermiossUsuario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                Name = "colPU_Nombre",
                HeaderText = Trad("AdminPerm_Col_Componente", "Componente"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewPermiossUsuario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Tipo",
                Name = "colPU_Tipo",
                HeaderText = Trad("AdminPerm_Col_Tipo", "Tipo"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridViewPermisos.AutoGenerateColumns = false;
            dataGridViewPermisos.Columns.Clear();
            dataGridViewPermisos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "colP_Id",
                Visible = false
            });
            dataGridViewPermisos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                Name = "colP_Nombre",
                HeaderText = Trad("AdminPerm_Col_Componente", "Componente"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewPermisos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Tipo",
                Name = "colP_Tipo",
                HeaderText = Trad("AdminPerm_Col_Tipo", "Tipo"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });

            dataGridViewPermisosPorFamilia.AutoGenerateColumns = false;
            dataGridViewPermisosPorFamilia.Columns.Clear();
            dataGridViewPermisosPorFamilia.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                Name = "colPF2_Id",
                Visible = false
            });
            dataGridViewPermisosPorFamilia.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                Name = "colPF2_Nombre",
                HeaderText = Trad("AdminPerm_Col_Componente", "Componente"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dataGridViewPermisosPorFamilia.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Tipo",
                Name = "colPF2_Tipo",
                HeaderText = Trad("AdminPerm_Col_Tipo", "Tipo"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
        }

        private void CargarUsuarios()
        {
            List<Usuario> usuarios = _usuarioSrv.Listar();
            dataGridViewUsuarios.DataSource = usuarios;
            dataGridViewUsuarios.ClearSelection();
        }

        private void CargarPermisos()
        {
            List<Familia> familias = new List<Familia>();
            List<Componente> componentesFamilia = _svc.ListarTodasFamilias();

            for (int index = 0; index < componentesFamilia.Count; index++)
            {
                if (componentesFamilia[index] is Familia)
                {
                    familias.Add((Familia)componentesFamilia[index]);
                }
            }

            List<Patente> patentes = new List<Patente>();
            List<Componente> componentesPatente = _svc.ListarTodasPatentes();

            for (int index = 0; index < componentesPatente.Count; index++)
            {
                if (componentesPatente[index] is Patente)
                {
                    patentes.Add((Patente)componentesPatente[index]);
                }
            }

            List<ComponenteGridItem> componentes = familias
                .Cast<Componente>()
                .Concat(patentes)
                .Select(component =>
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

                    ComponenteGridItem item = new ComponenteGridItem
                    {
                        Id = component.Id,
                        Nombre = component.Nombre,
                        Tipo = tipoComponente
                    };

                    return item;
                })
                .OrderBy(component => component.Tipo)
                .ThenBy(component => component.Nombre)
                .ToList();

            dataGridViewPermisos.DataSource = componentes;
            dataGridViewPermisos.ClearSelection();
            dataGridViewPermisosPorFamilia.DataSource = null;
        }

        private int? UsuarioSeleccionadoId()
        {
            if (dataGridViewUsuarios.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell idCell = dataGridViewUsuarios.CurrentRow.Cells["colUsr_Id"];

            if (idCell?.Value == null)
            {
                return null;
            }

            int usuarioId;

            if (!int.TryParse(idCell.Value.ToString(), out usuarioId))
            {
                return null;
            }

            return usuarioId;
        }

        private int? PermisoSeleccionadoId()
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

        private int? ComponenteAsignadoSeleccionadoId()
        {
            if (dataGridViewPermiossUsuario.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell idCell = dataGridViewPermiossUsuario.CurrentRow.Cells["colPU_Id"];

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

        private string ComponenteAsignadoSeleccionadoNombre()
        {
            if (dataGridViewPermiossUsuario.CurrentRow == null)
            {
                return null;
            }

            return dataGridViewPermiossUsuario.CurrentRow.Cells["colPU_Nombre"]?.Value?.ToString();
        }

        private void CargarPermisosDeUsuarioSeleccionado()
        {
            int? usuarioId = UsuarioSeleccionadoId();

            if (!usuarioId.HasValue)
            {
                dataGridViewPermiossUsuario.DataSource = null;
                return;
            }

            List<Componente> componentesAsignados = _svc.ObtenerAsignacionesUsuario(usuarioId.Value).ToList();

            List<ComponenteGridItem> componentes = componentesAsignados
                .Select(component =>
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

                    ComponenteGridItem item = new ComponenteGridItem
                    {
                        Id = component.Id,
                        Nombre = component.Nombre,
                        Tipo = tipoComponente
                    };

                    return item;
                })
                .OrderBy(component => component.Tipo)
                .ThenBy(component => component.Nombre)
                .ToList();

            dataGridViewPermiossUsuario.DataSource = componentes;
            dataGridViewPermiossUsuario.ClearSelection();
        }

        private void buttonAgregarPermiso_Click(object sender, EventArgs e)
        {
            if (!_permUsuarioAsignar)
            {
                MessageBox.Show(this, Trad("AdminPerm_Permiso_Asignar", "No tiene permiso para asignar permisos/familias a usuarios (USUARIO_ASIGNAR_PATENTE_FAMILIAS)."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? usuarioId = UsuarioSeleccionadoId();

            if (!usuarioId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminPerm_Msg_SeleccioneUsuario", "Seleccione un usuario."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int? componenteId = PermisoSeleccionadoId();

            if (!componenteId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminPerm_Msg_SeleccioneComponente", "Seleccione un componente (Familia o Patente) a agregar."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _svc.AsignarComponenteAUsuario(usuarioId.Value, componenteId.Value);
                CargarPermisosDeUsuarioSeleccionado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Trad("AdminPerm_Msg_NoSePudoAsignar", "No se pudo asignar: {0}"), ex.Message), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewPermiossUsuario_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            EliminarAsignacionUsuarioSeleccionada();
        }

        private void dataGridViewPermiossUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                EliminarAsignacionUsuarioSeleccionada();
                e.Handled = true;
            }
        }

        private void EliminarAsignacionUsuarioSeleccionada()
        {
            if (!_permUsuarioQuitar)
            {
                MessageBox.Show(this, Trad("AdminPerm_Permiso_Quitar", "No tiene permiso para quitar permisos/familias de usuarios (USUARIO_QUITAR_PATENTE_FAMILIAS)."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? usuarioId = UsuarioSeleccionadoId();

            if (!usuarioId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminPerm_Msg_SeleccioneUsuario", "Seleccione un usuario."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int? componenteId = ComponenteAsignadoSeleccionadoId();

            if (!componenteId.HasValue)
            {
                MessageBox.Show(this, Trad("AdminPerm_Msg_SeleccioneComponente", "Seleccione un componente (Familia o Patente) a quitar."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string nombre = ComponenteAsignadoSeleccionadoNombre();
            string confirm = Trad("AdminPerm_Msg_QuitarPregunta", "¿Quitar el componente (Familia/Patente) del usuario?");

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                confirm += Environment.NewLine + "[" + nombre + "]";
            }

            DialogResult respuesta = MessageBox.Show(this, confirm, Trad("AdminPerm_Msg_Confirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _svc.QuitarComponenteDeUsuario(usuarioId.Value, componenteId.Value);
                CargarPermisosDeUsuarioSeleccionado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Trad("AdminPerm_Msg_NoSePudoQuitar", "No se pudo quitar: {0}"), ex.Message), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEliminarPermiso_Click(object sender, EventArgs e)
        {
            EliminarAsignacionUsuarioSeleccionada();
        }

        private void dataGridViewPermisos_SelectionChanged(object sender, EventArgs e)
        {
            CargarPermisosPorFamiliaSeleccionada();
        }

        private void CargarPermisosPorFamiliaSeleccionada()
        {
            dataGridViewPermisosPorFamilia.DataSource = null;

            if (dataGridViewPermisos.CurrentRow == null)
            {
                return;
            }

            string tipo = dataGridViewPermisos.CurrentRow.Cells["colP_Tipo"]?.Value?.ToString();
            object idObj = dataGridViewPermisos.CurrentRow.Cells["colP_Id"]?.Value;

            if (!string.Equals(tipo, "Familia", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (idObj == null)
            {
                return;
            }

            int familiaId;

            if (!int.TryParse(idObj.ToString(), out familiaId))
            {
                return;
            }

            List<ComponenteGridItem> items = ListarComponentesRecursivosDeFamilia(familiaId);
            dataGridViewPermisosPorFamilia.DataSource = items;
            dataGridViewPermisosPorFamilia.ClearSelection();
        }

        private List<ComponenteGridItem> ListarComponentesRecursivosDeFamilia(int idFamilia)
        {
            List<ComponenteGridItem> lista = new List<ComponenteGridItem>();
            HashSet<int> visitadas = new HashSet<int>();

            AgregarDescendientes(idFamilia, lista, visitadas);

            return lista
                .OrderBy(component => component.Tipo)
                .ThenBy(component => component.Nombre)
                .ToList();
        }

        private void AgregarDescendientes(int idFamilia, List<ComponenteGridItem> lista, HashSet<int> visitadas)
        {
            if (visitadas.Contains(idFamilia))
            {
                return;
            }

            visitadas.Add(idFamilia);

            List<Componente> hijos = _svc.ObtenerHijosFamilia(idFamilia) ?? new List<Componente>();

            foreach (Componente hijo in hijos)
            {
                string tipoComponente;

                if (hijo is Familia)
                {
                    tipoComponente = "Familia";
                }
                else
                {
                    tipoComponente = "Patente";
                }

                lista.Add(new ComponenteGridItem
                {
                    Id = hijo.Id,
                    Nombre = hijo.Nombre,
                    Tipo = tipoComponente
                });

                if (hijo is Familia)
                {
                    AgregarDescendientes(hijo.Id, lista, visitadas);
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
            Text = Trad("AdminPerm_Titulo", "Administrar Permisos");

            if (labelAdministrarPermisos != null)
            {
                labelAdministrarPermisos.Text = Trad("AdminPerm_Label_Titulo", "Administrar Permisos");
            }

            if (labelUsuariosTitulo != null)
            {
                labelUsuariosTitulo.Text = Trad("AdminPerm_Label_Usuarios", "Usuarios");
            }

            if (LabelPermisosUsuario != null)
            {
                LabelPermisosUsuario.Text = Trad("AdminPerm_Label_PermisosUsuario", "Permisos Usuarios");
            }

            if (labelPermisos != null)
            {
                labelPermisos.Text = Trad("AdminPerm_Label_Permisos", "Permisos");
            }

            if (labelPermisosPorFamilia != null)
            {
                labelPermisosPorFamilia.Text = Trad("AdminPerm_Label_PermisosFamiliaDetalle", "Permisos de la familia");
            }

            if (buttonAgregarPermiso != null)
            {
                buttonAgregarPermiso.Text = Trad("AdminPerm_Boton_AgregarPermiso", "<<< Agregar");
            }

            if (buttonEliminarPermiso != null)
            {
                buttonEliminarPermiso.Text = Trad("AdminPerm_Boton_EliminarPermiso", "Eliminar >>>");
            }

            if (buttonNuevoPermiso != null)
            {
                buttonNuevoPermiso.Text = Trad("AdminPerm_Boton_NuevoPermiso", "Nuevo Permiso");
            }
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

        private void buttonNuevoPermiso_Click(object sender, EventArgs e)
        {
            if (!_permPatenteCrear)
            {
                MessageBox.Show(this, Trad("AdminPerm_Permiso_PatenteCrear", "No tiene permiso para crear permisos (PATENTE_CREAR)."), Trad("AdminPerm_Msg_Titulo", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (CrearPermiso formularioCrearPermiso = new CrearPermiso())
            {
                formularioCrearPermiso.StartPosition = FormStartPosition.CenterParent;

                if (formularioCrearPermiso.ShowDialog(this) == DialogResult.OK)
                {
                    CargarPermisos();
                }
            }
        }
    }
}
