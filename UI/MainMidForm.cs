using System;
using System.Linq;
using System.Windows.Forms;
using BL;
using Servicioss;
using BE;
using System.Drawing;
using System.Collections.Generic;

namespace UI
{
    public partial class MainMidForm : Form, IIdiomaObserver
    {
        private readonly AuthManager _authManager;
        private readonly BitacoraBL _servicioBitacora;
        private readonly BackupBL _backupBl;
        private const string PatenteBitacora = "AUDITORIA_BITACORA";
        private const string PermisoAccederFamilias = "ACCEDER_GESTIONAR_FAMILIAS";
        private const string PermisoAccederPermisos = "ACCEDER_GESTIONAR_PERMISOS";
        private const string PermisoAccederUsuarios = "ACCEDER_GESTIONAR_USUARIOS";
        private const string PermisoAccederBackup = "ACCEDER_GESTIONAR_BACKUP";
        private const string PermisoGenerarPlanificacion = "GENERAR_PLANIFICACION";
        private const string PermisoGestionarPlanificaciones = "GESTIONAR_PLANIFICACIONES";
        private const string PermisoVerTop10 = "VER_TOP10";
        private const string PermisoGestionarTiempoDisponible = "GESTIONAR_TIEMPO_DISPONIBLE";

        public MainMidForm()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;

            _backupBl = BackupBL.CrearBasico();
        }

        public MainMidForm(AuthManager authManager, BitacoraBL servicioBitacora) : this()
        {
            _authManager = authManager;
            _servicioBitacora = servicioBitacora;
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

            ActualizarTitulo();
            HabilitarOpcionesSegunPermisos();
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void ActualizarTitulo()
        {
            string baseTitle = IdiomaService.Instancia.Traducir("Main_Titulo_Base");
            if (string.IsNullOrWhiteSpace(baseTitle) || baseTitle == "Main_Titulo_Base")
            {
                baseTitle = "Gestión de Productividad";
            }

            string nombreUsuario = _authManager.UsuarioActual?.NombreUsuario ?? SesionActual.Instance?.NombreUsuario;
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                Text = baseTitle;
            }
            else
            {
                string formato = IdiomaService.Instancia.Traducir("Main_Titulo_Usuario");
                if (string.IsNullOrWhiteSpace(formato) || formato == "Main_Titulo_Usuario")
                {
                    formato = "Gestión de Productividad - Usuario: {0}";
                }

                Text = string.Format(formato, nombreUsuario);
            }
        }

        private void HabilitarOpcionesSegunPermisos()
        {
            bool autenticado = _authManager != null && _authManager.EstaAutenticado;

            bool tieneBitacora = autenticado && _authManager.ValidarPermiso(PatenteBitacora) && _servicioBitacora != null;
            bitacoraDeEventosToolStripMenuItem.Enabled = tieneBitacora;

            bool puedeVerTreeView = autenticado && _authManager.ValidarPermiso("ACCESO_TREEVIEW");
            treeViewPermisosToolStripMenuItem.Enabled = puedeVerTreeView;

            bool puedeAccederFamilias = autenticado && _authManager.ValidarPermiso(PermisoAccederFamilias);
            administrarFamiliasToolStripMenuItem.Enabled = puedeAccederFamilias;

            bool puedeAccederPermisos = autenticado && _authManager.ValidarPermiso(PermisoAccederPermisos);
            administrarPermisosToolStripMenuItem.Enabled = puedeAccederPermisos;

            bool puedeCargarIdioma = autenticado && _authManager.ValidarPermiso("IDIOMA_CARGAR_EDITAR");
            cargarIdiomaToolStripMenuItem.Enabled = puedeCargarIdioma;

            bool puedeGestionarUsuarios = autenticado && _authManager.ValidarPermiso(PermisoAccederUsuarios);
            if (gestionarUsuarioToolStripMenuItem != null)
            {
                gestionarUsuarioToolStripMenuItem.Enabled = puedeGestionarUsuarios;
                gestionarUsuarioToolStripMenuItem.Visible = true;
            }

            bool puedeAccederBackup = autenticado && _authManager.ValidarPermiso(PermisoAccederBackup);
            if (administrarBuckupToolStripMenuItem != null)
            {
                administrarBuckupToolStripMenuItem.Enabled = puedeAccederBackup;
                administrarBuckupToolStripMenuItem.Visible = true;
            }

            bool puedeGenerarPlanificacion = autenticado && _authManager.ValidarPermiso(PermisoGenerarPlanificacion);
            if (generarPlanificacionToolStripMenuItem != null)
            {
                generarPlanificacionToolStripMenuItem.Enabled = puedeGenerarPlanificacion;
            }

            bool puedeGestionarPlanificaciones = autenticado && _authManager.ValidarPermiso(PermisoGestionarPlanificaciones);
            if (gestionarPlanificacionesToolStripMenuItem != null)
            {
                gestionarPlanificacionesToolStripMenuItem.Enabled = puedeGestionarPlanificaciones;
            }

            bool puedeVerTop10 = autenticado && _authManager.ValidarPermiso(PermisoVerTop10);
            if (top10ToolStripMenuItem != null)
            {
                top10ToolStripMenuItem.Enabled = puedeVerTop10;
            }

            bool puedeGestionarTiempoDisponible = autenticado && _authManager.ValidarPermiso(PermisoGestionarTiempoDisponible);
            if (tiempoDisponibleToolStripMenuItem != null)
            {
                tiempoDisponibleToolStripMenuItem.Enabled = puedeGestionarTiempoDisponible;
            }
        }

