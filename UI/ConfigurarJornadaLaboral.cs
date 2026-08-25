using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class ConfigurarJornadaLaboral : Form, IIdiomaObserver
    {
        private readonly TiempoDisponibleBL tiempoDisponibleBL;
        private readonly int usuarioId;

        public ConfigurarJornadaLaboral(int usuarioIdParametro)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            tiempoDisponibleBL = new TiempoDisponibleBL();
            usuarioId = usuarioIdParametro;

            ConfigurarControles();
            ConfigurarGrid();
            CargarJornadas();
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
            comboBoxDiaSemana.DropDownStyle = ComboBoxStyle.DropDownList;

            CargarDiasSemana();

            dateTimePickerHoraInicio.Format = DateTimePickerFormat.Custom;
            dateTimePickerHoraInicio.CustomFormat = "HH:mm";
            dateTimePickerHoraInicio.ShowUpDown = true;

            dateTimePickerHoraFin.Format = DateTimePickerFormat.Custom;
            dateTimePickerHoraFin.CustomFormat = "HH:mm";
            dateTimePickerHoraFin.ShowUpDown = true;

            dateTimePickerHoraInicio.Value = DateTime.Today.AddHours(9);
            dateTimePickerHoraFin.Value = DateTime.Today.AddHours(16);

            lblMensaje.Text = string.Empty;
        }

        private void CargarDiasSemana()
        {
            int? diaSeleccionado = null;

            if (comboBoxDiaSemana.SelectedValue is int)
            {
                diaSeleccionado = (int)comboBoxDiaSemana.SelectedValue;
            }

            List<KeyValuePair<int, string>> dias = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1, Trad("Dia_Lunes", "Lunes")),
                new KeyValuePair<int, string>(2, Trad("Dia_Martes", "Martes")),
                new KeyValuePair<int, string>(3, Trad("Dia_Miercoles", "Miércoles")),
                new KeyValuePair<int, string>(4, Trad("Dia_Jueves", "Jueves")),
                new KeyValuePair<int, string>(5, Trad("Dia_Viernes", "Viernes")),
                new KeyValuePair<int, string>(6, Trad("Dia_Sabado", "Sábado")),
                new KeyValuePair<int, string>(7, Trad("Dia_Domingo", "Domingo"))
            };

            comboBoxDiaSemana.DisplayMember = "Value";
            comboBoxDiaSemana.ValueMember = "Key";
            comboBoxDiaSemana.DataSource = dias;

            if (diaSeleccionado.HasValue)
            {
                comboBoxDiaSemana.SelectedValue = diaSeleccionado.Value;
            }
        }

        private void ConfigurarGrid()
        {
            dataGridViewJornadas.AutoGenerateColumns = false;
            dataGridViewJornadas.AllowUserToAddRows = false;
            dataGridViewJornadas.AllowUserToDeleteRows = false;
            dataGridViewJornadas.ReadOnly = true;
            dataGridViewJornadas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewJornadas.MultiSelect = false;
            dataGridViewJornadas.Columns.Clear();

            dataGridViewJornadas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "JornadaLaboralUsuarioId", Name = "colJornadaId", Visible = false });
            dataGridViewJornadas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Dia", Name = "colDia", HeaderText = Trad("ConfigJornada_Col_Dia", "Día"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewJornadas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoraInicio", Name = "colHoraInicio", HeaderText = Trad("ConfigJornada_Col_Inicio", "Inicio") });
            dataGridViewJornadas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoraFin", Name = "colHoraFin", HeaderText = Trad("ConfigJornada_Col_Fin", "Fin") });
        }

        private void CargarJornadas()
        {
            try
            {
                ValidarUsuario();

                List<JornadaLaboralUsuario> jornadas = tiempoDisponibleBL.ListarJornadasPorUsuario(usuarioId);
                List<object> filas = new List<object>();

                foreach (JornadaLaboralUsuario jornada in jornadas)
                {
                    filas.Add(new
                    {
                        JornadaLaboralUsuarioId = jornada.JornadaLaboralUsuarioId,
                        Dia = ObtenerNombreDia(jornada.DiaSemana),
                        HoraInicio = FormatearHora(jornada.HoraInicio),
                        HoraFin = FormatearHora(jornada.HoraFin)
                    });
                }

                dataGridViewJornadas.DataSource = null;
                dataGridViewJornadas.DataSource = filas;
                dataGridViewJornadas.ClearSelection();
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarUsuario();

                JornadaLaboralUsuario jornada = new JornadaLaboralUsuario
                {
                    UsuarioId = usuarioId,
                    DiaSemana = ObtenerDiaSeleccionado(),
                    HoraInicio = dateTimePickerHoraInicio.Value.TimeOfDay,
                    HoraFin = dateTimePickerHoraFin.Value.TimeOfDay
                };

                tiempoDisponibleBL.CrearJornadaLaboral(jornada);

                MostrarOk(Trad("ConfigJornada_Msg_GuardadaOk", "Jornada laboral guardada correctamente."));
                CargarJornadas();
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarUsuario();

                int? jornadaId = JornadaSeleccionadaId();

                if (!jornadaId.HasValue)
                {
                    MostrarError(Trad("ConfigJornada_Msg_SeleccioneJornada", "Seleccione una jornada laboral."));
                    return;
                }

                JornadaLaboralUsuario jornada = tiempoDisponibleBL.ObtenerJornadaLaboral(jornadaId.Value);

                if (jornada == null)
                {
                    MostrarError(Trad("ConfigJornada_Msg_NoEncontrada", "No se encontró la jornada laboral."));
                    return;
                }

                jornada.UsuarioId = usuarioId;
                jornada.DiaSemana = ObtenerDiaSeleccionado();
                jornada.HoraInicio = dateTimePickerHoraInicio.Value.TimeOfDay;
                jornada.HoraFin = dateTimePickerHoraFin.Value.TimeOfDay;

                tiempoDisponibleBL.ActualizarJornadaLaboral(jornada);

                MostrarOk(Trad("ConfigJornada_Msg_ActualizadaOk", "Jornada laboral actualizada correctamente."));
                CargarJornadas();
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int? jornadaId = JornadaSeleccionadaId();

                if (!jornadaId.HasValue)
                {
                    MostrarError(Trad("ConfigJornada_Msg_SeleccioneJornada", "Seleccione una jornada laboral."));
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    Trad("ConfigJornada_Msg_ConfirmarEliminar", "¿Desea eliminar la jornada seleccionada?"),
                    Trad("ConfigJornada_Msg_Titulo", "Jornada laboral"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                tiempoDisponibleBL.EliminarJornadaLaboral(jornadaId.Value);

                MostrarOk(Trad("ConfigJornada_Msg_EliminadaOk", "Jornada laboral eliminada correctamente."));
                CargarJornadas();
            }
            catch (Exception exception)
            {
                MostrarError(exception.Message);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void dataGridViewJornadas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarSeleccionEnFormulario();
        }

        private void CargarSeleccionEnFormulario()
        {
            try
            {
                int? jornadaId = JornadaSeleccionadaId();

                if (!jornadaId.HasValue)
                {
                    return;
                }

                JornadaLaboralUsuario jornada = tiempoDisponibleBL.ObtenerJornadaLaboral(jornadaId.Value);

                if (jornada == null)
                {
                    return;
                }

                comboBoxDiaSemana.SelectedValue = jornada.DiaSemana;
                dateTimePickerHoraInicio.Value = DateTime.Today.Add(jornada.HoraInicio);
                dateTimePickerHoraFin.Value = DateTime.Today.Add(jornada.HoraFin);
            }
            catch
            {
            }
        }

        private int? JornadaSeleccionadaId()
        {
            if (dataGridViewJornadas.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell celda = dataGridViewJornadas.CurrentRow.Cells["colJornadaId"];

            if (celda == null || celda.Value == null)
            {
                return null;
            }

            int id;

            if (!int.TryParse(celda.Value.ToString(), out id))
            {
                return null;
            }

            return id;
        }

        private int ObtenerDiaSeleccionado()
        {
            if (comboBoxDiaSemana.SelectedValue is int dia)
            {
                return dia;
            }

            return 1;
        }

        private void ValidarUsuario()
        {
            if (usuarioId <= 0)
            {
                throw new InvalidOperationException(Trad("ConfigJornada_Msg_SinUsuario", "No hay usuario seleccionado."));
            }
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

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            Text = Trad("ConfigJornada_Titulo", "Configurar jornada laboral");
            lblTitulo.Text = Trad("ConfigJornada_Label_Titulo", "Configurar jornada laboral");
            lblDia.Text = Trad("ConfigJornada_Label_Dia", "Día");
            lblHoraInicio.Text = Trad("ConfigJornada_Label_Inicio", "Inicio");
            lblHoraFin.Text = Trad("ConfigJornada_Label_Fin", "Fin");

            btnGuardar.Text = Trad("ConfigJornada_Boton_Guardar", "Guardar");
            btnEditar.Text = Trad("ConfigJornada_Boton_Editar", "Editar");
            btnEliminar.Text = Trad("ConfigJornada_Boton_Eliminar", "Eliminar");
            btnCerrar.Text = Trad("ConfigJornada_Boton_Cerrar", "Cerrar");

            CargarDiasSemana();

            SetHeader(dataGridViewJornadas, "colDia", "ConfigJornada_Col_Dia", "Día");
            SetHeader(dataGridViewJornadas, "colHoraInicio", "ConfigJornada_Col_Inicio", "Inicio");
            SetHeader(dataGridViewJornadas, "colHoraFin", "ConfigJornada_Col_Fin", "Fin");

            CargarJornadas();
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

        private void ConfigurarJornadaLaboral_Load(object sender, EventArgs e)
        {

        }
    }
}