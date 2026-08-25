using System;
using System.Drawing;
using System.Windows.Forms;
using BL;
using Servicioss;
using System.Collections.Generic;
using BE;

namespace UI
{
    public partial class FrmLogin : Form, IIdiomaObserver
    {
        private readonly AuthManager _authManager;
        private readonly BitacoraBL _servicioBitacora;
        private readonly IdiomaBL _idiomaBL = new IdiomaBL();

        private string _lblMensajeClaveActual = null;
        private object[] _lblMensajeArgsActual = null;

        public FrmLogin()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            _servicioBitacora = BitacoraBL.CrearBasico();
            CryptoService cryptoService = new CryptoService();
            AutorizacionService autorizacionService = _servicioBitacora.Autorizacion;
            _authManager = new AuthManager(cryptoService, _servicioBitacora, autorizacionService);

            txtContraseña.PasswordChar = '•';
            checkBoxVer.CheckedChanged += checkBoxVer_CheckedChanged;

            if (lblMensaje != null)
            {
                lblMensaje.Text = string.Empty;
            }

            this.AcceptButton = btnIngresar;
            this.CancelButton = btnCancelar;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            VerificarIntegridadAlIniciar();

            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Focus();
            }
            else
            {
                txtContraseña.Focus();
            }

            IdiomaService.Instancia.Suscribir(this);
            CargarIdiomas();

            if (string.IsNullOrWhiteSpace(lblMensaje.Text))
            {
                SetMensaje("Login_Mensaje_Inicial", "Ingrese sus credenciales");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void CargarIdiomas()
        {
            try
            {
                List<BE.Idioma> idiomas = _idiomaBL.ListarIdiomas();
                comboIdioma.DataSource = idiomas;
                comboIdioma.DisplayMember = "Nombre";
                comboIdioma.ValueMember = "Id";

                comboIdioma.SelectedIndexChanged -= comboIdioma_SelectedIndexChanged;
                comboIdioma.SelectedIndexChanged += comboIdioma_SelectedIndexChanged;

                int indiceEspanolArgentina = idiomas.FindIndex(idiomaItem => idiomaItem.CodigoISO == "es-AR" || idiomaItem.Nombre.Contains("Español (Argentina)"));
                if (indiceEspanolArgentina < 0)
                {
                    indiceEspanolArgentina = 0;
                }

                comboIdioma.SelectedIndex = indiceEspanolArgentina;

                BE.Idioma idiomaSeleccionado = comboIdioma.SelectedItem as BE.Idioma;
                if (idiomaSeleccionado != null)
                {
                    Dictionary<string, string> diccionarioTraducciones = _idiomaBL.ObtenerTraducciones(idiomaSeleccionado.Id);
                    IdiomaService.Instancia.CambiarIdioma(diccionarioTraducciones);
                }
            }
            catch
            {
            }
        }

        private void comboIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboIdioma.SelectedItem is BE.Idioma idioma)
            {
                Dictionary<string, string> diccionarioTraducciones = _idiomaBL.ObtenerTraducciones(idioma.Id);
                IdiomaService.Instancia.CambiarIdioma(diccionarioTraducciones);
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string username = txtUsuario.Text.Trim();
            string password = txtContraseña.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblMensaje.ForeColor = Color.Maroon;
                SetMensaje("Login_Validacion_CamposObligatorios", "Ingrese usuario y contraseña.");
                return;
            }

            LoginResultado resultado = _authManager.IntentarLogin(username, password);

