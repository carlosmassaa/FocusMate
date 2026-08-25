using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Servicioss;

namespace UI
{
    public partial class EditarFamiliaPermiso : Form, IIdiomaObserver
    {
        public enum ModoEdicion
        {
            Agregar,
            Editar,
            Eliminar
        }

        public ModoEdicion Modo { get; }
        public string NombreIngresado { get; private set; } = string.Empty;

        public EditarFamiliaPermiso(ModoEdicion modo, string nombreActual)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            Modo = modo;

            textBox1.Text = nombreActual ?? string.Empty;
            textBox1.ReadOnly = modo == ModoEdicion.Eliminar;

            buttonGuardar.Visible = modo != ModoEdicion.Eliminar;
            buttonEliminar.Visible = modo == ModoEdicion.Eliminar;

            buttonGuardar.Click += buttonGuardar_Click;
            buttonEliminar.Click += buttonEliminar_Click;
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

        private void buttonGuardar_Click(object sender, EventArgs eventArgs)
        {
            string nombreFamilia = (textBox1.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombreFamilia))
            {
                MessageBox.Show(this, Trad("PermFam_Msg_IngreseNombre", "Ingrese un nombre."), Trad("PermFam_Msg_Titulo", "Familias"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NombreIngresado = nombreFamilia;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonEliminar_Click(object sender, EventArgs eventArgs)
        {
            DialogResult respuesta = MessageBox.Show(this, "¿Confirma eliminar esta familia?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
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
            string titulo;

            switch (Modo)
            {
                case ModoEdicion.Agregar:
                    titulo = Trad("PermFam_Titulo_Agregar", "Agregar familia de permisos");
                    break;

                case ModoEdicion.Editar:
                    titulo = Trad("PermFam_Titulo_Editar", "Editar familia de permisos");
                    break;

                case ModoEdicion.Eliminar:
                    titulo = Trad("PermFam_Titulo_Eliminar", "Eliminar familia de permisos");
                    break;

                default:
                    titulo = Trad("PermFam_Titulo_Editar", "Editar familia de permisos");
                    break;
            }

            Text = titulo;

            if (labelFamiliaPermiso != null)
            {
                labelFamiliaPermiso.Text = titulo;
            }

            if (labelNombre != null)
            {
                labelNombre.Text = Trad("PermFam_Label_Nombre", "Nombre:");
            }

            if (buttonGuardar != null)
            {
                buttonGuardar.Text = Trad("PermFam_Boton_Guardar", "Guardar");
            }

            if (buttonEliminar != null)
            {
                buttonEliminar.Text = Trad("PermFam_Boton_Eliminar", "Eliminar");
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

        private void buttonGuardar_Click_1(object sender, EventArgs eventArgs)
        {
        }
    }
}
