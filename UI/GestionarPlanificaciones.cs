using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class GestionarPlanificaciones : Form, IIdiomaObserver
    {
        private readonly PlanificacionBL planificacionBL;
        private readonly UsuarioBL usuarioBL;
        private readonly TareaBL tareaBL;
        private readonly AuthManager authManager;

        private const string PermisoRevisarPlanificacion = "REVISAR_PLANIFICACION";
        private const string PermisoObservarPlanificacion = "OBSERVAR_PLANIFICACION";
        private const string PermisoAprobarPlanificacion = "APROBAR_PLANIFICACION";

        private bool permisoRevisarPlanificacion;
        private bool permisoObservarPlanificacion;
        private bool permisoAprobarPlanificacion;
        private bool puedeVerTodasLasPlanificaciones;

        private List<Planificacion> planificacionesCargadas;

        public GestionarPlanificaciones(AuthManager auth)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            authManager = auth;
            planificacionBL = new PlanificacionBL();
            usuarioBL = new UsuarioBL();
            tareaBL = new TareaBL();
            planificacionesCargadas = new List<Planificacion>();

            ConfigurarGridPlanificaciones();
            ConfigurarGridDetalle();
            ConfigurarGridTareasUsuario();
            AplicarPermisosUI();
            ConfigurarFiltros();
            AplicarTraduccionesEstaticas();
            CargarPlanificaciones();
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

        private void AplicarPermisosUI()
        {
            permisoRevisarPlanificacion = false;
            permisoObservarPlanificacion = false;
            permisoAprobarPlanificacion = false;

            if (authManager != null && authManager.EstaAutenticado)
            {
                permisoRevisarPlanificacion = authManager.ValidarPermiso(PermisoRevisarPlanificacion);
                permisoObservarPlanificacion = authManager.ValidarPermiso(PermisoObservarPlanificacion);
                permisoAprobarPlanificacion = authManager.ValidarPermiso(PermisoAprobarPlanificacion);
            }

            puedeVerTodasLasPlanificaciones = permisoRevisarPlanificacion || permisoObservarPlanificacion || permisoAprobarPlanificacion;

            comboBoxUsuarios.Visible = puedeVerTodasLasPlanificaciones;
            lblUsuario.Visible = puedeVerTodasLasPlanificaciones;

            btnRegistrarRevision.Enabled = permisoRevisarPlanificacion;
            btnRegistrarRevision.Visible = permisoRevisarPlanificacion;

            btnRegistrarObservacion.Enabled = permisoObservarPlanificacion;
            btnRegistrarObservacion.Visible = permisoObservarPlanificacion;

            btnAprobar.Enabled = permisoAprobarPlanificacion;
            btnAprobar.Visible = permisoAprobarPlanificacion;

            txtObservacion.ReadOnly = !permisoObservarPlanificacion && !permisoRevisarPlanificacion && !permisoAprobarPlanificacion;
        }

        private void ConfigurarFiltros()
        {
            comboBoxEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEstado.Items.Clear();
            comboBoxEstado.Items.Add(Trad("GestPlan_Combo_Todos", "Todos"));
            comboBoxEstado.Items.Add(EstadoPlanificacion.Generada);
            comboBoxEstado.Items.Add(EstadoPlanificacion.Revisada);
            comboBoxEstado.Items.Add(EstadoPlanificacion.Aprobada);
            comboBoxEstado.Items.Add(EstadoPlanificacion.Observada);
            comboBoxEstado.Items.Add(EstadoPlanificacion.Desactualizada);
            comboBoxEstado.SelectedIndex = 0;

            dateTimePickerDesde.Format = DateTimePickerFormat.Short;
            dateTimePickerHasta.Format = DateTimePickerFormat.Short;

            dateTimePickerDesde.Value = DateTime.Today.AddMonths(-1);
            dateTimePickerHasta.Value = DateTime.Today;

            if (puedeVerTodasLasPlanificaciones)
            {
                List<Usuario> usuarios = usuarioBL.Listar() ?? new List<Usuario>();

                Usuario usuarioTodos = new Usuario
                {
                    Id = 0,
                    NombreUsuario = Trad("GestPlan_Combo_Todos", "Todos")
                };

                usuarios.Insert(0, usuarioTodos);

                comboBoxUsuarios.DisplayMember = "NombreUsuario";
                comboBoxUsuarios.ValueMember = "Id";
                comboBoxUsuarios.DataSource = usuarios;
                comboBoxUsuarios.SelectedValue = 0;
            }
            else
            {
                comboBoxUsuarios.DataSource = null;
            }
        }

        private void ConfigurarGridPlanificaciones()
        {
            dataGridViewPlanificaciones.AutoGenerateColumns = false;
            dataGridViewPlanificaciones.AllowUserToAddRows = false;
            dataGridViewPlanificaciones.AllowUserToDeleteRows = false;
            dataGridViewPlanificaciones.ReadOnly = true;
            dataGridViewPlanificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlanificaciones.MultiSelect = false;
            dataGridViewPlanificaciones.Columns.Clear();

            dataGridViewPlanificaciones.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "PlanificacionId",Name = "PlanificacionId",HeaderText = Trad("GestPlan_Col_Id", "Id")});
            dataGridViewPlanificaciones.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "UsuarioId",Name = "UsuarioId",HeaderText = Trad("GestPlan_Col_Usuario", "Usuario")});
            dataGridViewPlanificaciones.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "SupervisorId",Name = "SupervisorId",HeaderText = Trad("GestPlan_Col_Supervisor", "Supervisor")});
            dataGridViewPlanificaciones.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "FechaGeneracionUtc",Name = "FechaGeneracionUtc",HeaderText = Trad("GestPlan_Col_Generada", "Generada")});
            dataGridViewPlanificaciones.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Estado",Name = "Estado",HeaderText = Trad("GestPlan_Col_Estado", "Estado")});

            dataGridViewPlanificaciones.SelectionChanged += dataGridViewPlanificaciones_SelectionChanged;
        }

        private void ConfigurarGridDetalle()
        {
            dataGridViewDetallePlanificacion.AutoGenerateColumns = false;
            dataGridViewDetallePlanificacion.AllowUserToAddRows = false;
            dataGridViewDetallePlanificacion.AllowUserToDeleteRows = false;
            dataGridViewDetallePlanificacion.ReadOnly = true;
            dataGridViewDetallePlanificacion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDetallePlanificacion.MultiSelect = false;
            dataGridViewDetallePlanificacion.Columns.Clear();

            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Orden",Name = "colOrden",HeaderText = Trad("GestPlan_Col_Orden", "Orden")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "TareaId",Name = "colTareaId",HeaderText = "TareaId",Visible = false});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Titulo",Name = "colTitulo",HeaderText = Trad("GestPlan_Col_Titulo", "Título"),AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "FechaLimite",Name = "colFechaLimite",HeaderText = Trad("GestPlan_Col_FechaLimite", "Fecha límite")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Importancia",Name = "colImportancia",HeaderText = Trad("GestPlan_Col_Importancia", "Importancia")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "EnergiaRequerida",Name = "colEnergia",HeaderText = Trad("GestPlan_Col_Energia", "Energía")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "DuracionEstimadaMin",Name = "colDuracion",HeaderText = Trad("GestPlan_Col_Duracion", "Duración")});
            dataGridViewDetallePlanificacion.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "ScorePrioridad",Name = "colScore",HeaderText = Trad("GestPlan_Col_Score", "Score")});
        }

        private void ConfigurarGridTareasUsuario()
        {
            dataGridViewTareasUsuario.AutoGenerateColumns = false;
            dataGridViewTareasUsuario.AllowUserToAddRows = false;
            dataGridViewTareasUsuario.AllowUserToDeleteRows = false;
            dataGridViewTareasUsuario.ReadOnly = true;
            dataGridViewTareasUsuario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTareasUsuario.MultiSelect = false;
            dataGridViewTareasUsuario.Columns.Clear();

            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "TareaId",Name = "colTareaUsuarioId",HeaderText = "Id",Visible = false});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Titulo",Name = "colTareaUsuarioTitulo",HeaderText = Trad("GestPlan_Col_Titulo", "Título")});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "FechaLimite",Name = "colTareaUsuarioFecha",HeaderText = Trad("GestPlan_Col_FechaLimite", "Fecha límite")});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Importancia",Name = "colTareaUsuarioImportancia",HeaderText = Trad("GestPlan_Col_Importancia", "Importancia")});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "EnergiaRequerida",Name = "colTareaUsuarioEnergia",HeaderText = Trad("GestPlan_Col_Energia", "Energía")});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "DuracionEstimadaMin",Name = "colTareaUsuarioDuracion",HeaderText = Trad("GestPlan_Col_Duracion", "Duración")});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "ScorePrioridad",Name = "colTareaUsuarioScore",HeaderText = Trad("GestPlan_Col_Score", "Score")});
            dataGridViewTareasUsuario.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "Estado",Name = "colTareaUsuarioEstado",HeaderText = Trad("GestPlan_Col_Estado", "Estado")});
        }

        private void CargarPlanificaciones()
        {
            if (puedeVerTodasLasPlanificaciones)
            {
                planificacionesCargadas = planificacionBL.ListarDisponibles();
            }
            else
            {
                int usuarioId = SesionActual.Instance.UsuarioId;

                if (usuarioId <= 0)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SinSesion", "No hay sesión activa."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                planificacionesCargadas = planificacionBL.ListarPorUsuario(usuarioId);
            }

            AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            List<Planificacion> planificacionesFiltradas = new List<Planificacion>();

            int usuarioSeleccionadoId = 0;

            if (puedeVerTodasLasPlanificaciones && comboBoxUsuarios.SelectedValue is int)
            {
                usuarioSeleccionadoId = (int)comboBoxUsuarios.SelectedValue;
            }

            object estadoSeleccionado = comboBoxEstado.SelectedItem;

            DateTime fechaDesde = dateTimePickerDesde.Value.Date;
            DateTime fechaHasta = dateTimePickerHasta.Value.Date;

            foreach (Planificacion planificacion in planificacionesCargadas)
            {
                bool cumpleUsuario = true;
                bool cumpleEstado = true;
                bool cumpleFecha = true;

                if (puedeVerTodasLasPlanificaciones && usuarioSeleccionadoId > 0)
                {
                    cumpleUsuario = planificacion.UsuarioId == usuarioSeleccionadoId;
                }

                if (estadoSeleccionado is EstadoPlanificacion)
                {
                    EstadoPlanificacion estado = (EstadoPlanificacion)estadoSeleccionado;
                    cumpleEstado = planificacion.Estado == estado;
                }

                DateTime fechaGeneracionLocal = planificacion.FechaGeneracionUtc.ToLocalTime().Date;
                cumpleFecha = fechaGeneracionLocal >= fechaDesde && fechaGeneracionLocal <= fechaHasta;

                if (cumpleUsuario && cumpleEstado && cumpleFecha)
                {
                    planificacionesFiltradas.Add(planificacion);
                }
            }

            dataGridViewPlanificaciones.DataSource = null;
            dataGridViewPlanificaciones.DataSource = planificacionesFiltradas;
            dataGridViewPlanificaciones.ClearSelection();

            dataGridViewDetallePlanificacion.DataSource = null;
            dataGridViewTareasUsuario.DataSource = null;
            txtObservacion.Text = string.Empty;
        }

        private int? ObtenerPlanificacionSeleccionadaId()
        {
            if (dataGridViewPlanificaciones.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell celda = dataGridViewPlanificaciones.CurrentRow.Cells["PlanificacionId"];

            if (celda == null || celda.Value == null)
            {
                return null;
            }

            int planificacionId;

            if (!int.TryParse(celda.Value.ToString(), out planificacionId))
            {
                return null;
            }

            return planificacionId;
        }

        private void CargarTareasUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                dataGridViewTareasUsuario.DataSource = null;
                return;
            }

            List<Tarea> tareas = tareaBL.ListarPorUsuario(usuarioId);

            dataGridViewTareasUsuario.DataSource = null;
            dataGridViewTareasUsuario.DataSource = tareas;
            dataGridViewTareasUsuario.ClearSelection();
            dataGridViewTareasUsuario.AutoResizeColumns();
        }

        private void CargarDetallePlanificacion(int planificacionId)
        {
            Planificacion planificacion = planificacionBL.Obtener(planificacionId);

            if (planificacion == null)
            {
                dataGridViewDetallePlanificacion.DataSource = null;
                dataGridViewTareasUsuario.DataSource = null;
                txtObservacion.Text = string.Empty;
                return;
            }

            CargarTareasUsuario(planificacion.UsuarioId);

            List<object> filas = new List<object>();

            foreach (PlanificacionDetalle detalle in planificacion.Detalles)
            {
                if (detalle.Tarea != null)
                {
                    filas.Add(new{Orden = detalle.Orden,TareaId = detalle.TareaId,Titulo = detalle.Tarea.Titulo,FechaLimite = detalle.Tarea.FechaLimite,Importancia = detalle.Tarea.Importancia,EnergiaRequerida = detalle.Tarea.EnergiaRequerida,DuracionEstimadaMin = detalle.Tarea.DuracionEstimadaMin,ScorePrioridad = detalle.ScorePrioridad});
                }
            }

            dataGridViewDetallePlanificacion.DataSource = null;
            dataGridViewDetallePlanificacion.DataSource = filas;
            dataGridViewDetallePlanificacion.AutoResizeColumns();

            if (!string.IsNullOrWhiteSpace(planificacion.ObservacionRevision))
            {
                txtObservacion.Text = planificacion.ObservacionRevision;
            }
            else if (!string.IsNullOrWhiteSpace(planificacion.ObservacionAprobacion))
            {
                txtObservacion.Text = planificacion.ObservacionAprobacion;
            }
            else
            {
                txtObservacion.Text = string.Empty;
            }
        }

        private void RegistrarRevision(bool planificacionAdecuada)
        {
            try
            {
                if (planificacionAdecuada && !permisoRevisarPlanificacion)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SinPermisoRevisar", "No tiene permiso para revisar planificaciones."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                if (!planificacionAdecuada && !permisoObservarPlanificacion)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SinPermisoObservar", "No tiene permiso para observar planificaciones."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                int? planificacionId = ObtenerPlanificacionSeleccionadaId();

                if (!planificacionId.HasValue)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SeleccionePlanificacion", "Seleccione una planificación."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                    return;
                }

                int supervisorId = SesionActual.Instance.UsuarioId;

                if (supervisorId <= 0)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SinSesion", "No hay sesión activa."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                planificacionBL.RegistrarRevision(planificacionId.Value, supervisorId, txtObservacion.Text, planificacionAdecuada);

                MessageBox.Show(Trad("GestPlan_Msg_RevisionOk", "La revisión fue registrada correctamente."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                CargarPlanificaciones();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            if (puedeVerTodasLasPlanificaciones && comboBoxUsuarios.Items.Count > 0)
            {
                comboBoxUsuarios.SelectedIndex = 0;
            }

            comboBoxEstado.SelectedIndex = 0;
            dateTimePickerDesde.Value = DateTime.Today.AddYears(-1);
            dateTimePickerHasta.Value = DateTime.Today.AddDays(1);

            AplicarFiltros();
        }

        private void dataGridViewPlanificaciones_SelectionChanged(object sender, EventArgs e)
        {
            int? planificacionId = ObtenerPlanificacionSeleccionadaId();

            if (planificacionId.HasValue)
            {
                CargarDetallePlanificacion(planificacionId.Value);
            }
        }

        private void btnCargarPlanificaciones_Click(object sender, EventArgs e)
        {
            CargarPlanificaciones();
        }

        private void btnRegistrarRevision_Click(object sender, EventArgs e)
        {
            RegistrarRevision(true);
        }

        private void btnRegistrarObservacion_Click(object sender, EventArgs e)
        {
            RegistrarRevision(false);
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!permisoAprobarPlanificacion)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SinPermisoAprobar", "No tiene permiso para aprobar planificaciones."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                int? planificacionId = ObtenerPlanificacionSeleccionadaId();

                if (!planificacionId.HasValue)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SeleccionePlanificacion", "Seleccione una planificación."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                    return;
                }

                int supervisorId = SesionActual.Instance.UsuarioId;

                if (supervisorId <= 0)
                {
                    MessageBox.Show(Trad("GestPlan_Msg_SinSesion", "No hay sesión activa."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Warning);

                    return;
                }

                DialogResult respuesta = MessageBox.Show(Trad("GestPlan_Msg_ConfirmarAprobacion", "¿Confirma la aprobación de la planificación?"),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                planificacionBL.AprobarPlanificacion(planificacionId.Value, supervisorId, txtObservacion.Text);

                MessageBox.Show(Trad("GestPlan_Msg_AprobadaOk", "La planificación fue aprobada correctamente."),Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Information);

                CargarPlanificaciones();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message,Trad("GestPlan_Msg_Titulo", "Planificación"),MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
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
            Text = Trad("GestPlan_Titulo", "Gestionar Planificaciones");

            if (lblTitulo != null)
            {
                lblTitulo.Text = Trad("GestPlan_Label_Titulo", "Gestionar planificaciones");
            }

            if (lblUsuario != null)
            {
                lblUsuario.Text = Trad("GestPlan_Label_Usuario", "Usuario:");
            }

            if (lblEstado != null)
            {
                lblEstado.Text = Trad("GestPlan_Label_Estado", "Estado:");
            }

            if (lblDesde != null)
            {
                lblDesde.Text = Trad("GestPlan_Label_Desde", "Desde:");
            }

            if (lblHasta != null)
            {
                lblHasta.Text = Trad("GestPlan_Label_Hasta", "Hasta:");
            }

            if (lblTareasUsuario != null)
            {
                lblTareasUsuario.Text = Trad("GestPlan_Label_TareasUsuario", "Tareas del usuario");
            }

            if (lblObservacion != null)
            {
                lblObservacion.Text = Trad("GestPlan_Label_Observacion", "Observación:");
            }

            if (btnAplicarFiltros != null)
            {
                btnAplicarFiltros.Text = Trad("GestPlan_Boton_AplicarFiltros", "Aplicar filtros");
            }

            if (btnLimpiarFiltros != null)
            {
                btnLimpiarFiltros.Text = Trad("GestPlan_Boton_LimpiarFiltros", "Limpiar filtros");
            }

            if (btnCargarPlanificaciones != null)
            {
                btnCargarPlanificaciones.Text = Trad("GestPlan_Boton_ActualizarListado", "Actualizar listado");
            }

            if (btnRegistrarRevision != null)
            {
                btnRegistrarRevision.Text = Trad("GestPlan_Boton_RegistrarRevision", "Registrar revisión");
            }

            if (btnRegistrarObservacion != null)
            {
                btnRegistrarObservacion.Text = Trad("GestPlan_Boton_RegistrarObservacion", "Registrar observación");
            }

            if (btnAprobar != null)
            {
                btnAprobar.Text = Trad("GestPlan_Boton_Aprobar", "Aprobar");
            }

            if (btnCerrar != null)
            {
                btnCerrar.Text = Trad("GestPlan_Boton_Cerrar", "Cerrar");
            }

            SetHeader(dataGridViewPlanificaciones, "PlanificacionId", "GestPlan_Col_Id", "Id");
            SetHeader(dataGridViewPlanificaciones, "UsuarioId", "GestPlan_Col_Usuario", "Usuario");
            SetHeader(dataGridViewPlanificaciones, "SupervisorId", "GestPlan_Col_Supervisor", "Supervisor");
            SetHeader(dataGridViewPlanificaciones, "FechaGeneracionUtc", "GestPlan_Col_Generada", "Generada");
            SetHeader(dataGridViewPlanificaciones, "Estado", "GestPlan_Col_Estado", "Estado");

            SetHeader(dataGridViewDetallePlanificacion, "colOrden", "GestPlan_Col_Orden", "Orden");
            SetHeader(dataGridViewDetallePlanificacion, "colTitulo", "GestPlan_Col_Titulo", "Título");
            SetHeader(dataGridViewDetallePlanificacion, "colFechaLimite", "GestPlan_Col_FechaLimite", "Fecha límite");
            SetHeader(dataGridViewDetallePlanificacion, "colImportancia", "GestPlan_Col_Importancia", "Importancia");
            SetHeader(dataGridViewDetallePlanificacion, "colEnergia", "GestPlan_Col_Energia", "Energía");
            SetHeader(dataGridViewDetallePlanificacion, "colDuracion", "GestPlan_Col_Duracion", "Duración");
            SetHeader(dataGridViewDetallePlanificacion, "colScore", "GestPlan_Col_Score", "Score");

            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioTitulo", "GestPlan_Col_Titulo", "Título");
            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioFecha", "GestPlan_Col_FechaLimite", "Fecha límite");
            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioImportancia", "GestPlan_Col_Importancia", "Importancia");
            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioEnergia", "GestPlan_Col_Energia", "Energía");
            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioDuracion", "GestPlan_Col_Duracion", "Duración");
            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioScore", "GestPlan_Col_Score", "Score");
            SetHeader(dataGridViewTareasUsuario, "colTareaUsuarioEstado", "GestPlan_Col_Estado", "Estado");
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

        private void GestionarPlanificaciones_Load(object sender, EventArgs e)
        {

        }
    }
}