using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BL;
using Servicioss;

namespace UI
{
    public partial class CrearPermiso : Form, IIdiomaObserver
    {
        private readonly AutorizacionService _autorizacionService = new AutorizacionService();

        public int? PermisoCreadoId { get; private set; }
        public string PermisoCreadoNombre { get; private set; }

        public CrearPermiso()
        {
            InitializeComponent();

            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            AcceptButton = buttonAgregar;
            CancelButton = buttonCancelar;

            buttonAgregar.Click += buttonAgregar_Click;
            buttonCancelar.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
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
            else
            {
                AplicarTraduccionesEstaticas();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(e);
        }

        private void buttonAgregar_Click(object sender, EventArgs e)
        {
            string nombre = textBoxAgregarPermiso.Text ?? string.Empty;
            nombre = nombre.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show(this, Trad("CrearPerm_Msg_IngreseNombre", "Ingrese un nombre de permiso."), Trad("CrearPerm_Msg_TituloPermisos", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxAgregarPermiso.Focus();
                return;
            }

            try
            {
                int id = _autorizacionService.CrearPatente(nombre);
                if (id <= 0)
                {
                    throw new InvalidOperationException(Trad("CrearPerm_Msg_NoSePudoCrear", "No se pudo crear el permiso."));
                }

                PermisoCreadoId = id;
                PermisoCreadoNombre = nombre;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Trad("CrearPerm_Msg_ErrorCrearPermiso", "No se pudo crear el permiso: {0}"), ex.Message), Trad("CrearPerm_Msg_TituloPermisos", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Text = Trad("CrearPerm_Titulo", "Crear Permiso");

            if (labelAgregarPermiso != null)
            {
                labelAgregarPermiso.Text = Trad("CrearPerm_Label_AgregarPermiso", "Agregar permiso:");
            }

            if (buttonAgregar != null)
            {
                buttonAgregar.Text = Trad("CrearPerm_Boton_Agregar", "Agregar");
            }

            if (buttonCancelar != null)
            {
                buttonCancelar.Text = Trad("CrearPerm_Boton_Cancelar", "Cancelar");
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

        private void buttonAgregar_Click_1(object sender, EventArgs e)
        {
        }
    }
}
