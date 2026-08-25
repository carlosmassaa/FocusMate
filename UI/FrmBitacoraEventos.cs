using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;
using System.Drawing;

namespace UI
{
    public partial class FrmBitacoraEventos : Form, IIdiomaObserver
    {
        private BitacoraBL servicioBitacora;

        public FrmBitacoraEventos()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            dtpDesde.Enabled = false;
            dtpHasta.Enabled = false;
        }

        public FrmBitacoraEventos(BitacoraBL servicio) : this()
        {
            servicioBitacora = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        public void Configurar(BitacoraBL servicio)
        {
            servicioBitacora = servicio ?? throw new ArgumentNullException(nameof(servicio));
        }

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            IdiomaService.Instancia.Suscribir(this);
            Dictionary<string, string> traducciones = IdiomaService.Instancia.ObtenerTraduccionesActuales();
            if (traducciones != null && traducciones.Count > 0)
            {
                ActualizarTraducciones(traducciones);
            }
            else
            {
                AplicarTraduccionesEstaticas();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs args)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(args);
        }

        private void btnBuscar_Click(object sender, EventArgs args)
        {
            if (!ValidarServicio())
            {
                return;
            }

            if (chkDesde.Checked && chkHasta.Checked && dtpDesde.Value > dtpHasta.Value)
            {
                MessageBox.Show(this, Trad("Bitacora_Validacion_Fechas", "La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'."), Trad("Bitacora_Msg_Titulo", "Bitácora"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BitacoraFiltros filtros = ConstruirFiltros();
                int usuarioActualId = SesionActual.Instance.UsuarioId;
                List<Bitacora> registros = servicioBitacora.Buscar(filtros, usuarioActualId);
                MostrarRegistros(registros);
            }
            catch (Exception excepcion)
            {
                MostrarErrorCarga(excepcion);
            }
        }

        private void btnExportarCsv_Click(object sender, EventArgs args)
        {
            if (dataGridView1.DataSource == null || dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show(this, Trad("Bitacora_Export_SinDatos", "No hay datos para exportar."), Trad("Bitacora_Msg_Titulo", "Bitácora"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = Trad("Bitacora_Export_FiltroCsv", "Archivo CSV (*.csv)|*.csv"),
                FileName = "Bitacora_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv",
                Title = Trad("Bitacora_Export_TituloDialogo", "Guardar CSV de Bitácora")
            };

            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                string contenidoCsv = ConstruirCsvDesdeGrid(dataGridView1);
                File.WriteAllText(saveFileDialog.FileName, contenidoCsv, new UTF8Encoding(true));
                MessageBox.Show(this, Trad("Bitacora_Export_Ok", "Exportación realizada correctamente."), Trad("Bitacora_Msg_Titulo", "Bitácora"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception excepcion)
            {
                MessageBox.Show(this, string.Format(Trad("Bitacora_Export_Error", "Error al exportar: {0}"), excepcion.Message), Trad("Bitacora_Msg_Titulo", "Bitácora"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConstruirCsvDesdeGrid(DataGridView grid)
        {
            StringBuilder csvBuilder = new StringBuilder();
            List<DataGridViewColumn> columnasVisiblesOrdenadas = grid.Columns.Cast<DataGridViewColumn>()
                .Where(columna => columna.Visible)
                .OrderBy(columna => columna.DisplayIndex)
                .ToList();

            csvBuilder.AppendLine(string.Join(",", columnasVisiblesOrdenadas.Select(columna => EscaparCampoCsv(columna.HeaderText))));

            foreach (DataGridViewRow fila in grid.Rows)
            {
                if (fila.IsNewRow)
                {
                    continue;
                }

                List<string> valores = new List<string>();
                foreach (DataGridViewColumn columna in columnasVisiblesOrdenadas)
                {
                    object celdaValor = fila.Cells[columna.Name].Value;
                    string textoCelda = celdaValor == null ? string.Empty : celdaValor.ToString();
                    valores.Add(EscaparCampoCsv(textoCelda));
                }

                csvBuilder.AppendLine(string.Join(",", valores));
            }

            return csvBuilder.ToString();
        }

        private string EscaparCampoCsv(string valor)
        {
            if (valor == null)
            {
                return "\"\"";
            }

            bool requiereComillas = valor.Contains(",") || valor.Contains(";") || valor.Contains("\"") || valor.Contains("\r") || valor.Contains("\n");
            valor = valor.Replace("\"", "\"\"");

            if (requiereComillas)
            {
                return "\"" + valor + "\"";
            }

            return valor;
        }

        private bool ValidarServicio()
        {
            if (servicioBitacora == null)
            {
                MessageBox.Show(this, Trad("Bitacora_Msg_ServicioNoInyectado", "El servicio de bitácora no fue inyectado."), Trad("Bitacora_Msg_Titulo", "Bitácora"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void MostrarRegistros(List<Bitacora> registros)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = registros;
            dataGridView1.AutoResizeColumns();

            if (dataGridView1.Columns.Contains("EntidadId"))
            {
                dataGridView1.Columns["EntidadId"].Visible = false;
            }

            AplicarTraduccionesGrid();
            dataGridView1.ClearSelection();
        }

        private void MostrarErrorCarga(Exception excepcion)
        {
            MessageBox.Show(this, string.Format(Trad("Bitacora_Msg_ErrorCarga", "Error al cargar la bitácora: {0}"), excepcion.Message), Trad("Bitacora_Msg_Titulo", "Bitácora"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private BitacoraFiltros ConstruirFiltros()
        {
            BitacoraFiltros filtros = new BitacoraFiltros();

            if (chkDesde.Checked)
            {
                filtros.FechaDesdeUtc = dtpDesde.Value.ToUniversalTime();
            }

            if (chkHasta.Checked)
            {
                filtros.FechaHastaUtc = dtpHasta.Value.ToUniversalTime();
            }

            if (!string.IsNullOrWhiteSpace(txtModulo.Text))
            {
                filtros.Modulo = txtModulo.Text.Trim();
            }

            if (!string.IsNullOrWhiteSpace(txtAccion.Text))
            {
                filtros.Accion = txtAccion.Text.Trim();
            }

            if (!string.IsNullOrWhiteSpace(txtResultado.Text))
            {
                filtros.Resultado = txtResultado.Text.Trim();
            }

            if (!string.IsNullOrWhiteSpace(txtEntidad.Text))
            {
                filtros.Entidad = txtEntidad.Text.Trim();
            }

            int usuarioIdFiltro;
            if (!string.IsNullOrWhiteSpace(txtUsuarioId.Text) && int.TryParse(txtUsuarioId.Text.Trim(), out usuarioIdFiltro))
            {
                filtros.UsuarioId = usuarioIdFiltro;
            }

            if (!string.IsNullOrWhiteSpace(txtTextoLibre.Text))
            {
                filtros.TextoLibre = txtTextoLibre.Text.Trim();
            }

            return filtros;
        }

        private void btnLimpiar_Click(object sender, EventArgs args)
        {
            chkDesde.Checked = false;
            chkHasta.Checked = false;
            txtModulo.Text = string.Empty;
            txtAccion.Text = string.Empty;
            txtResultado.Text = string.Empty;
            txtTextoLibre.Text = string.Empty;
            txtEntidad.Text = string.Empty;
            txtUsuarioId.Text = string.Empty;
            dataGridView1.DataSource = null;
        }

        private void chkDesde_CheckedChanged(object sender, EventArgs args)
        {
            dtpDesde.Enabled = chkDesde.Checked;
        }

        private void chkHasta_CheckedChanged(object sender, EventArgs args)
        {
            dtpHasta.Enabled = chkHasta.Checked;
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            AplicarTraduccionesEstaticas();
            AplicarTraduccionesGrid();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("Bitacora_Titulo", "Bitácora de Eventos");

            if (btnBuscar != null)
            {
                btnBuscar.Text = Trad("Bitacora_Boton_Buscar", "Buscar");
            }

            if (btnLimpiar != null)
            {
                btnLimpiar.Text = Trad("Bitacora_Boton_Limpiar", "Limpiar");
            }

            if (btnExportarCsv != null)
            {
                btnExportarCsv.Text = Trad("Bitacora_Boton_ExportarCsv", "Exportar CSV");
            }

            if (lblDesde != null)
            {
                lblDesde.Text = Trad("Bitacora_Label_Desde", "Desde:");
            }

            if (lblHasta != null)
            {
                lblHasta.Text = Trad("Bitacora_Label_Hasta", "Hasta:");
            }

            if (lblModulo != null)
            {
                lblModulo.Text = Trad("Bitacora_Label_Modulo", "Módulo:");
            }

            if (lblAccion != null)
            {
                lblAccion.Text = Trad("Bitacora_Label_Accion", "Acción:");
            }

            if (lblResultado != null)
            {
                lblResultado.Text = Trad("Bitacora_Label_Resultado", "Resultado:");
            }

            if (lblTextoLibre != null)
            {
                lblTextoLibre.Text = Trad("Bitacora_Label_TextoLibre", "Texto:");
            }

            if (lblEntidad != null)
            {
                lblEntidad.Text = Trad("Bitacora_Label_Entidad", "Entidad:");
            }

            if (lblUsuarioId != null)
            {
                lblUsuarioId.Text = Trad("Bitacora_Label_UsuarioId", "Usuario Id:");
            }
        }

        private void AplicarTraduccionesGrid()
        {
            if (dataGridView1 == null || dataGridView1.Columns == null || dataGridView1.Columns.Count == 0)
            {
                return;
            }

            SetHeader(dataGridView1, "FechaUtc", "Bitacora_Col_FechaUtc", "Fecha (UTC)");
            SetHeader(dataGridView1, "Modulo", "Bitacora_Col_Modulo", "Módulo");
            SetHeader(dataGridView1, "Accion", "Bitacora_Col_Accion", "Acción");
            SetHeader(dataGridView1, "Resultado", "Bitacora_Col_Resultado", "Resultado");
            SetHeader(dataGridView1, "Entidad", "Bitacora_Col_Entidad", "Entidad");
            SetHeader(dataGridView1, "UsuarioId", "Bitacora_Col_UsuarioId", "Usuario Id");
            SetHeader(dataGridView1, "Texto", "Bitacora_Col_Texto", "Texto");
        }

        private void SetHeader(DataGridView grid, string colName, string key, string fallback)
        {
            if (grid == null)
            {
                return;
            }

            DataGridViewColumn columna = grid.Columns[colName];
            if (columna == null)
            {
                return;
            }

            columna.HeaderText = Trad(key, fallback);
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
