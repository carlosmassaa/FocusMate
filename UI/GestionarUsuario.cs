using System;
using System.Linq;
using System.Windows.Forms;
using BL;
using System.Collections.Generic;
using Servicioss;
using System.Drawing;

namespace UI
{
    public partial class GestionarUsuario : Form, IIdiomaObserver
    {
        private readonly AuthManager _authManager;
        private readonly UsuarioBL _usuarioBL = new UsuarioBL();

        private const string PermisoAcceder = "ACCEDER_GESTIONAR_USUARIOS";
        private const string PermisoRegistrar = "USUARIOS_REGISTRAR";
        private const string PermisoRefrescar = "USUARIOS_REFRESCAR";
        private const string PermisoEliminar = "USUARIOS_ELIMINAR";
        private const string PermisoBloquear = "USUARIOS_BLOQUEAR";
        private const string PermisoDesbloquear = "USUARIOS_DESBLOQUEAR";
        private const string PermisoActivar = "USUARIOS_ACTIVAR";

        public GestionarUsuario(AuthManager authManager)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
        }

        private void GestionarUsuario_Load(object sender, EventArgs eventArgs)
        {
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

            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoAcceder))
            {
                MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_SinPermisoAcceso", "No tiene permiso para acceder a la gestión de usuarios ({0})."), PermisoAcceder), Trad("GestUsr_Msg_TituloPermisos", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            dateTimePicker1.Value = DateTime.Now.AddHours(1);
            dateTimePicker1.MinDate = DateTime.Now.AddMinutes(1);

            AplicarPermisosAControles();
            CargarUsuarios();
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void AplicarPermisosAControles()
        {
            btnRegistrarUsuario.Enabled = _authManager.ValidarPermiso(PermisoRegistrar);
            btnRefrescar.Enabled = _authManager.ValidarPermiso(PermisoRefrescar);
            btnEliminar.Enabled = _authManager.ValidarPermiso(PermisoEliminar);
            btnBloquear.Enabled = _authManager.ValidarPermiso(PermisoBloquear);
            btnDesbloquear.Enabled = _authManager.ValidarPermiso(PermisoDesbloquear);
            btnActivar.Enabled = _authManager.ValidarPermiso(PermisoActivar);

            bool puedeBloquear = btnBloquear.Enabled;
            lblBloqueoHasta.Enabled = puedeBloquear;
            dateTimePicker1.Enabled = puedeBloquear && !chkIndefinido.Checked;
            chkIndefinido.Enabled = puedeBloquear;
        }

        private bool VerificarPermisoOAdvertir(string permiso)
        {
            if (_authManager.ValidarPermiso(permiso))
            {
                return true;
            }

            MessageBox.Show(this, string.Format(Trad("GestUsr_Permiso_AccionRequerida", "No tiene permiso para realizar esta acción ({0})."), permiso), Trad("GestUsr_Msg_TituloPermisos", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AplicarPermisosAControles();
            return false;
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuariosParaGrid = _usuarioBL.Listar()
                    .Select(usuario =>
                    {
                        string bloqueadoHastaLocal = usuario.BloqueadoHastaUtc == DateTime.MinValue
                            ? string.Empty
                            : usuario.BloqueadoHastaUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                        return new
                        {
                            usuario.Id,
                            Usuario = usuario.NombreUsuario,
                            Activo = usuario.EstaActivo,
                            IntentosFallidos = usuario.FailedAttempts,
                            BloqueadoHastaLocal = bloqueadoHastaLocal
                        };
                    })
                    .OrderBy(usuarioItem => usuarioItem.Usuario)
                    .ToList();

                dgvUsuarios.AutoGenerateColumns = true;
                dgvUsuarios.DataSource = usuariosParaGrid;

                if (dgvUsuarios.Columns["Id"] != null)
                {
                    dgvUsuarios.Columns["Id"].Visible = false;
                }

                if (dgvUsuarios.Columns["Usuario"] != null)
                {
                    dgvUsuarios.Columns["Usuario"].HeaderText = Trad("GestUsr_Col_Usuario", "Usuario");
                }

                if (dgvUsuarios.Columns["Activo"] != null)
                {
                    dgvUsuarios.Columns["Activo"].HeaderText = Trad("GestUsr_Col_Activo", "Activo");
                }

                if (dgvUsuarios.Columns["IntentosFallidos"] != null)
                {
                    dgvUsuarios.Columns["IntentosFallidos"].HeaderText = Trad("GestUsr_Col_IntentosFallidos", "Intentos Fallidos");
                }

                if (dgvUsuarios.Columns["BloqueadoHastaLocal"] != null)
                {
                    dgvUsuarios.Columns["BloqueadoHastaLocal"].HeaderText = Trad("GestUsr_Col_BloqueadoHasta", "Bloqueado Hasta");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ErrorCargarUsuarios", "Error al cargar usuarios: {0}"), exception.Message), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetUsuarioSeleccionadoId(out string nombreUsuario)
        {
            nombreUsuario = null;

            if (dgvUsuarios.CurrentRow == null)
            {
                return null;
            }

            DataGridViewRow filaSeleccionada = dgvUsuarios.CurrentRow;
            object idCelda = filaSeleccionada.Cells["Id"]?.Value;
            object nombreCelda = filaSeleccionada.Cells["Usuario"]?.Value;

            int usuarioId;
            if (idCelda == null || !int.TryParse(idCelda.ToString(), out usuarioId))
            {
                return null;
            }

            nombreUsuario = nombreCelda?.ToString();
            return usuarioId;
        }

        private bool EsUsuarioActual(string nombreUsuario)
        {
            return !string.IsNullOrWhiteSpace(_authManager?.UsuarioActual?.NombreUsuario)
                && string.Equals(nombreUsuario, _authManager.UsuarioActual.NombreUsuario, StringComparison.OrdinalIgnoreCase);
        }

        private void btnRegistrarUsuario_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoRegistrar))
            {
                return;
            }

            using (FrmRegistrarUsuario registrarUsuarioFormulario = new FrmRegistrarUsuario(_authManager))
            {
                registrarUsuarioFormulario.StartPosition = FormStartPosition.CenterParent;

                if (registrarUsuarioFormulario.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(registrarUsuarioFormulario.UsuarioCreado))
                {
                    MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_UsuarioCreado", "Usuario '{0}' creado."), registrarUsuarioFormulario.UsuarioCreado), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoRefrescar))
            {
                return;
            }

            CargarUsuarios();
        }

        private void btnEliminar_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoEliminar))
            {
                return;
            }

            string nombreUsuarioSeleccionado;
            int? usuarioId = GetUsuarioSeleccionadoId(out nombreUsuarioSeleccionado);

            if (!usuarioId.HasValue)
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_SeleccioneUsuario", "Seleccione un usuario."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (EsUsuarioActual(nombreUsuarioSeleccionado))
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_NoEliminarPropio", "No puede eliminar su propio usuario."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ConfirmarEliminar", "¿Eliminar (desactivar) '{0}'?"), nombreUsuarioSeleccionado), Trad("GestUsr_Msg_Confirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (_usuarioBL.EliminarUsuario(usuarioId.Value))
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_UsuarioDesactivado", "Usuario desactivado."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_NoSePudoDesactivar", "No se pudo desactivar."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ErrorGenerico", "Error: {0}"), exception.Message), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBloquear_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoBloquear))
            {
                return;
            }

            string nombreUsuarioSeleccionado;
            int? usuarioId = GetUsuarioSeleccionadoId(out nombreUsuarioSeleccionado);

            if (!usuarioId.HasValue)
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_SeleccioneUsuario", "Seleccione un usuario."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (EsUsuarioActual(nombreUsuarioSeleccionado))
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_NoBloquearPropio", "No puede bloquear su propio usuario."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime? bloqueadoHastaUtc;

            if (chkIndefinido.Checked)
            {
                bloqueadoHastaUtc = null;
            }
            else
            {
                DateTime fechaHoraLocalSeleccionada = dateTimePicker1.Value;

                if (fechaHoraLocalSeleccionada <= DateTime.Now.AddMinutes(1))
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_FechaHoraInvalida", "La fecha/hora debe ser futura (? 1 minuto)."), Trad("GestUsr_Msg_TituloValidacion", "Validación"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bloqueadoHastaUtc = fechaHoraLocalSeleccionada.ToUniversalTime();
            }

            string mensajeConfirmacion;
            if (chkIndefinido.Checked)
            {
                mensajeConfirmacion = string.Format(Trad("GestUsr_Msg_ConfirmarBloqueoIndef", "¿Bloquear indefinidamente a '{0}'?"), nombreUsuarioSeleccionado);
            }
            else
            {
                mensajeConfirmacion = string.Format(Trad("GestUsr_Msg_ConfirmarBloqueoHasta", "¿Bloquear a '{0}' hasta {1}?"), nombreUsuarioSeleccionado, dateTimePicker1.Value.ToString("dd/MM/yyyy HH:mm"));
            }

            if (MessageBox.Show(this, mensajeConfirmacion, Trad("GestUsr_Msg_Confirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (_usuarioBL.BloquearUsuario(usuarioId.Value, bloqueadoHastaUtc))
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_UsuarioBloqueado", "Usuario bloqueado."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_NoSePudoBloquear", "No se pudo bloquear."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ErrorGenerico", "Error: {0}"), exception.Message), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDesbloquear_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoDesbloquear))
            {
                return;
            }

            string nombreUsuarioSeleccionado;
            int? usuarioId = GetUsuarioSeleccionadoId(out nombreUsuarioSeleccionado);

            if (!usuarioId.HasValue)
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_SeleccioneUsuario", "Seleccione un usuario."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ConfirmarDesbloquear", "¿Desbloquear '{0}'?"), nombreUsuarioSeleccionado), Trad("GestUsr_Msg_Confirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (_usuarioBL.DesbloquearUsuario(usuarioId.Value))
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_UsuarioDesbloqueado", "Usuario desbloqueado."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_NoSePudoDesbloquear", "No se pudo desbloquear."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ErrorGenerico", "Error: {0}"), exception.Message), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkIndefinido_CheckedChanged(object sender, EventArgs eventArgs)
        {
            if (!_authManager.ValidarPermiso(PermisoBloquear))
            {
                chkIndefinido.Checked = false;
                AplicarPermisosAControles();
                return;
            }

            dateTimePicker1.Enabled = !chkIndefinido.Checked && btnBloquear.Enabled;
            lblBloqueoHasta.Enabled = !chkIndefinido.Checked && btnBloquear.Enabled;
        }

        private void btnActivar_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoActivar))
            {
                return;
            }

            string nombreUsuarioSeleccionado;
            int? usuarioId = GetUsuarioSeleccionadoId(out nombreUsuarioSeleccionado);

            if (!usuarioId.HasValue)
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_SeleccioneUsuario", "Seleccione un usuario."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool yaActivo = false;
            try
            {
                object valorActivoCelda = dgvUsuarios.CurrentRow?.Cells["Activo"]?.Value;
                if (valorActivoCelda != null)
                {
                    yaActivo = Convert.ToBoolean(valorActivoCelda);
                }
            }
            catch
            {
            }

            if (yaActivo)
            {
                MessageBox.Show(this, Trad("GestUsr_Msg_YaActivo", "El usuario ya está activo."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ConfirmarActivar", "¿Activar '{0}'?"), nombreUsuarioSeleccionado), Trad("GestUsr_Msg_Confirmar", "Confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (_usuarioBL.ActivarUsuario(usuarioId.Value))
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_UsuarioActivado", "Usuario activado."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarUsuarios();
                }
                else
                {
                    MessageBox.Show(this, Trad("GestUsr_Msg_NoSePudoActivar", "No se pudo activar."), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("GestUsr_Msg_ErrorGenerico", "Error: {0}"), exception.Message), Trad("GestUsr_Msg_TituloUsuarios", "Usuarios"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            AplicarTraduccionesEstaticas();

            if (dgvUsuarios != null && dgvUsuarios.Columns.Count > 0)
            {
                if (dgvUsuarios.Columns["Usuario"] != null)
                {
                    dgvUsuarios.Columns["Usuario"].HeaderText = Trad("GestUsr_Col_Usuario", "Usuario");
                }
                if (dgvUsuarios.Columns["Activo"] != null)
                {
                    dgvUsuarios.Columns["Activo"].HeaderText = Trad("GestUsr_Col_Activo", "Activo");
                }
                if (dgvUsuarios.Columns["IntentosFallidos"] != null)
                {
                    dgvUsuarios.Columns["IntentosFallidos"].HeaderText = Trad("GestUsr_Col_IntentosFallidos", "Intentos Fallidos");
                }
                if (dgvUsuarios.Columns["BloqueadoHastaLocal"] != null)
                {
                    dgvUsuarios.Columns["BloqueadoHastaLocal"].HeaderText = Trad("GestUsr_Col_BloqueadoHasta", "Bloqueado Hasta");
                }
            }
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("GestUsr_Titulo", "Gestionar Usuario");

            if (btnRegistrarUsuario != null)
            {
                btnRegistrarUsuario.Text = Trad("GestUsr_Boton_Registrar", "Registrar nuevo usuario");
            }

            if (btnRefrescar != null)
            {
                btnRefrescar.Text = Trad("GestUsr_Boton_Refrescar", "Refrescar");
            }

            if (btnEliminar != null)
            {
                btnEliminar.Text = Trad("GestUsr_Boton_Eliminar", "Eliminar usuario");
            }

            if (lblBloqueoHasta != null)
            {
                lblBloqueoHasta.Text = Trad("GestUsr_Label_BloqueoHasta", "Bloquear hasta (F/H)");
            }

            if (chkIndefinido != null)
            {
                chkIndefinido.Text = Trad("GestUsr_Label_Indefinido", "?");
            }

            if (btnBloquear != null)
            {
                btnBloquear.Text = Trad("GestUsr_Boton_Bloquear", "Bloquear usuario");
            }

            if (btnDesbloquear != null)
            {
                btnDesbloquear.Text = Trad("GestUsr_Boton_Desbloquear", "Desbloquear usuario");
            }

            if (btnActivar != null)
            {
                btnActivar.Text = Trad("GestUsr_Boton_Activar", "Activar usuario");
            }
        }

        private string Trad(string key, string fallback)
        {
            string texto = IdiomaService.Instancia.Traducir(key);
            if (string.IsNullOrWhiteSpace(texto) || texto == key)
            {
                return fallback;
            }
            return texto;
        }
     
    }
}
