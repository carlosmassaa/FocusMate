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
    public partial class CambiarIdioma : Form, IIdiomaObserver
    {
        private readonly IdiomaBL _idiomaBl = new IdiomaBL();
        private readonly AuthManager _authManager;
        private readonly AutorizacionService _autorizacionService = new AutorizacionService();
        private readonly UsuarioBL _usuarioBl = new UsuarioBL();
        private bool _permisoIdiomaCrear;

        public CambiarIdioma()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            ButtonGuardar.Click += ButtonGuardar_Click;
            buttonCancelar.Click += (object origen, EventArgs eventArgs) => { DialogResult = DialogResult.Cancel; Close(); };

            if (comboBox1 != null)
            {
                comboBox1.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
                {
                    if (checkBoxDefaultIdioma != null)
                    {
                        checkBoxDefaultIdioma.Checked = false;
                    }
                };
            }
        }

        public CambiarIdioma(AuthManager authManager) : this()
        {
            _authManager = authManager;
        }

        protected override void OnLoad(EventArgs eventArgs)
        {
            base.OnLoad(eventArgs);
            IdiomaService.Instancia.Suscribir(this);
            CargarIdiomas();
            PreseleccionarIdiomaPorDefectoUsuario();
            AplicarTraduccionesEstaticas();
            AplicarPermisosUI();
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void AplicarPermisosUI()
        {
            _permisoIdiomaCrear = false;

            try
            {
                if (_authManager != null && _authManager.EstaAutenticado)
                {
                    _permisoIdiomaCrear = _authManager.ValidarPermiso("IDIOMA_CREAR");
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
                            _permisoIdiomaCrear = _autorizacionService.TienePermiso(usuario, "IDIOMA_CREAR");
                        }
                    }
                }
            }
            catch
            {
            }

            if (buttonAgregarIdioma != null)
            {
                buttonAgregarIdioma.Enabled = _permisoIdiomaCrear;
            }
        }

        private void CargarIdiomas()
        {
            List<Idioma> idiomas = _idiomaBl.ListarIdiomas() ?? new List<Idioma>();
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";
            comboBox1.DataSource = idiomas;

            if (idiomas.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void PreseleccionarIdiomaPorDefectoUsuario()
        {
            try
            {
                int usuarioId = SesionActual.Instance?.UsuarioId ?? 0;

                if (usuarioId <= 0)
                {
                    return;
                }

                Usuario usuario = _usuarioBl.Obtener(usuarioId);

                if (usuario?.IdiomaId != null && usuario.IdiomaId.Value > 0)
                {
                    int idiomaId = usuario.IdiomaId.Value;

                    for (int indice = 0; indice < comboBox1.Items.Count; indice++)
                    {
                        if (comboBox1.Items[indice] is Idioma idioma && idioma.Id == idiomaId)
                        {
                            comboBox1.SelectedIndex = indice;

                            if (checkBoxDefaultIdioma != null)
                            {
                                checkBoxDefaultIdioma.Checked = true;
                            }

                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void ButtonGuardar_Click(object origen, EventArgs eventArgs)
        {
            if (!(comboBox1.SelectedValue is int idIdiomaSeleccionado) || idIdiomaSeleccionado <= 0)
            {
                MessageBox.Show(this, Trad("CambIdioma_Msg_SeleccioneIdioma", "Seleccione un idioma."), Trad("CambIdioma_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Dictionary<string, string> traducciones = _idiomaBl.ObtenerTraducciones(idIdiomaSeleccionado);

            if (traducciones == null || traducciones.Count == 0)
            {
                MessageBox.Show(this, Trad("CambIdioma_Msg_SinTraducciones", "El idioma seleccionado no tiene traducciones configuradas."), Trad("CambIdioma_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IdiomaService.Instancia.CambiarIdioma(traducciones);

            if (checkBoxDefaultIdioma != null && checkBoxDefaultIdioma.Checked)
            {
                int usuarioId = SesionActual.Instance?.UsuarioId ?? 0;

                if (usuarioId > 0)
                {
                    try
                    {
                        _usuarioBl.EstablecerIdiomaPorDefecto(usuarioId, idIdiomaSeleccionado);
                    }
                    catch
                    {
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
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
            Text = Trad("CambIdioma_Titulo", "Cambiar idioma");

            if (labelSeleccioneIdioma != null)
            {
                labelSeleccioneIdioma.Text = Trad("CambIdioma_Label_Seleccione", "Seleccione un Idioma:");
            }

            if (ButtonGuardar != null)
            {
                ButtonGuardar.Text = Trad("CambIdioma_Boton_Guardar", "Guardar");
            }

            if (buttonCancelar != null)
            {
                buttonCancelar.Text = Trad("CambIdioma_Boton_Cancelar", "Cancelar");
            }

            if (buttonAgregarIdioma != null)
            {
                buttonAgregarIdioma.Text = Trad("CambIdioma_Boton_Agregar", "Agregar Idioma");
            }

            if (checkBoxDefaultIdioma != null)
            {
                checkBoxDefaultIdioma.Text = Trad("CambIdioma_Check_Default", "Usar como idioma por defecto");
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

        private void buttonAgregarIdioma_Click_2(object sender, EventArgs eventArgs)
        {
            if (!_permisoIdiomaCrear)
            {
                MessageBox.Show(this, Trad("CambIdioma_Permiso_Crear", "No tiene permiso para crear idiomas (IDIOMA_CREAR)."), Trad("CambIdioma_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (AgregarIdioma formularioAgregarIdioma = new AgregarIdioma())
            {
                formularioAgregarIdioma.StartPosition = FormStartPosition.CenterParent;

                if (formularioAgregarIdioma.ShowDialog(this) == DialogResult.OK)
                {
                    CargarIdiomas();
                    PreseleccionarIdiomaPorDefectoUsuario();
                }
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {

        }
    }
}