        private void bitacoraDeEventosToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (!bitacoraDeEventosToolStripMenuItem.Enabled)
            {
                return;
            }

            FrmBitacoraEventos formularioAbierto = MdiChildren.OfType<FrmBitacoraEventos>().FirstOrDefault();
            if (formularioAbierto != null)
            {
                formularioAbierto.BringToFront();
                formularioAbierto.Activate();
                return;
            }

            FrmBitacoraEventos formulario = new FrmBitacoraEventos(_servicioBitacora)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            formulario.Show();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            string pregunta = IdiomaService.Instancia.Traducir("Main_Msg_CerrarSesion_Pregunta");
            if (string.IsNullOrWhiteSpace(pregunta) || pregunta == "Main_Msg_CerrarSesion_Pregunta")
            {
                pregunta = "¿Desea cerrar la sesión actual?";
            }

            string titulo = IdiomaService.Instancia.Traducir("Main_Msg_Confirmar");
            if (string.IsNullOrWhiteSpace(titulo) || titulo == "Main_Msg_Confirmar")
            {
                titulo = "Confirmar";
            }

            DialogResult respuesta = MessageBox.Show(this, pregunta, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _authManager?.Logout();
            }
            catch
            {
            }

            Close();
        }

        private void administraciónToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
        }

        private void crearTareaToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            using (RegistrarTarea registrarTareaFormulario = new RegistrarTarea())
            {
                registrarTareaFormulario.StartPosition = FormStartPosition.CenterParent;
                registrarTareaFormulario.ShowDialog(this);
            }
        }

        private void verTareasToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                TareaBL tareaBl = new TareaBL();
                string detalleDvh;
                bool dvhOk = tareaBl.VerificarDVH_Tarea(out detalleDvh);

                IntegridadBL integridadBl = new IntegridadBL();
                string detalleDvv;
                bool dvvOk = integridadBl.VerificarTarea(out detalleDvv);

                if (!dvhOk || !dvvOk)
                {
                    string mensaje = IdiomaService.Instancia.Traducir("Main_Msg_Integridad_Fallo");
                    if (string.IsNullOrWhiteSpace(mensaje) || mensaje == "Main_Msg_Integridad_Fallo")
                    {
                        mensaje = "Se detectaron errores de integridad (DVH/DVV) en las tareas. La sesión será cerrada.";
                    }

                    string detalle = (detalleDvh + " " + detalleDvv).Trim();

                    if (_servicioBitacora != null)
                    {
                        string usuario = _authManager?.UsuarioActual?.NombreUsuario ?? SesionActual.Instance?.NombreUsuario ?? string.Empty;
                        _servicioBitacora.Registrar("VER_TAREAS_INTEGRIDAD", "Tarea", "FAIL", usuario, "Integridad", detalle);
                    }

                    MessageBox.Show(this, mensaje + Environment.NewLine + detalle, "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    try
                    {
                        _authManager?.Logout();
                    }
                    catch
                    {
                    }
                    Close();
                    return;
                }
            }
            catch (Exception exception)
            {
                string mensajeError = IdiomaService.Instancia.Traducir("Main_Msg_Integridad_Error");
                if (string.IsNullOrWhiteSpace(mensajeError) || mensajeError == "Main_Msg_Integridad_Error")
                {
                    mensajeError = "Ocurrió un error al verificar la integridad. La sesión será cerrada.";
                }

                if (_servicioBitacora != null)
                {
                    string usuario = _authManager?.UsuarioActual?.NombreUsuario ?? SesionActual.Instance?.NombreUsuario ?? string.Empty;
                    _servicioBitacora.Registrar("VER_TAREAS_INTEGRIDAD", "Tarea", "FAIL", usuario, "Integridad", "Excepción=" + exception.Message);
                }

                MessageBox.Show(this, mensajeError, "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    _authManager?.Logout();
                }
                catch
                {
                }
                Close();
                return;
            }

            ListadoTareas listadoTareasFormulario = new ListadoTareas(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            listadoTareasFormulario.Show();
        }

        private void treeViewPermisosToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso("ACCESO_TREEVIEW"))
            {
                return;
            }

            FrmPermisos permisosFormularioAbierto = Application.OpenForms.OfType<FrmPermisos>().FirstOrDefault();
            if (permisosFormularioAbierto != null)
            {
                permisosFormularioAbierto.BringToFront();
                permisosFormularioAbierto.Activate();
                return;
            }

            using (FrmPermisos permisosFormulario = new FrmPermisos(_authManager))
            {
                permisosFormulario.StartPosition = FormStartPosition.CenterParent;
                permisosFormulario.ShowDialog(this);
            }
        }

        private void administrarPermisosToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoAccederPermisos))
            {
                return;
            }

            AdministrarPermisos administrarPermisosAbierto = MdiChildren.OfType<AdministrarPermisos>().FirstOrDefault();
            if (administrarPermisosAbierto != null)
            {
                administrarPermisosAbierto.BringToFront();
                administrarPermisosAbierto.Activate();
                return;
            }

            AdministrarPermisos administrarPermisosFormulario = new AdministrarPermisos(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            administrarPermisosFormulario.Show();
        }

        private void administrarFamiliasToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
        }

        private void administrarFamiliasToolStripMenuItem_Click_1(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoAccederFamilias))
            {
                return;
            }

            AdministrarFamilias administrarFamiliasAbierto = MdiChildren.OfType<AdministrarFamilias>().FirstOrDefault();
            if (administrarFamiliasAbierto != null)
            {
                administrarFamiliasAbierto.BringToFront();
                administrarFamiliasAbierto.Activate();
                return;
            }

            AdministrarFamilias administrarFamiliasFormulario = new AdministrarFamilias(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            administrarFamiliasFormulario.Show();
        }

        private void MainMidForm_Load(object sender, EventArgs eventArgs)
        {
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            using (CambiarIdioma cambiarIdiomaFormulario = new CambiarIdioma(_authManager))
            {
                cambiarIdiomaFormulario.StartPosition = FormStartPosition.CenterParent;
                cambiarIdiomaFormulario.ShowDialog(this);
            }
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            ActualizarTitulo();

            if (traducciones == null)
            {
                return;
            }

            if (tareasToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_Tareas"))
            {
                tareasToolStripMenuItem.Text = traducciones["Main_Menu_Tareas"];
            }

            if (bitacoraDeEventosToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_Bitacora"))
            {
                bitacoraDeEventosToolStripMenuItem.Text = traducciones["Main_Menu_Bitacora"];
            }

            if (treeViewPermisosToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_TreeViewPermisos"))
            {
                treeViewPermisosToolStripMenuItem.Text = traducciones["Main_Menu_TreeViewPermisos"];
            }

            if (administraciónToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_Administracion"))
            {
                administraciónToolStripMenuItem.Text = traducciones["Main_Menu_Administracion"];
            }

            if (administrarPermisosToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_AdministrarPermisos"))
            {
                administrarPermisosToolStripMenuItem.Text = traducciones["Main_Menu_AdministrarPermisos"];
            }

            if (administrarFamiliasToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_AdministrarFamilias"))
            {
                administrarFamiliasToolStripMenuItem.Text = traducciones["Main_Menu_AdministrarFamilias"];
            }

            if (administrarBuckupToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_AdministrarBackup"))
            {
                administrarBuckupToolStripMenuItem.Text = traducciones["Main_Menu_AdministrarBackup"];
            }

            if (gestionarUsuarioToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_GestionarUsuario"))
            {
                gestionarUsuarioToolStripMenuItem.Text = traducciones["Main_Menu_GestionarUsuario"];
            }

            if (cargarIdiomaToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_CargarIdioma"))
            {
                cargarIdiomaToolStripMenuItem.Text = traducciones["Main_Menu_CargarIdioma"];
            }

            if (crearTareaToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_CrearTarea"))
            {
                crearTareaToolStripMenuItem.Text = traducciones["Main_Menu_CrearTarea"];
            }

            if (verTareasToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_VerTareas"))
            {
                verTareasToolStripMenuItem.Text = traducciones["Main_Menu_VerTareas"];
            }

            if (cerrarSesiónToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_CerrarSesion"))
            {
                cerrarSesiónToolStripMenuItem.Text = traducciones["Main_Menu_CerrarSesion"];
            }

            if (cambiarIdiomaToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_CambiarIdioma"))
            {
                cambiarIdiomaToolStripMenuItem.Text = traducciones["Main_Menu_CambiarIdioma"];
            }

            if (configuracionToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_Configuracion"))
            {
                configuracionToolStripMenuItem.Text = traducciones["Main_Menu_Configuracion"];
            }

            if (generarPlanificacionToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_GenerarPlanificacion"))
            {
                generarPlanificacionToolStripMenuItem.Text = traducciones["Main_Menu_GenerarPlanificacion"];
            }

            if (gestionarPlanificacionesToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_GestionarPlanificaciones"))
            {
                gestionarPlanificacionesToolStripMenuItem.Text = traducciones["Main_Menu_GestionarPlanificaciones"];
            }

            if (top10ToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_TOP10"))
            {
                top10ToolStripMenuItem.Text = traducciones["Main_Menu_TOP10"];
            }

            if (tiempoDisponibleToolStripMenuItem != null && traducciones.ContainsKey("Main_Menu_TiempoDisponible"))
            {
                tiempoDisponibleToolStripMenuItem.Text = traducciones["Main_Menu_TiempoDisponible"];
            }
        }

        private void configuracionToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
        }

        private void tareasToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
        }

        private void cargarIdiomaToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso("IDIOMA_CARGAR_EDITAR"))
            {
                return;
            }

            CargarIdioma cargarIdiomaAbierto = MdiChildren.OfType<CargarIdioma>().FirstOrDefault();
            if (cargarIdiomaAbierto != null)
            {
                cargarIdiomaAbierto.BringToFront();
                cargarIdiomaAbierto.Activate();
                return;
            }

            CargarIdioma cargarIdiomaFormulario = new CargarIdioma
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            cargarIdiomaFormulario.Show();
        }

        private void administrarBuckupToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoAccederBackup))
            {
                return;
            }

            AdministrarBuckup administrarBuckupAbierto = MdiChildren.OfType<AdministrarBuckup>().FirstOrDefault();
            if (administrarBuckupAbierto != null)
            {
                administrarBuckupAbierto.BringToFront();
                administrarBuckupAbierto.Activate();
                return;
            }

            AdministrarBuckup administrarBuckupFormulario = new AdministrarBuckup(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            administrarBuckupFormulario.Show();
        }

        private void generarPlanificacionToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoGenerarPlanificacion))
            {
                return;
            }

            GenerarPlanificacion generarPlanificacionAbierto = MdiChildren.OfType<GenerarPlanificacion>().FirstOrDefault();

            if (generarPlanificacionAbierto != null)
            {
                generarPlanificacionAbierto.BringToFront();
                generarPlanificacionAbierto.Activate();
                return;
            }

            GenerarPlanificacion generarPlanificacionFormulario = new GenerarPlanificacion(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };

            generarPlanificacionFormulario.Show();
        }

        private void gestionarPlanificacionesToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoGestionarPlanificaciones))
            {
                return;
            }

            GestionarPlanificaciones gestionarPlanificacionesAbierto = MdiChildren.OfType<GestionarPlanificaciones>().FirstOrDefault();

            if (gestionarPlanificacionesAbierto != null)
            {
                gestionarPlanificacionesAbierto.BringToFront();
                gestionarPlanificacionesAbierto.Activate();
                return;
            }

            GestionarPlanificaciones gestionarPlanificacionesFormulario = new GestionarPlanificaciones(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };

            gestionarPlanificacionesFormulario.Show();
        }

        private void top10ToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoVerTop10))
            {
                return;
            }

            TOP10 top10Abierto = MdiChildren.OfType<TOP10>().FirstOrDefault();

            if (top10Abierto != null)
            {
                top10Abierto.BringToFront();
                top10Abierto.Activate();
                return;
            }

            TOP10 top10Formulario = new TOP10
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };

            top10Formulario.Show();
        }

        private void tiempoDisponibleToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoGestionarTiempoDisponible))
            {
                return;
            }

            GestionarTiempoDisponible tiempoDisponibleAbierto = MdiChildren.OfType<GestionarTiempoDisponible>().FirstOrDefault();

            if (tiempoDisponibleAbierto != null)
            {
                tiempoDisponibleAbierto.BringToFront();
                tiempoDisponibleAbierto.Activate();
                return;
            }

            GestionarTiempoDisponible tiempoDisponibleFormulario = new GestionarTiempoDisponible(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };

            tiempoDisponibleFormulario.Show();
        }

        private void gestionarUsuarioToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoAccederUsuarios))
            {
                return;
            }

            GestionarUsuario gestionarUsuarioAbierto = MdiChildren.OfType<GestionarUsuario>().FirstOrDefault();
            if (gestionarUsuarioAbierto != null)
            {
                gestionarUsuarioAbierto.BringToFront();
                gestionarUsuarioAbierto.Activate();
                return;
            }

            GestionarUsuario gestionarUsuarioFormulario = new GestionarUsuario(_authManager)
            {
                MdiParent = this,
                StartPosition = FormStartPosition.CenterParent
            };
            gestionarUsuarioFormulario.Show();
        }
    }
}