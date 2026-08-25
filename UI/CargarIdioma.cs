using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BL;
using Servicioss;
using System.Drawing;
using BE;

namespace UI
{
    public partial class CargarIdioma : Form, IIdiomaObserver
    {
        private readonly IdiomaBL _idiomaBL = new IdiomaBL();

        private static readonly string[] ClavesMain = new[]
        {
            "Main_Menu_Tareas","Main_Menu_Bitacora","Main_Menu_TreeViewPermisos","Main_Menu_Administracion",
            "Main_Menu_AdministrarPermisos","Main_Menu_AdministrarFamilias","Main_Menu_CargarIdioma","Main_Menu_CrearTarea",
            "Main_Menu_VerTareas","Main_Menu_CerrarSesion","Main_Menu_CambiarIdioma","Main_Menu_Configuracion",
            "Main_Titulo_Base","Main_Titulo_Usuario","Main_Msg_CerrarSesion_Pregunta","Main_Msg_Confirmar",
            "Main_Menu_AdministrarBackup","Main_Menu_GestionarUsuario"
        };

        private BindingList<FilaVM> _filas = new BindingList<FilaVM>();

        private sealed class FilaVM
        {
            public string Clave { get; set; }
            public string TextoBase { get; set; }
            public string Traduccion { get; set; }
        }

        public CargarIdioma()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            btnCancelar.Click += (sender, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };
            btnGuardar.Click += btnGuardar_Click;
            btnCompletar.Click += btnCompletar_Click;
            cboIdioma.SelectedIndexChanged += cboIdioma_SelectedIndexChanged;

            dgvTraducciones.AutoGenerateColumns = false;
            dgvTraducciones.DataSource = _filas;
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

            CargarIdiomas();
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
                List<Idioma> idiomas = _idiomaBL.ListarIdiomas() ?? new List<Idioma>();
                cboIdioma.DisplayMember = "Nombre";
                cboIdioma.ValueMember = "Id";
                cboIdioma.DataSource = idiomas;
                if (idiomas.Count > 0)
                {
                    cboIdioma.SelectedIndex = 0;
                }
            }
            catch
            {
            }
        }

        private void cboIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cboIdioma.SelectedValue is int id) || id <= 0)
            {
                return;
            }

            CargarTraducciones(id);
        }

        private void CargarTraducciones(int idIdioma)
        {
            _filas.Clear();
            IEnumerable<EtiquetaTraduccion> traducciones = _idiomaBL.ObtenerTraduccionesPorClaves(idIdioma, ClavesMain);

            foreach (EtiquetaTraduccion traduccion in traducciones)
            {
                string textoBaseCalculado = string.IsNullOrWhiteSpace(traduccion.TextoBase) ? traduccion.Clave : traduccion.TextoBase;

                _filas.Add(new FilaVM
                {
                    Clave = traduccion.Clave,
                    TextoBase = textoBaseCalculado,
                    Traduccion = traduccion.Texto
                });
            }

            AplicarHeadersGrid();
            ActualizarEstado();
        }

        private void ActualizarEstado()
        {
            int total = _filas.Count;
            int completos = _filas.Count(fila => !string.IsNullOrWhiteSpace(fila.Traduccion) && !EsPlaceholder(fila.Traduccion));

            int pct = total == 0 ? 0 : (int)Math.Round((double)completos * 100.0 / total, MidpointRounding.AwayFromZero);

            if (lblEstado != null)
            {
                lblEstado.Text = string.Format(Trad("CargarIdi_Label_EstadoFmt", "Estado: {0}/{1} ({2}%)"), completos, total, pct);
            }
        }

        private static bool EsPlaceholder(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return true;
            }

            string textoRecortado = texto.Trim();
            if (string.Equals(textoRecortado, "<Traduccion>", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (textoRecortado.StartsWith("<") && textoRecortado.EndsWith(">") && textoRecortado.Length > 2)
            {
                return true;
            }

            return false;
        }

        private void btnCompletar_Click(object sender, EventArgs e)
        {
            foreach (FilaVM fila in _filas)
            {
                if (string.IsNullOrWhiteSpace(fila.Traduccion))
                {
                    string textoBaseCalculado = string.IsNullOrWhiteSpace(fila.TextoBase) ? fila.Clave : fila.TextoBase;
                    fila.Traduccion = "<" + textoBaseCalculado + ">";
                }
            }

            dgvTraducciones.Refresh();
            ActualizarEstado();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!(cboIdioma.SelectedValue is int idIdioma) || idIdioma <= 0)
            {
                return;
            }

            try
            {
                List<EtiquetaTraduccion> aGuardar = _filas
                    .Select(fila =>
                    {
                        string textoBaseCalculado = string.IsNullOrWhiteSpace(fila.TextoBase) ? fila.Clave : fila.TextoBase;
                        return new EtiquetaTraduccion
                        {
                            Clave = fila.Clave,
                            TextoBase = textoBaseCalculado,
                            Texto = fila.Traduccion ?? string.Empty
                        };
                    })
                    .ToList();

                _idiomaBL.GuardarTraducciones(idIdioma, aGuardar);

                MessageBox.Show(this, Trad("CargarIdi_Msg_GuardadoOk", "Traducciones guardadas."), Trad("CargarIdi_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                ActualizarEstado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(Trad("CargarIdi_Msg_ErrorGuardar", "Error al guardar: {0}"), ex.Message), Trad("CargarIdi_Msg_Titulo", "Idioma"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            AplicarTraduccionesEstaticas();
            AplicarHeadersGrid();
            ActualizarEstado();
        }

        private void AplicarTraduccionesEstaticas()
        {
            this.Text = Trad("CargarIdi_Titulo", "Cargar Idiomas");

            if (lblTitulo != null)
            {
                lblTitulo.Text = Trad("CargarIdi_Label_Titulo", "Cargar/Editar traducciones (Main)");
            }
            if (lblIdioma != null)
            {
                lblIdioma.Text = Trad("CargarIdi_Label_Idioma", "Idioma:");
            }
            if (lblEstado != null)
            {
                lblEstado.Text = string.Format(Trad("CargarIdi_Label_EstadoFmt", "Estado: {0}/{1} ({2}%)"), 0, 0, 0);
            }

            if (btnGuardar != null)
            {
                btnGuardar.Text = Trad("CargarIdi_Boton_Guardar", "Guardar");
            }
            if (btnCancelar != null)
            {
                btnCancelar.Text = Trad("CargarIdi_Boton_Cancelar", "Cancelar");
            }
            if (btnCompletar != null)
            {
                btnCompletar.Text = Trad("CargarIdi_Boton_Completar", "Completar faltantes");
            }
        }

        private void AplicarHeadersGrid()
        {
            SetHeader(dgvTraducciones, "colClave", "CargarIdi_Col_Clave", "Clave");
            SetHeader(dgvTraducciones, "colTextoBase", "CargarIdi_Col_TextoBase", "Texto base");
            SetHeader(dgvTraducciones, "colTraduccion", "CargarIdi_Col_Traduccion", "Traducción");
        }

        private void SetHeader(DataGridView grid, string colName, string key, string fallback)
        {
            if (grid == null)
            {
                return;
            }
            var column = grid.Columns[colName];
            if (column == null)
            {
                return;
            }
            column.HeaderText = Trad(key, fallback);
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

        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
        }
    }
}