            if (resultado.RequiereAprobacionDv)
            {
                FrmIntegridadDv dlg = new FrmIntegridadDv(resultado.DetalleIntegridad);
                DialogResult dr = dlg.ShowDialog(this);

                if (dr != DialogResult.OK)
                {
                    lblMensaje.ForeColor = Color.Maroon;
                    string debeAceptar = IdiomaService.Instancia.Traducir("Login_Integridad_DebeAceptar");
                    if (string.IsNullOrEmpty(debeAceptar) || debeAceptar == "Login_Integridad_DebeAceptar")
                    {
                        debeAceptar = "Debe aceptar una acción (recalcular o restaurar) para continuar.";
                    }

                    _lblMensajeClaveActual = "Login_Integridad_DebeAceptar";
                    _lblMensajeArgsActual = null;
                    lblMensaje.Text = debeAceptar;
                    return;
                }

                switch (dlg.Decision)
                {
                    case IntegridadDecision.AceptarCambios:
                        {
                            bool reparacionOk = _authManager.RepararIntegridad(username);
                            if (reparacionOk)
                            {
                                lblMensaje.ForeColor = Color.DarkGreen;
                                string msgOk = IdiomaService.Instancia.Traducir("Login_Integridad_Reparada");
                                if (string.IsNullOrEmpty(msgOk) || msgOk == "Login_Integridad_Reparada")
                                {
                                    msgOk = "Integridad recalculada. Inicie sesión nuevamente.";
                                }

                                _lblMensajeClaveActual = "Login_Integridad_Reparada";
                                _lblMensajeArgsActual = null;
                                lblMensaje.Text = msgOk;
                                txtContraseña.Clear();
                                txtContraseña.Focus();
                            }
                            else
                            {
                                lblMensaje.ForeColor = Color.Maroon;
                                string msgFail = IdiomaService.Instancia.Traducir("Login_Integridad_NoSePudoReparar");
                                if (string.IsNullOrEmpty(msgFail) || msgFail == "Login_Integridad_NoSePudoReparar")
                                {
                                    msgFail = "No fue posible recalcular la integridad.";
                                }

                                _lblMensajeClaveActual = "Login_Integridad_NoSePudoReparar";
                                _lblMensajeArgsActual = null;
                                lblMensaje.Text = msgFail;
                            }

                            return;
                        }

                    case IntegridadDecision.RestaurarBackup:
                        {
                            lblMensaje.ForeColor = Color.DarkGreen;
                            string msgRest = "Base restaurada desde backup.\nInicie sesión nuevamente.";
                            _lblMensajeClaveActual = null;
                            _lblMensajeArgsActual = null;
                            lblMensaje.Text = msgRest;
                            txtContraseña.Clear();
                            txtUsuario.Focus();
                            return;
                        }

                    case IntegridadDecision.Cancelar:
                    default:
                        {
                            lblMensaje.ForeColor = Color.Maroon;
                            string debeAceptar = IdiomaService.Instancia.Traducir("Login_Integridad_DebeAceptar");
                            if (string.IsNullOrEmpty(debeAceptar) || debeAceptar == "Login_Integridad_DebeAceptar")
                            {
                                debeAceptar = "Debe elegir Aceptar cambios o Restaurar para continuar.";
                            }

                            _lblMensajeClaveActual = "Login_Integridad_DebeAceptar";
                            _lblMensajeArgsActual = null;
                            lblMensaje.Text = debeAceptar;
                            return;
                        }
                }
            }

            if (!resultado.Exito)
            {
                lblMensaje.ForeColor = Color.Maroon;

                switch (resultado.Status)
                {
                    case LoginStatus.ParametrosInvalidos:
                        SetMensaje("Login_Validacion_CamposObligatorios", "Ingrese usuario y contraseña.");
                        break;

                    case LoginStatus.UsuarioInexistente:
                        SetMensaje("Login_Validacion_UsuarioNoExiste", "El usuario no existe.");
                        break;

                    case LoginStatus.UsuarioBloqueado:
                        SetMensaje("Login_Validacion_UsuarioBloqueado", "El usuario está bloqueado. Contacte a un administrador.");
                        break;

                    case LoginStatus.CredencialesInvalidas:
                    default:
                        {
                            if (resultado.IntentosFallidos == 0 && !string.IsNullOrWhiteSpace(resultado.Mensaje))
                            {
                                _lblMensajeClaveActual = null;
                                _lblMensajeArgsActual = null;
                                lblMensaje.Text = resultado.Mensaje;
                            }
                            else if (resultado.IntentosFallidos > 0 && resultado.FaltanParaBloqueo > 0 && resultado.UmbralBloqueoActual > 0)
                            {
                                string plantilla = IdiomaService.Instancia.Traducir("Login_Validacion_CredencialesInvalidas_Detalle");
                                if (string.IsNullOrEmpty(plantilla) || plantilla == "Login_Validacion_CredencialesInvalidas_Detalle")
                                {
                                    plantilla = "Credenciales inválidas. Intentos: {0}. Bloqueo al llegar a {1} (faltan {2}).";
                                }

                                _lblMensajeClaveActual = "Login_Validacion_CredencialesInvalidas_Detalle";
                                _lblMensajeArgsActual = new object[] { resultado.IntentosFallidos, resultado.UmbralBloqueoActual, resultado.FaltanParaBloqueo };
                                lblMensaje.Text = string.Format(plantilla, _lblMensajeArgsActual);
                            }
                            else
                            {
                                SetMensaje("Login_Validacion_ContraseñaIncorrecta", "Contraseña incorrecta.");
                            }

                            break;
                        }
                }

                txtContraseña.Clear();
                txtContraseña.Focus();
                return;
            }

            lblMensaje.ForeColor = Color.DarkGreen;
            string usuarioActual = SesionActual.Instance?.NombreUsuario ?? username;
            _lblMensajeClaveActual = "Login_Validacion_Bienvenido";
            _lblMensajeArgsActual = new object[] { usuarioActual };
            string bienvenido = IdiomaService.Instancia.Traducir("Login_Validacion_Bienvenido");
            if (string.IsNullOrEmpty(bienvenido) || bienvenido == "Login_Validacion_Bienvenido")
            {
                bienvenido = "Bienvenido {0}!";
            }

            lblMensaje.Text = string.Format(bienvenido, _lblMensajeArgsActual);

