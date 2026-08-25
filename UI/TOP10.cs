using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class TOP10 : Form, IIdiomaObserver
    {
        private readonly PlanificacionBL planificacionBL;
        private readonly TareaBL tareaBL;

        private Planificacion planificacionActual;

        public TOP10()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            planificacionBL = new PlanificacionBL();
            tareaBL = new TareaBL();

            ConfigurarGrid();
            ConfigurarComboEstado();
            AplicarTraduccionesEstaticas();
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

            CargarUltimaPlanificacionAprobada();
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void TOP10_Load(object sender, EventArgs e)
        {
        }

        private void ConfigurarGrid()
        {
            dataGridViewTop10.AutoGenerateColumns = false;
            dataGridViewTop10.AllowUserToAddRows = false;
            dataGridViewTop10.AllowUserToDeleteRows = false;
            dataGridViewTop10.ReadOnly = true;
            dataGridViewTop10.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTop10.MultiSelect = false;
            dataGridViewTop10.RowHeadersWidth = 25;
            dataGridViewTop10.Columns.Clear();

            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Orden", Name = "colOrden", HeaderText = Trad("TOP10_Col_Orden", "Orden"), Width = 60 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TareaId", Name = "colTareaId", HeaderText = "TareaId", Visible = false });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", Name = "colTitulo", HeaderText = Trad("TOP10_Col_Titulo", "Título"), Width = 220 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "FechaLimite", Name = "colFechaLimite", HeaderText = Trad("TOP10_Col_FechaLimite", "Fecha límite"), Width = 100 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Importancia", Name = "colImportancia", HeaderText = Trad("TOP10_Col_Importancia", "Importancia"), Width = 90 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "EnergiaRequerida", Name = "colEnergia", HeaderText = Trad("TOP10_Col_Energia", "Energía"), Width = 80 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DuracionEstimadaMin", Name = "colDuracion", HeaderText = Trad("TOP10_Col_Duracion", "Duración"), Width = 80 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ScorePrioridad", Name = "colScore", HeaderText = Trad("TOP10_Col_Score", "Score"), Width = 80 });
            dataGridViewTop10.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Estado", Name = "colEstado", HeaderText = Trad("TOP10_Col_Estado", "Estado"), Width = 90 });
        }

        private void ConfigurarComboEstado()
        {
            comboBoxEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEstado.DataSource = Enum.GetValues(typeof(EstadoTarea));
        }

        private void CargarUltimaPlanificacionAprobada()
        {
            try
            {
                int usuarioId = SesionActual.Instance.UsuarioId;

                if (usuarioId <= 0)
                {
                    MessageBox.Show(Trad("TOP10_Msg_SinSesion", "No hay sesión activa."),Trad("TOP10_Msg_Titulo", "TOP 10"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                planificacionActual = planificacionBL.ObtenerUltimaAprobadaPorUsuario(usuarioId);

                if (planificacionActual == null)
                {
                    dataGridViewTop10.DataSource = null;
                    lblInfo.Text = Trad("TOP10_Label_SinPlanificacion", "No existe una planificación aprobada para el usuario actual.");
                    return;
                }

                lblInfo.Text = string.Format(
                    Trad("TOP10_Label_UltimaAprobada", "Última planificación aprobada: {0}"),
                    planificacionActual.FechaAprobacionUtc);

                List<object> filas = new List<object>();

                foreach (PlanificacionDetalle detalle in planificacionActual.Detalles)
                {
                    if (detalle.Tarea != null)
                    {
                        filas.Add(new{Orden = detalle.Orden,TareaId = detalle.TareaId,Titulo = detalle.Tarea.Titulo,FechaLimite = detalle.Tarea.FechaLimite,Importancia = detalle.Tarea.Importancia,EnergiaRequerida = detalle.Tarea.EnergiaRequerida,DuracionEstimadaMin = detalle.Tarea.DuracionEstimadaMin,ScorePrioridad = detalle.ScorePrioridad,Estado = detalle.Tarea.Estado});
                    }
                }

                dataGridViewTop10.DataSource = null;
                dataGridViewTop10.DataSource = filas;
                dataGridViewTop10.ClearSelection();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,Trad("TOP10_Msg_Titulo", "TOP 10"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private int? ObtenerTareaSeleccionadaId()
        {
            if (dataGridViewTop10.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell celda = dataGridViewTop10.CurrentRow.Cells["colTareaId"];

            if (celda == null || celda.Value == null)
            {
                return null;
            }

            int tareaId;

            if (!int.TryParse(celda.Value.ToString(), out tareaId))
            {
                return null;
            }

            return tareaId;
        }

        private void btnActualizarEstado_Click(object sender, EventArgs e)
        {
            try
            {
                int? tareaId = ObtenerTareaSeleccionadaId();

                if (!tareaId.HasValue)
                {
                    MessageBox.Show(Trad("TOP10_Msg_SeleccioneTarea", "Seleccione una tarea."),Trad("TOP10_Msg_Titulo", "TOP 10"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                    return;
                }

                if (!(comboBoxEstado.SelectedItem is EstadoTarea))
                {
                    MessageBox.Show(Trad("TOP10_Msg_SeleccioneEstado", "Seleccione un estado válido."),Trad("TOP10_Msg_Titulo", "TOP 10"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                EstadoTarea nuevoEstado = (EstadoTarea)comboBoxEstado.SelectedItem;
                int usuarioId = SesionActual.Instance.UsuarioId;

                tareaBL.CambiarEstadoDesdeTop10(tareaId.Value, usuarioId, nuevoEstado);

                MessageBox.Show(Trad("TOP10_Msg_EstadoActualizado", "El estado de la tarea fue actualizado correctamente."),Trad("TOP10_Msg_Titulo", "TOP 10"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                CargarUltimaPlanificacionAprobada();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,Trad("TOP10_Msg_Titulo", "TOP 10"),MessageBoxButtons.OK,MessageBoxIcon.Error);}
        }

        private void btnActualizarListado_Click(object sender, EventArgs e)
        {
            CargarUltimaPlanificacionAprobada();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("TOP10_Titulo", "TOP 10");

            if (lblTitulo != null)
            {
                lblTitulo.Text = Trad("TOP10_Label_Titulo", "TOP 10 - Planificación aprobada");
            }

            if (lblInfo != null)
            {
                lblInfo.Text = Trad("TOP10_Label_Info", "Última planificación aprobada:");
            }

            if (lblEstado != null)
            {
                lblEstado.Text = Trad("TOP10_Label_Estado", "Estado:");
            }

            if (btnActualizarEstado != null)
            {
                btnActualizarEstado.Text = Trad("TOP10_Boton_ActualizarEstado", "Actualizar estado");
            }

            if (btnActualizarListado != null)
            {
                btnActualizarListado.Text = Trad("TOP10_Boton_ActualizarListado", "Actualizar listado");
            }

            if (btnCerrar != null)
            {
                btnCerrar.Text = Trad("TOP10_Boton_Cerrar", "Cerrar");
            }

            SetHeader(dataGridViewTop10, "colOrden", "TOP10_Col_Orden", "Orden");
            SetHeader(dataGridViewTop10, "colTitulo", "TOP10_Col_Titulo", "Título");
            SetHeader(dataGridViewTop10, "colFechaLimite", "TOP10_Col_FechaLimite", "Fecha límite");
            SetHeader(dataGridViewTop10, "colImportancia", "TOP10_Col_Importancia", "Importancia");
            SetHeader(dataGridViewTop10, "colEnergia", "TOP10_Col_Energia", "Energía");
            SetHeader(dataGridViewTop10, "colDuracion", "TOP10_Col_Duracion", "Duración");
            SetHeader(dataGridViewTop10, "colScore", "TOP10_Col_Score", "Score");
            SetHeader(dataGridViewTop10, "colEstado", "TOP10_Col_Estado", "Estado");
        }

        private void SetHeader(DataGridView grid, string nombreColumna, string clave, string textoPredeterminado)
        {
            if (grid == null)
            {
                return;
            }

            if (!grid.Columns.Contains(nombreColumna))
            {
                return;
            }

            grid.Columns[nombreColumna].HeaderText = Trad(clave, textoPredeterminado);
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

        private void dataGridViewTop10_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}