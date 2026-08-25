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
    public partial class HistorialListadoTarea : Form, IIdiomaObserver
    {
        private readonly TareaBL _tareaBl = new TareaBL();
        private readonly int _tareaId;

        public HistorialListadoTarea(int tareaId)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _tareaId = tareaId;
            ConfigurarGrid();
            AplicarTraduccionesEstaticas();
            CargarHistorial();
            TopMost = true;
            StartPosition = FormStartPosition.CenterParent;
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

        private void ConfigurarGrid()
        {
            dataGridViewHistorialTarea.AutoGenerateColumns = false;
            dataGridViewHistorialTarea.ReadOnly = true;
            dataGridViewHistorialTarea.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewHistorialTarea.MultiSelect = false;
            dataGridViewHistorialTarea.AllowUserToResizeRows = false;
            dataGridViewHistorialTarea.AllowUserToResizeColumns = true;
            dataGridViewHistorialTarea.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewHistorialTarea.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewHistorialTarea.Columns.Clear();

            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HistorialId", Name = "HistorialId", Visible = false });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaUtc", Name = "colFechaUtc", HeaderText = Trad("HistTarea_Col_FechaUtc", "Fecha (UTC)") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UsuarioOperacionId", Name = "colUsuarioOperacionId", HeaderText = Trad("HistTarea_Col_UsuarioOperacionId", "Usuario Operación Id") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "UsuarioIdPropietario", Name = "colUsuarioPropietarioId", HeaderText = Trad("HistTarea_Col_UsuarioPropietarioId", "Usuario Propietario Id") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Accion", Name = "colAccion", HeaderText = Trad("HistTarea_Col_Accion", "Acción") });

            DataGridViewTextBoxColumn columnaTitulo = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Titulo",
                Name = "colTitulo",
                HeaderText = Trad("HistTarea_Col_Titulo", "Título")
            };

            DataGridViewTextBoxColumn columnaDescripcion = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Descripcion",
                Name = "colDescripcion",
                HeaderText = Trad("HistTarea_Col_Descripcion", "Descripción")
            };

            dataGridViewHistorialTarea.Columns.Add(columnaTitulo);
            dataGridViewHistorialTarea.Columns.Add(columnaDescripcion);
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaLimite", Name = "colFechaLimite", HeaderText = Trad("HistTarea_Col_FechaLimite", "Fecha límite") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Importancia", Name = "colImportancia", HeaderText = Trad("HistTarea_Col_Importancia", "Importancia") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EnergiaRequerida", Name = "colEnergia", HeaderText = Trad("HistTarea_Col_Energia", "Energía") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DuracionEstimadaMin", Name = "colDuracion", HeaderText = Trad("HistTarea_Col_Duracion", "Duración (min)") });
            dataGridViewHistorialTarea.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", Name = "colEstado", HeaderText = Trad("HistTarea_Col_Estado", "Estado") });

            dataGridViewHistorialTarea.CellDoubleClick += dataGridViewHistorialTarea_CellDoubleClick;
        }

        private void CargarHistorial()
        {
            List<TareaHistorialEntry> historial = _tareaBl.ListarHistorial(_tareaId)
                .OrderByDescending(entradaHistorial => entradaHistorial.FechaUtc)
                .ThenByDescending(entradaHistorial => entradaHistorial.HistorialId)
                .ToList();

            dataGridViewHistorialTarea.DataSource = null;
            dataGridViewHistorialTarea.DataSource = historial;

            AjustarColumnasPostCarga();
            dataGridViewHistorialTarea.ClearSelection();

            if (historial.Any())
            {
                dataGridViewHistorialTarea.Rows[0].Selected = true;
            }
        }

        private void AjustarColumnasPostCarga()
        {
            dataGridViewHistorialTarea.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridViewHistorialTarea.AutoResizeRows();

            int anchoTotalColumnas = dataGridViewHistorialTarea.Columns.Cast<DataGridViewColumn>().Where(columna => columna.Visible).Sum(columna => columna.Width);
            int anchoDisponible = dataGridViewHistorialTarea.ClientSize.Width;

            if (anchoTotalColumnas < anchoDisponible)
            {
                if (dataGridViewHistorialTarea.Columns.Contains("colTitulo"))
                {
                    dataGridViewHistorialTarea.Columns["colTitulo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dataGridViewHistorialTarea.Columns.Contains("colDescripcion"))
                {
                    dataGridViewHistorialTarea.Columns["colDescripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void RevertirFila(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridViewHistorialTarea.Rows.Count)
            {
                return;
            }

            DataGridViewRow filaSeleccionada = dataGridViewHistorialTarea.Rows[rowIndex];

            int historialId;

            if (filaSeleccionada.DataBoundItem is TareaHistorialEntry entradaHistorial)
            {
                historialId = entradaHistorial.HistorialId;
            }
            else
            {
                DataGridViewCell celdaHistorial = filaSeleccionada.Cells["HistorialId"];

                if (celdaHistorial?.Value == null || !int.TryParse(celdaHistorial.Value.ToString(), out historialId))
                {
                    return;
                }
            }

            try
            {
                _tareaBl.RevertirA(_tareaId, historialId);
                CargarHistorial();
                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                string mensaje = string.Format(Trad("HistTarea_Msg_RevertirError", "No fue posible revertir la tarea: {0}"), exception.Message);
                MessageBox.Show(this, mensaje, Trad("HistTarea_Msg_Titulo", "Historial"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewHistorialTarea_CellDoubleClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.RowIndex < 0)
            {
                return;
            }

            RevertirFila(eventArgs.RowIndex);
        }

        private void btnRevertirSeleccion_Click(object sender, EventArgs eventArgs)
        {
            if (dataGridViewHistorialTarea.CurrentRow == null)
            {
                return;
            }

            RevertirFila(dataGridViewHistorialTarea.CurrentRow.Index);
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            AplicarTraduccionesEstaticas();
            AjustarColumnasPostCarga();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("HistTarea_Titulo", "Historial");

            if (lblHistoriaTarea != null)
            {
                lblHistoriaTarea.Text = Trad("HistTarea_Label_Encabezado", "Historial de la Tarea");
            }

            if (btnRevertirSeleccion != null)
            {
                btnRevertirSeleccion.Text = Trad("HistTarea_Boton_Revertir", "Recomponer");
            }

            SetHeader("colFechaUtc", "HistTarea_Col_FechaUtc", "Fecha (UTC)");
            SetHeader("colUsuarioOperacionId", "HistTarea_Col_UsuarioOperacionId", "Usuario Operación Id");
            SetHeader("colUsuarioPropietarioId", "HistTarea_Col_UsuarioPropietarioId", "Usuario Propietario Id");
            SetHeader("colAccion", "HistTarea_Col_Accion", "Acción");
            SetHeader("colTitulo", "HistTarea_Col_Titulo", "Título");
            SetHeader("colDescripcion", "HistTarea_Col_Descripcion", "Descripción");
            SetHeader("colFechaLimite", "HistTarea_Col_FechaLimite", "Fecha límite");
            SetHeader("colImportancia", "HistTarea_Col_Importancia", "Importancia");
            SetHeader("colEnergia", "HistTarea_Col_Energia", "Energía");
            SetHeader("colDuracion", "HistTarea_Col_Duracion", "Duración (min)");
            SetHeader("colEstado", "HistTarea_Col_Estado", "Estado");
        }

        private void SetHeader(string colName, string key, string fallback)
        {
            DataGridViewColumn columna = dataGridViewHistorialTarea.Columns[colName];

            if (columna == null)
            {
                return;
            }

            string textoTraducido = IdiomaService.Instancia.Traducir(key);

            if (!string.IsNullOrWhiteSpace(textoTraducido) && textoTraducido != key)
            {
                columna.HeaderText = textoTraducido;
            }
            else
            {
                columna.HeaderText = fallback;
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

        private void dataGridViewHistorialTarea_CellContentClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
        }
    }
}