            this.Hide();
            MainMidForm formularioPrincipal = new MainMidForm(_authManager, _servicioBitacora);
            formularioPrincipal.FormClosed += delegate (object sender2, FormClosedEventArgs args)
            {
                txtContraseña.Clear();
                txtUsuario.Focus();
                lblMensaje.Text = string.Empty;
                lblMensaje.ForeColor = Color.Black;

                this.Show();
                this.Activate();
                this.BringToFront();

                SetMensaje("Login_Mensaje_Inicial", "Ingrese sus credenciales");
                lblMensaje.ForeColor = Color.Black;
            };
            formularioPrincipal.Show();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void checkBoxVer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxVer.Checked)
            {
                txtContraseña.PasswordChar = '\0';
            }
            else
            {
                txtContraseña.PasswordChar = '•';
            }
        }

        private void VerificarIntegridadAlIniciar()
        {
            TareaBL tareaBL = new TareaBL();
            string detalleDvh;
            bool okDvh = tareaBL.VerificarDVH_Tarea(out detalleDvh);

            IntegridadBL integridadService = new IntegridadBL();
            string detalleDvv;
            bool okDvv = integridadService.VerificarTarea(out detalleDvv);

            if (!okDvh || !okDvv)
            {
                string advertencia = IdiomaService.Instancia.Traducir("Login_Integridad_AdvertenciaInicio");
                if (string.IsNullOrEmpty(advertencia) || advertencia == "Login_Integridad_AdvertenciaInicio")
                {
                    advertencia = "Se detectaron errores de integridad. Debe iniciar sesión un administrador autorizado.";
                }

                string titulo = IdiomaService.Instancia.Traducir("Login_Integridad_Titulo");
                if (string.IsNullOrEmpty(titulo) || titulo == "Login_Integridad_Titulo")
                {
                    titulo = "Integridad de datos";
                }

                MessageBox.Show(this, advertencia, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnIngresar_Click_1(object sender, EventArgs e)
        {
            btnIngresar_Click(sender, e);
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            FrmRegistrarUsuario formularioRegistro = new FrmRegistrarUsuario(_authManager);
            DialogResult resultado = formularioRegistro.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                txtUsuario.Text = formularioRegistro.UsuarioCreado;
                txtContraseña.Clear();
                txtContraseña.Focus();

                lblMensaje.ForeColor = Color.DarkGreen;
                lblMensaje.Text = "Usuario registrado. Ingrese la contraseña.";
            }

            formularioRegistro.Dispose();
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            if (traducciones.ContainsKey("Login_Titulo"))
            {
                this.Text = traducciones["Login_Titulo"];
            }

            if (traducciones.ContainsKey("Login_Boton_Ingresar"))
            {
                btnIngresar.Text = traducciones["Login_Boton_Ingresar"];
            }

            if (traducciones.ContainsKey("Login_Boton_Cancelar"))
            {
                btnCancelar.Text = traducciones["Login_Boton_Cancelar"];
            }

            if (traducciones.ContainsKey("Login_Boton_Registrar"))
            {
                btnRegistrar.Text = traducciones["Login_Boton_Registrar"];
            }

            if (traducciones.ContainsKey("Login_Check_Ver"))
            {
                checkBoxVer.Text = traducciones["Login_Check_Ver"];
            }

            if (traducciones.ContainsKey("Login_Label_Idioma"))
            {
                lblIdioma.Text = traducciones["Login_Label_Idioma"];
            }

            if (traducciones.ContainsKey("Login_Label_Usuario"))
            {
                label1.Text = traducciones["Login_Label_Usuario"];
            }

            if (traducciones.ContainsKey("Login_Label_Contrasena"))
            {
                label2.Text = traducciones["Login_Label_Contrasena"];
            }

            if (_lblMensajeClaveActual != null)
            {
                string textoTraducido = IdiomaService.Instancia.Traducir(_lblMensajeClaveActual);
                if (string.IsNullOrEmpty(textoTraducido) || textoTraducido == _lblMensajeClaveActual)
                {
                    textoTraducido = lblMensaje.Text;
                }

                lblMensaje.Text = string.Format(textoTraducido, _lblMensajeArgsActual ?? Array.Empty<object>());
            }
            else if (string.IsNullOrWhiteSpace(lblMensaje.Text) && traducciones.ContainsKey("Login_Mensaje_Inicial"))
            {
                lblMensaje.Text = traducciones["Login_Mensaje_Inicial"];
                _lblMensajeClaveActual = "Login_Mensaje_Inicial";
                _lblMensajeArgsActual = null;
            }
        }

        private void SetMensaje(string clave, string fallback, params object[] args)
        {
            _lblMensajeClaveActual = clave;
            _lblMensajeArgsActual = args;

            string texto = IdiomaService.Instancia.Traducir(clave);
            if (string.IsNullOrEmpty(texto) || texto == clave)
            {
                texto = fallback ?? string.Empty;
            }

            lblMensaje.Text = string.Format(texto, args ?? Array.Empty<object>());
        }
    }
}
