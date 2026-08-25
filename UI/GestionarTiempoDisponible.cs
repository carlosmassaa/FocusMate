using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class GestionarTiempoDisponible : Form, IIdiomaObserver
    {
        private readonly TiempoDisponibleBL tiempoDisponibleBL;
        private readonly PlanificacionBL planificacionBL;
        private readonly UsuarioBL usuarioBL;
        private readonly AuthManager authManager;

        private List<BloqueCalendario> agendaGeneradaActual;

        private const string PermisoGestionarTiempoDisponibleUsuarios = "GESTIONAR_TIEMPO_DISPONIBLE_USUARIOS";

        private const int MargenIzquierdoCalendario = 70;
        private const int MargenSuperiorCalendario = 45;
        private const int AnchoColumnaDia = 190;
        private const int AltoPorMinuto = 2;
        private const int AltoMinimoBloque = 24;

        public GestionarTiempoDisponible()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            tiempoDisponibleBL = new TiempoDisponibleBL();
            planificacionBL = new PlanificacionBL();
            usuarioBL = new UsuarioBL();
            agendaGeneradaActual = null;

            ConfigurarControles();
            ConfigurarUsuarios();
            CargarDatos();
        }

        public GestionarTiempoDisponible(AuthManager auth) : this()
        {
            authManager = auth;

            ConfigurarUsuarios();
            CargarDatos();
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

        private void ConfigurarControles()
        {
            comboBoxSeleccionarUsuarios.DropDownStyle = ComboBoxStyle.DropDownList;

            panelCalendario.AutoScroll = true;
            panelCalendario.BackColor = Color.White;

            lblMensaje.Text = string.Empty;
            lblDetalleTitulo.Text = Trad("TiempoDisponible_Label_Detalle", "Detalle");
            lblDetalleTexto.Text = Trad("TiempoDisponible_Label_SeleccioneBloque", "Seleccione un bloque del calendario.");
        }

        private bool PuedeSeleccionarUsuarios()
        {
            if (authManager == null)
            {
                return false;
            }

            return authManager.ValidarPermiso(PermisoGestionarTiempoDisponibleUsuarios);
        }

        private void ConfigurarUsuarios()
        {
            bool puedeSeleccionarUsuario = PuedeSeleccionarUsuarios();

            comboBoxSeleccionarUsuarios.Visible = puedeSeleccionarUsuario;
            lblUsuario.Visible = puedeSeleccionarUsuario;

            comboBoxSeleccionarUsuarios.SelectedIndexChanged -= comboBoxSeleccionarUsuarios_SelectedIndexChanged;

            if (puedeSeleccionarUsuario)
            {
                List<Usuario> usuarios = usuarioBL.Listar();

                if (usuarios == null)
                {
                    usuarios = new List<Usuario>();
                }

                comboBoxSeleccionarUsuarios.DisplayMember = "NombreUsuario";
                comboBoxSeleccionarUsuarios.ValueMember = "Id";
                comboBoxSeleccionarUsuarios.DataSource = usuarios;

                int usuarioSesionId = SesionActual.Instance.UsuarioId;

                if (usuarioSesionId > 0)
                {
                    comboBoxSeleccionarUsuarios.SelectedValue = usuarioSesionId;
                }

                comboBoxSeleccionarUsuarios.SelectedIndexChanged += comboBoxSeleccionarUsuarios_SelectedIndexChanged;
            }
            else
            {
                comboBoxSeleccionarUsuarios.DataSource = null;
            }
        }

        private int ResolverUsuario()
        {
            int usuarioSesionId = SesionActual.Instance.UsuarioId;

            if (PuedeSeleccionarUsuarios())
            {
                if (comboBoxSeleccionarUsuarios.SelectedValue is int usuarioSeleccionadoId && usuarioSeleccionadoId > 0)
                {
                    return usuarioSeleccionadoId;
                }
            }

            return usuarioSesionId;
        }

        private string ObtenerNombreUsuarioActual()
        {
            if (PuedeSeleccionarUsuarios() && comboBoxSeleccionarUsuarios.SelectedItem != null)
            {
                Usuario usuarioSeleccionado = comboBoxSeleccionarUsuarios.SelectedItem as Usuario;

                if (usuarioSeleccionado != null)
                {
                    return usuarioSeleccionado.NombreUsuario;
                }
            }

            if (SesionActual.Instance != null)
            {
                return SesionActual.Instance.NombreUsuario;
            }

            return "-";
        }

        private void CargarDatos()
        {
            try
            {
                int usuarioId = ResolverUsuario();

                if (usuarioId <= 0)
                {
                    return;
                }

                CargarResumen(usuarioId);
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void CargarResumen(int usuarioId)
        {
            int minutosJornada = tiempoDisponibleBL.CalcularMinutosJornadaSemanal(usuarioId);
            int minutosBloqueados = tiempoDisponibleBL.CalcularMinutosBloqueadosSemanales(usuarioId);
            int minutosDisponibles = tiempoDisponibleBL.CalcularMinutosDisponiblesSemanales(usuarioId);

            lblResumen.Text = Trad("TiempoDisponible_Label_JornadaSemanal", "Jornada semanal") + ": " + FormatearMinutos(minutosJornada) + " | " + Trad("TiempoDisponible_Label_Bloqueado", "Bloqueado") + ": " + FormatearMinutos(minutosBloqueados) + " | " + Trad("TiempoDisponible_Label_Disponible", "Disponible") + ": " + FormatearMinutos(minutosDisponibles);
        }

        private void btnConfigurarJornada_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioId = ResolverUsuario();

                using (ConfigurarJornadaLaboral formulario = new ConfigurarJornadaLaboral(usuarioId))
                {
                    formulario.StartPosition = FormStartPosition.CenterParent;
                    formulario.ShowDialog(this);
                }

                CargarDatos();
                LimpiarCalendario();
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnConfigurarBloques_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioId = ResolverUsuario();

                using (ConfigurarBloquesTiempo formulario = new ConfigurarBloquesTiempo(usuarioId))
                {
                    formulario.StartPosition = FormStartPosition.CenterParent;
                    formulario.ShowDialog(this);
                }

                CargarDatos();
                LimpiarCalendario();
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnGenerarAgenda_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioId = ResolverUsuario();

                Planificacion planificacion = planificacionBL.ObtenerUltimaAprobadaPorUsuario(usuarioId);

                if (planificacion == null)
                {
                    agendaGeneradaActual = null;

                    MostrarError(Trad("TiempoDisponible_Msg_SinPlanificacion", "No existe una planificación aprobada para generar la agenda laboral."));

                    return;
                }

                List<BloqueCalendario> calendario = tiempoDisponibleBL.GenerarAgendaLaboral(usuarioId, planificacion);

                agendaGeneradaActual = calendario;

                DibujarCalendario(calendario);
                MostrarOk(Trad("TiempoDisponible_Msg_AgendaOk", "Agenda laboral generada correctamente."));
            }
            catch (Exception exception)
            {
                agendaGeneradaActual = null;

                MostrarError(exception.Message);
            }
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (agendaGeneradaActual == null || agendaGeneradaActual.Count == 0)
                {
                    MostrarError(Trad("TiempoDisponible_Msg_GenerarAgendaPrimero", "Primero debe generar una agenda laboral."));

                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = Trad("TiempoDisponible_Dlg_ExportarPdfTitulo", "Exportar agenda laboral a PDF");
                    saveFileDialog.Filter = Trad("TiempoDisponible_Filtro_Pdf", "Archivo PDF (*.pdf)|*.pdf");
                    saveFileDialog.FileName = "AgendaLaboral_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";

                    if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    AgendaLaboralPdfExporter exporter = new AgendaLaboralPdfExporter();

                    exporter.Exportar(saveFileDialog.FileName, ObtenerNombreUsuarioActual(), lblResumen.Text, agendaGeneradaActual);

                    MostrarOk(Trad("TiempoDisponible_Msg_PdfExportadoOk", "Agenda laboral exportada correctamente."));
                }
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void LimpiarCalendario()
        {
            agendaGeneradaActual = null;

            panelCalendario.Controls.Clear();

            lblDetalleTitulo.Text = Trad("TiempoDisponible_Label_Detalle", "Detalle");
            lblDetalleTexto.Text = Trad("TiempoDisponible_Label_SeleccioneBloque", "Seleccione un bloque del calendario.");
        }

        private void DibujarCalendario(List<BloqueCalendario> calendario)
        {
            panelCalendario.SuspendLayout();
            panelCalendario.Controls.Clear();

            if (calendario == null || calendario.Count == 0)
            {
                panelCalendario.ResumeLayout();

                MostrarError(Trad("TiempoDisponible_Msg_NoHayBloques", "No hay bloques para mostrar."));

                return;
            }

            List<DateTime> fechas = ObtenerFechasCalendario(calendario);
            List<BloqueCalendario> bloquesJornada = ObtenerBloquesJornada(calendario);
            List<BloqueCalendario> bloquesVisibles = ObtenerBloquesVisibles(calendario);
            List<BloqueCalendario> tareasSinUbicar = ObtenerTareasSinUbicar(calendario);

            if (fechas.Count == 0)
            {
                panelCalendario.ResumeLayout();

                MostrarError(Trad("TiempoDisponible_Msg_NoHayFechas", "No hay fechas para mostrar en el calendario."));

                return;
            }

            TimeSpan horaInicioCalendario = ObtenerHoraInicioCalendario(calendario);
            TimeSpan horaFinCalendario = ObtenerHoraFinCalendario(calendario);

            int minutosTotales = Convert.ToInt32((horaFinCalendario - horaInicioCalendario).TotalMinutes);

            if (minutosTotales <= 0)
            {
                minutosTotales = 480;
            }

            int anchoTotal = MargenIzquierdoCalendario + (fechas.Count * AnchoColumnaDia) + 30;
            int altoTotal = MargenSuperiorCalendario + (minutosTotales * AltoPorMinuto) + 160;

            panelCalendario.AutoScrollMinSize = new Size(anchoTotal, altoTotal);

            DibujarFondosJornada(bloquesJornada, fechas, horaInicioCalendario);
            DibujarEncabezados(fechas);
            DibujarLineasHoras(horaInicioCalendario, horaFinCalendario, fechas.Count);

            foreach (BloqueCalendario bloque in bloquesVisibles)
            {
                DibujarBloque(bloque, fechas, horaInicioCalendario);
            }

            DibujarTareasSinUbicar(tareasSinUbicar, fechas.Count, horaInicioCalendario, horaFinCalendario);

            panelCalendario.ResumeLayout();
        }

        private List<DateTime> ObtenerFechasCalendario(List<BloqueCalendario> calendario)
        {
            List<DateTime> fechas = new List<DateTime>();

            foreach (BloqueCalendario bloque in calendario)
            {
                if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
                {
                    continue;
                }

                DateTime fecha = bloque.Fecha.Date;

                if (!fechas.Contains(fecha))
                {
                    fechas.Add(fecha);
                }
            }

            fechas.Sort();

            return fechas;
        }

        private List<BloqueCalendario> ObtenerBloquesJornada(List<BloqueCalendario> calendario)
        {
            List<BloqueCalendario> bloques = new List<BloqueCalendario>();

            foreach (BloqueCalendario bloque in calendario)
            {
                if (bloque.TipoBloque == TipoBloqueCalendario.Jornada)
                {
                    bloques.Add(bloque);
                }
            }

            return bloques;
        }

        private List<BloqueCalendario> ObtenerBloquesVisibles(List<BloqueCalendario> calendario)
        {
            List<BloqueCalendario> bloques = new List<BloqueCalendario>();

            foreach (BloqueCalendario bloque in calendario)
            {
                if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
                {
                    continue;
                }

                if (bloque.TipoBloque == TipoBloqueCalendario.Jornada)
                {
                    continue;
                }

                bloques.Add(bloque);
            }

            return bloques;
        }

        private List<BloqueCalendario> ObtenerTareasSinUbicar(List<BloqueCalendario> calendario)
        {
            List<BloqueCalendario> bloques = new List<BloqueCalendario>();

            foreach (BloqueCalendario bloque in calendario)
            {
                if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
                {
                    bloques.Add(bloque);
                }
            }

            return bloques;
        }

        private TimeSpan ObtenerHoraInicioCalendario(List<BloqueCalendario> bloques)
        {
            TimeSpan horaInicio = new TimeSpan(23, 59, 0);

            foreach (BloqueCalendario bloque in bloques)
            {
                if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
                {
                    continue;
                }

                if (bloque.HoraInicio < horaInicio)
                {
                    horaInicio = bloque.HoraInicio;
                }
            }

            int hora = horaInicio.Hours;

            return new TimeSpan(hora, 0, 0);
        }

        private TimeSpan ObtenerHoraFinCalendario(List<BloqueCalendario> bloques)
        {
            TimeSpan horaFin = TimeSpan.Zero;

            foreach (BloqueCalendario bloque in bloques)
            {
                if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
                {
                    continue;
                }

                if (bloque.HoraFin > horaFin)
                {
                    horaFin = bloque.HoraFin;
                }
            }

            int hora = horaFin.Hours;

            if (horaFin.Minutes > 0)
            {
                hora++;
            }

            if (hora > 23)
            {
                hora = 23;
            }

            return new TimeSpan(hora, 0, 0);
        }

        private void DibujarFondosJornada(List<BloqueCalendario> jornadas, List<DateTime> fechas, TimeSpan horaInicioCalendario)
        {
            foreach (BloqueCalendario jornada in jornadas)
            {
                int indiceFecha = fechas.IndexOf(jornada.Fecha.Date);

                if (indiceFecha < 0)
                {
                    continue;
                }

                int minutosDesdeInicio = Convert.ToInt32((jornada.HoraInicio - horaInicioCalendario).TotalMinutes);
                int duracionMinutos = jornada.DuracionMinutos;

                if (duracionMinutos <= 0)
                {
                    continue;
                }

                int x = MargenIzquierdoCalendario + (indiceFecha * AnchoColumnaDia) + 4;
                int y = MargenSuperiorCalendario + (minutosDesdeInicio * AltoPorMinuto);
                int alto = duracionMinutos * AltoPorMinuto;

                Panel fondoJornada = new Panel() { Left = x, Top = y, Width = AnchoColumnaDia - 9, Height = alto, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle };

                panelCalendario.Controls.Add(fondoJornada);

                fondoJornada.SendToBack();
            }
        }

        private void DibujarEncabezados(List<DateTime> fechas)
        {
            for (int i = 0; i < fechas.Count; i++)
            {
                DateTime fecha = fechas[i];

                Label lblDia = new Label() { Left = MargenIzquierdoCalendario + (i * AnchoColumnaDia), Top = 10, Width = AnchoColumnaDia - 5, Height = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font(Font, FontStyle.Bold), BorderStyle = BorderStyle.FixedSingle, BackColor = Color.Gainsboro, Text = ObtenerNombreDia(ObtenerDiaSemanaLaboral(fecha)) + Environment.NewLine + fecha.ToString("dd/MM") };

                panelCalendario.Controls.Add(lblDia);

                lblDia.BringToFront();
            }
        }

        private void DibujarLineasHoras(TimeSpan horaInicioCalendario, TimeSpan horaFinCalendario, int cantidadDias)
        {
            int minutosTotales = Convert.ToInt32((horaFinCalendario - horaInicioCalendario).TotalMinutes);

            for (int minuto = 0; minuto <= minutosTotales; minuto += 60)
            {
                TimeSpan hora = horaInicioCalendario.Add(TimeSpan.FromMinutes(minuto));
                int y = MargenSuperiorCalendario + (minuto * AltoPorMinuto);

                Label lblHora = new Label() { Left = 5, Top = y - 8, Width = 60, Height = 18, TextAlign = ContentAlignment.MiddleRight, Text = FormatearHora(hora) };

                Panel linea = new Panel() { Left = MargenIzquierdoCalendario, Top = y, Width = cantidadDias * AnchoColumnaDia, Height = 1, BackColor = Color.LightGray };

                panelCalendario.Controls.Add(lblHora);
                panelCalendario.Controls.Add(linea);

                lblHora.BringToFront();
                linea.BringToFront();
            }

            for (int i = 0; i <= cantidadDias; i++)
            {
                Panel lineaVertical = new Panel() { Left = MargenIzquierdoCalendario + (i * AnchoColumnaDia), Top = MargenSuperiorCalendario, Width = 1, Height = minutosTotales * AltoPorMinuto, BackColor = Color.LightGray };

                panelCalendario.Controls.Add(lineaVertical);

                lineaVertical.BringToFront();
            }
        }

        private void DibujarBloque(BloqueCalendario bloque, List<DateTime> fechas, TimeSpan horaInicioCalendario)
        {
            int indiceFecha = fechas.IndexOf(bloque.Fecha.Date);

            if (indiceFecha < 0)
            {
                return;
            }

            int minutosDesdeInicio = Convert.ToInt32((bloque.HoraInicio - horaInicioCalendario).TotalMinutes);
            int duracionMinutos = bloque.DuracionMinutos;

            if (duracionMinutos <= 0)
            {
                return;
            }

            int x = MargenIzquierdoCalendario + (indiceFecha * AnchoColumnaDia) + 6;
            int y = MargenSuperiorCalendario + (minutosDesdeInicio * AltoPorMinuto);
            int alto = duracionMinutos * AltoPorMinuto;

            if (alto < AltoMinimoBloque)
            {
                alto = AltoMinimoBloque;
            }

            Panel panelBloque = new Panel() { Left = x, Top = y, Width = AnchoColumnaDia - 13, Height = alto, BorderStyle = BorderStyle.FixedSingle, BackColor = ObtenerColorBloque(bloque.TipoBloque), Cursor = Cursors.Hand };

            Label lblTexto = new Label() { Dock = DockStyle.Fill, Padding = new Padding(4, 2, 4, 2), TextAlign = ContentAlignment.TopLeft, AutoEllipsis = true, Font = new Font(Font.FontFamily, 8) };

            if (alto < 38)
            {
                lblTexto.Text = FormatearHora(bloque.HoraInicio) + " - " + bloque.Titulo;
            }
            else
            {
                lblTexto.Text = bloque.Titulo + Environment.NewLine + FormatearHora(bloque.HoraInicio) + " - " + FormatearHora(bloque.HoraFin);
            }

            panelBloque.Click += delegate
            {
                MostrarDetalleBloque(bloque);
            };

            lblTexto.Click += delegate
            {
                MostrarDetalleBloque(bloque);
            };

            panelBloque.Controls.Add(lblTexto);
            panelCalendario.Controls.Add(panelBloque);

            panelBloque.BringToFront();
        }

        private Color ObtenerColorBloque(TipoBloqueCalendario tipoBloque)
        {
            if (tipoBloque == TipoBloqueCalendario.Jornada)
            {
                return Color.WhiteSmoke;
            }

            if (tipoBloque == TipoBloqueCalendario.BloqueFijo)
            {
                return Color.Khaki;
            }

            if (tipoBloque == TipoBloqueCalendario.TareaPlanificada)
            {
                return Color.LightSkyBlue;
            }

            return Color.LightCoral;
        }

        private void DibujarTareasSinUbicar(List<BloqueCalendario> tareasSinUbicar, int cantidadDias, TimeSpan horaInicioCalendario, TimeSpan horaFinCalendario)
        {
            if (tareasSinUbicar == null || tareasSinUbicar.Count == 0)
            {
                return;
            }

            int minutosTotales = Convert.ToInt32((horaFinCalendario - horaInicioCalendario).TotalMinutes);
            int top = MargenSuperiorCalendario + (minutosTotales * AltoPorMinuto) + 30;

            Label lblTituloSinUbicar = new Label() { Left = MargenIzquierdoCalendario, Top = top, Width = cantidadDias * AnchoColumnaDia, Height = 22, Font = new Font(Font, FontStyle.Bold), Text = Trad("TiempoDisponible_Label_TareasSinUbicar", "Tareas sin ubicar") };

            panelCalendario.Controls.Add(lblTituloSinUbicar);

            int y = top + 28;

            foreach (BloqueCalendario bloque in tareasSinUbicar)
            {
                Label lblTarea = new Label() { Left = MargenIzquierdoCalendario, Top = y, Width = cantidadDias * AnchoColumnaDia, Height = 24, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.MistyRose, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(4, 0, 0, 0), Text = bloque.Titulo };

                panelCalendario.Controls.Add(lblTarea);

                y += 28;
            }
        }

        private void MostrarDetalleBloque(BloqueCalendario bloque)
        {
            lblDetalleTitulo.Text = bloque.Titulo;

            string fechaTexto = string.Empty;
            string horarioTexto = string.Empty;
            string diaTexto = string.Empty;

            if (bloque.Fecha == DateTime.MinValue)
            {
                fechaTexto = "-";
            }
            else
            {
                fechaTexto = bloque.Fecha.ToString("dd/MM/yyyy");
            }

            if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
            {
                horarioTexto = "-";
            }
            else
            {
                horarioTexto = FormatearHora(bloque.HoraInicio) + " - " + FormatearHora(bloque.HoraFin);
            }

            if (bloque.DiaSemana == 0)
            {
                diaTexto = "-";
            }
            else
            {
                diaTexto = ObtenerNombreDia(bloque.DiaSemana);
            }

            string texto = Trad("TiempoDisponible_Detalle_Fecha", "Fecha") + ": " + fechaTexto + Environment.NewLine +Trad("TiempoDisponible_Detalle_Dia", "Día") + ": " + diaTexto + Environment.NewLine +Trad("TiempoDisponible_Detalle_Horario", "Horario") + ": " + horarioTexto + Environment.NewLine + Trad("TiempoDisponible_Detalle_Tipo", "Tipo") + ": " + bloque.TipoBloque;

            if (bloque.ScorePrioridad.HasValue)
            {
                texto += Environment.NewLine + Trad("TiempoDisponible_Detalle_Score", "Score") + ": " + bloque.ScorePrioridad.Value.ToString("0.##");
            }

            lblDetalleTexto.Text = texto;
        }

        private int ObtenerDiaSemanaLaboral(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Monday)
            {
                return 1;
            }

            if (fecha.DayOfWeek == DayOfWeek.Tuesday)
            {
                return 2;
            }

            if (fecha.DayOfWeek == DayOfWeek.Wednesday)
            {
                return 3;
            }

            if (fecha.DayOfWeek == DayOfWeek.Thursday)
            {
                return 4;
            }

            if (fecha.DayOfWeek == DayOfWeek.Friday)
            {
                return 5;
            }

            if (fecha.DayOfWeek == DayOfWeek.Saturday)
            {
                return 6;
            }

            return 7;
        }

        private string ObtenerNombreDia(int diaSemana)
        {
            if (diaSemana == 1)
            {
                return Trad("Dia_Lunes", "Lunes");
            }

            if (diaSemana == 2)
            {
                return Trad("Dia_Martes", "Martes");
            }

            if (diaSemana == 3)
            {
                return Trad("Dia_Miercoles", "Miércoles");
            }

            if (diaSemana == 4)
            {
                return Trad("Dia_Jueves", "Jueves");
            }

            if (diaSemana == 5)
            {
                return Trad("Dia_Viernes", "Viernes");
            }

            if (diaSemana == 6)
            {
                return Trad("Dia_Sabado", "Sábado");
            }

            if (diaSemana == 7)
            {
                return Trad("Dia_Domingo", "Domingo");
            }

            return "-";
        }

        private string FormatearHora(TimeSpan hora)
        {
            return hora.ToString(@"hh\:mm");
        }

        private string FormatearMinutos(int minutos)
        {
            int horas = minutos / 60;
            int minutosRestantes = minutos % 60;

            return horas.ToString() + " h " + minutosRestantes.ToString() + " min";
        }

        private void MostrarError(string mensaje)
        {
            lblMensaje.ForeColor = Color.Maroon;
            lblMensaje.Text = mensaje;
        }

        private void MostrarOk(string mensaje)
        {
            lblMensaje.ForeColor = Color.DarkGreen;
            lblMensaje.Text = mensaje;
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

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("TiempoDisponible_Titulo", "Gestionar tiempo disponible");

            lblTitulo.Text = Trad("TiempoDisponible_Label_Titulo", "Gestión de tiempo disponible");
            lblUsuario.Text = Trad("TiempoDisponible_Label_Usuario", "Usuario");
            lblAgenda.Text = Trad("TiempoDisponible_Label_Agenda", "Agenda laboral");

            btnConfigurarJornada.Text = Trad("TiempoDisponible_Boton_ConfigurarJornada", "Configurar jornada");
            btnConfigurarBloques.Text = Trad("TiempoDisponible_Boton_ConfigurarBloques", "Configurar bloques");
            btnGenerarAgenda.Text = Trad("TiempoDisponible_Boton_GenerarAgenda", "Generar agenda");
            btnExportarPdf.Text = Trad("TiempoDisponible_Boton_ExportarPdf", "Exportar PDF");
            btnGuardarCopiaAgenda.Text = Trad("TiempoDisponible_Boton_GuardarCopiaAgenda", "Guardar copia");
            btnCargarCopiaAgenda.Text = Trad("TiempoDisponible_Boton_CargarCopiaAgenda", "Cargar copia");
            btnCerrar.Text = Trad("TiempoDisponible_Boton_Cerrar", "Cerrar");

            lblDetalleTitulo.Text = Trad("TiempoDisponible_Label_Detalle", "Detalle");
            lblDetalleTexto.Text = Trad("TiempoDisponible_Label_SeleccioneBloque", "Seleccione un bloque del calendario.");

            CargarDatos();
        }

        private void comboBoxSeleccionarUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDatos();
            LimpiarCalendario();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GestionarTiempoDisponible_Load(object sender, EventArgs e)
        {

        }

        private void btnGuardarCopiaAgenda_Click(object sender, EventArgs e)
        {
            try
            {
                if (agendaGeneradaActual == null || agendaGeneradaActual.Count == 0)
                {
                    MostrarError(Trad("TiempoDisponible_Msg_GenerarAgendaPrimero", "Primero debe generar una agenda laboral."));

                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = Trad("TiempoDisponible_Dlg_GuardarCopiaTitulo", "Guardar copia de agenda laboral");
                    saveFileDialog.Filter = Trad("TiempoDisponible_Filtro_Json", "Archivo JSON (*.json)|*.json");
                    saveFileDialog.FileName = "AgendaLaboral_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".json";

                    if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    AgendaLaboralSerializada copia = new AgendaLaboralSerializada() { UsuarioId = ResolverUsuario(), NombreUsuario = ObtenerNombreUsuarioActual(), FechaGeneracion = DateTime.Now, Resumen = lblResumen.Text, Bloques = agendaGeneradaActual };

                    AgendaLaboralJsonSerializer serializer = new AgendaLaboralJsonSerializer();

                    serializer.Guardar(saveFileDialog.FileName, copia);

                    MostrarOk(Trad("TiempoDisponible_Msg_CopiaGuardadaOk", "Copia de agenda guardada correctamente."));
                }
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnCargarCopiaAgenda_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = Trad("TiempoDisponible_Dlg_CargarCopiaTitulo", "Cargar copia de agenda laboral");
                    openFileDialog.Filter = Trad("TiempoDisponible_Filtro_Json", "Archivo JSON (*.json)|*.json");

                    if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    AgendaLaboralJsonSerializer serializer = new AgendaLaboralJsonSerializer();

                    AgendaLaboralSerializada copia = serializer.Cargar(openFileDialog.FileName);

                    if (copia == null || copia.Bloques == null || copia.Bloques.Count == 0)
                    {
                        MostrarError(Trad("TiempoDisponible_Msg_ArchivoAgendaInvalido", "El archivo seleccionado no corresponde a una copia válida de agenda laboral."));

                        return;
                    }

                    int usuarioActualId = ResolverUsuario();

                    if (copia.UsuarioId != usuarioActualId)
                    {
                        MostrarError(Trad("TiempoDisponible_Msg_CopiaOtroUsuario", "La copia seleccionada pertenece a otro usuario. Seleccione el usuario correspondiente antes de cargarla."));

                        return;
                    }

                    agendaGeneradaActual = copia.Bloques;

                    DibujarCalendario(agendaGeneradaActual);

                    MostrarOk(Trad("TiempoDisponible_Msg_CopiaCargadaOk", "Copia de agenda cargada correctamente."));
                }
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }
    }
}