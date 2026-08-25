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
    public partial class AgregarIdioma : Form, IIdiomaObserver
    {
        private readonly IdiomaBL idiomaService = new IdiomaBL();

        public AgregarIdioma()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            AcceptButton = ButtonGuardar;
            CancelButton = buttonCancelar;
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

        private void ButtonCancelar_Click(object origen, EventArgs eventArgs)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonGuardar_Click(object origen, EventArgs eventArgs)
        {
            string textoIdiomaIngresado = (textBoxIdioma.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(textoIdiomaIngresado))
            {
                MessageBox.Show(this, Trad("AgregarIdioma_Msg_IngreseIdioma", "Ingrese un idioma (nombre o código ISO)."), Trad("AgregarIdioma_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxIdioma.Focus();
                return;
            }

            try
            {
                List<Idioma> idiomasExistentes = idiomaService.ListarIdiomas() ?? new List<Idioma>();

                string nombreIdioma = textoIdiomaIngresado;
                string codigoIso = NormalizarOCrearCodigo(textoIdiomaIngresado, idiomasExistentes);

                int nuevoIdiomaId = idiomaService.CrearIdiomaConPlaceholdersTextoBase(nombreIdioma, codigoIso);

                if (nuevoIdiomaId <= 0)
                {
                    throw new InvalidOperationException(Trad("AgregarIdioma_Msg_NoCrear", "No fue posible crear el idioma."));
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("AgregarIdioma_Msg_Error", "No se pudo agregar el idioma: {0}"), exception.Message), Trad("AgregarIdioma_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string NormalizarOCrearCodigo(string textoIngresado, IEnumerable<Idioma> idiomasExistentes)
        {
            string textoNormalizado = textoIngresado.Trim();
            string codigoIso;

            if (textoNormalizado.Contains("-") && textoNormalizado.Length <= 10)
            {
                string[] partes = textoNormalizado.Split('-');

                if (partes.Length >= 2 && partes[0].Length >= 2 && partes[1].Length >= 2)
                {
                    codigoIso = partes[0].Substring(0, 2).ToLowerInvariant() + "-" + partes[1].Substring(0, 2).ToUpperInvariant();
                }
                else
                {
                    codigoIso = textoNormalizado.ToLowerInvariant();
                }
            }
            else
            {
                string letrasValidas = new string(textoNormalizado.Where(char.IsLetter).ToArray()).ToLowerInvariant();
                string codigoLenguaje = letrasValidas.Length >= 2
                    ? letrasValidas.Substring(0, 2)
                    : (letrasValidas + "xx").Substring(0, 2);

                codigoIso = codigoLenguaje + "-XX";
            }

            string codigoBase = codigoIso;
            int sufijo = 1;

            while (idiomasExistentes.Any(idioma => string.Equals(idioma.CodigoISO, codigoIso, StringComparison.OrdinalIgnoreCase)))
            {
                string codigoLenguaje = codigoBase.Split('-')[0];

                if (codigoLenguaje.Length > 2)
                {
                    codigoLenguaje = codigoLenguaje.Substring(0, 2);
                }

                codigoIso = codigoLenguaje + "-X" + sufijo;
                sufijo++;
            }

            return codigoIso;
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("AgregarIdioma_Titulo", "Agregar Idioma");

            if (labelAgregarIdioma != null)
            {
                labelAgregarIdioma.Text = Trad("AgregarIdioma_Label_Agregar", "Agregar Idioma:");
            }

            if (ButtonGuardar != null)
            {
                ButtonGuardar.Text = Trad("AgregarIdioma_Boton_Guardar", "Guardar");
            }

            if (buttonCancelar != null)
            {
                buttonCancelar.Text = Trad("AgregarIdioma_Boton_Cancelar", "Cancelar");
            }
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
    }
}
