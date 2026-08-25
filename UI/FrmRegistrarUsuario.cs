using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BL;
using Servicioss;

namespace UI
{
    public partial class FrmRegistrarUsuario : Form, IIdiomaObserver
    {
        private readonly AuthManager _authManager;
        public string UsuarioCreado { get; private set; }

        private string _lblMensajeClaveActual = null;
        private object[] _lblMensajeArgsActual = null;

        public FrmRegistrarUsuario()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            btnRegistrar.Enabled = false;
            Text += " (sin contexto)";
            InitPasswordChars();
        }

        public FrmRegistrarUsuario(AuthManager authManager)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            btnRegistrar.Enabled = true;
            InitPasswordChars();
        }

        protected override void OnLoad(EventArgs eventArgs)
        {
            base.OnLoad(eventArgs);
            IdiomaService.Instancia.Suscribir(this);

            if (string.IsNullOrWhiteSpace(lblMensaje.Text))
            {
                SetMensaje("Reg_Mensaje_Inicial", "Complete todos los campos.");
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void InitPasswordChars()
        {
            txtPassword.PasswordChar = '•';
            txtConfirm.PasswordChar = '•';
            chkVer.Checked = false;
            chkVer.CheckedChanged += chkVer_CheckedChanged;
        }

        private void btnRegistrar_Click(object sender, EventArgs eventArgs)
        {
            Registrar();
        }

        private void btnCancelar_Click(object sender, EventArgs eventArgs)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void chkVer_CheckedChanged(object sender, EventArgs eventArgs)
        {
            char caracter;

            if (chkVer.Checked)
            {
                caracter = '\0';
            }
            else
            {
                caracter = '•';
            }

            txtPassword.PasswordChar = caracter;
            txtConfirm.PasswordChar = caracter;
        }

        private void Registrar()
        {
            if (_authManager == null)
            {
                MostrarErrorClave("Reg_Error_SinContexto", "Sin contexto de autenticación.");
                return;
            }

            lblMensaje.Text = string.Empty;

            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text;
            string confirmacion = txtConfirm.Text;

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmacion))
            {
                MostrarErrorClave("Reg_Error_Campos", "Complete todos los campos.");
                return;
            }

            if (!password.Equals(confirmacion))
            {
                MostrarErrorClave("Reg_Error_NoCoinciden", "Las contraseñas no coinciden.");
                txtConfirm.Focus();
                return;
            }

            if (!_authManager.ValidarPoliticasPassword(password))
            {
                MostrarErrorClave("Reg_Error_Politicas", "La contraseña no cumple políticas (8, mayús, minús, número y especial).");
                return;
            }

            if (!_authManager.RegistrarUsuario(usuario, password))
            {
                MostrarErrorClave("Reg_Error_NoSePudo", "No se pudo registrar. Quizás ya exista.");
                return;
            }

            UsuarioCreado = usuario;
            lblMensaje.ForeColor = Color.DarkGreen;
            SetMensaje("Reg_Ok_Creado", "Usuario creado correctamente.");
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MostrarErrorClave(string clave, string fallback)
        {
            lblMensaje.ForeColor = Color.Maroon;
            SetMensaje(clave, fallback);
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

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            if (traducciones.ContainsKey("Reg_Titulo"))
            {
                Text = traducciones["Reg_Titulo"];
            }

            if (traducciones.ContainsKey("Reg_Boton_Registrar"))
            {
                btnRegistrar.Text = traducciones["Reg_Boton_Registrar"];
            }

            if (traducciones.ContainsKey("Reg_Boton_Cancelar"))
            {
                btnCancelar.Text = traducciones["Reg_Boton_Cancelar"];
            }

            if (traducciones.ContainsKey("Reg_Check_Ver"))
            {
                chkVer.Text = traducciones["Reg_Check_Ver"];
            }

            try
            {
                Label lblUsuarioCtrl = Controls["lblUsuario"] as Label ?? Controls["label1"] as Label;
                Label lblPasswordCtrl = Controls["lblPassword"] as Label ?? Controls["label2"] as Label;
                Label lblConfirmCtrl = Controls["lblConfirm"] as Label ?? Controls["label3"] as Label;

                if (lblUsuarioCtrl != null && traducciones.ContainsKey("Reg_Label_Usuario"))
                {
                    lblUsuarioCtrl.Text = traducciones["Reg_Label_Usuario"];
                }

                if (lblPasswordCtrl != null && traducciones.ContainsKey("Reg_Label_Password"))
                {
                    lblPasswordCtrl.Text = traducciones["Reg_Label_Password"];
                }

                if (lblConfirmCtrl != null && traducciones.ContainsKey("Reg_Label_Confirmar"))
                {
                    lblConfirmCtrl.Text = traducciones["Reg_Label_Confirmar"];
                }
            }
            catch
            {
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
            else if (string.IsNullOrWhiteSpace(lblMensaje.Text) && traducciones.ContainsKey("Reg_Mensaje_Inicial"))
            {
                lblMensaje.Text = traducciones["Reg_Mensaje_Inicial"];
            }
        }
    }
}
