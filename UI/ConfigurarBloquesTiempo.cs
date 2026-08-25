using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class ConfigurarBloquesTiempo : Form, IIdiomaObserver
    {
        private readonly TiempoDisponibleBL tiempoDisponibleBL;
        private readonly int usuarioId;

        public ConfigurarBloquesTiempo(int usuarioIdParametro)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            tiempoDisponibleBL = new TiempoDisponibleBL();
            usuarioId = usuarioIdParametro;

            ConfigurarControles();
            ConfigurarGrid();
            CargarBloques();
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
            comboBoxTipoBloque.DropDownStyle = ComboBoxStyle.DropDownList;

            CargarDiasSemana();
            CargarTiposBloque();

            dateTimePickerHoraInicio.Format = DateTimePickerFormat.Custom;
            dateTimePickerHoraInicio.CustomFormat = "HH:mm";
            dateTimePickerHoraInicio.ShowUpDown = true;

            dateTimePickerHoraFin.Format = DateTimePickerFormat.Custom;
            dateTimePickerHoraFin.CustomFormat = "HH:mm";
            dateTimePickerHoraFin.ShowUpDown = true;

            dateTimePickerHoraInicio.Value = DateTime.Today.AddHours(9);
            dateTimePickerHoraFin.Value = DateTime.Today.AddHours(9).AddMinutes(15);

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

        private void CargarTiposBloque()
        {
            TipoBloqueTiempo? tipoSeleccionado = null;

            if (comboBoxTipoBloque.SelectedValue is TipoBloqueTiempo)
            {
                tipoSeleccionado = (TipoBloqueTiempo)comboBoxTipoBloque.SelectedValue;
            }

            List<KeyValuePair<TipoBloqueTiempo, string>> tipos = new List<KeyValuePair<TipoBloqueTiempo, string>>
            {
                new KeyValuePair<TipoBloqueTiempo, string>(TipoBloqueTiempo.Otro, Trad("TipoBloque_Otro", "Otro")),
                new KeyValuePair<TipoBloqueTiempo, string>(TipoBloqueTiempo.Daily, Trad("TipoBloque_Daily", "Daily")),
                new KeyValuePair<TipoBloqueTiempo, string>(TipoBloqueTiempo.Almuerzo, Trad("TipoBloque_Almuerzo", "Almuerzo")),
                new KeyValuePair<TipoBloqueTiempo, string>(TipoBloqueTiempo.Reunion, Trad("TipoBloque_Reunion", "Reunión")),
                new KeyValuePair<TipoBloqueTiempo, string>(TipoBloqueTiempo.Descanso, Trad("TipoBloque_Descanso", "Descanso")),
                new KeyValuePair<TipoBloqueTiempo, string>(TipoBloqueTiempo.Capacitacion, Trad("TipoBloque_Capacitacion", "Capacitación"))
            };

            comboBoxTipoBloque.DisplayMember = "Value";
            comboBoxTipoBloque.ValueMember = "Key";
            comboBoxTipoBloque.DataSource = tipos;

            if (tipoSeleccionado.HasValue)
            {
                comboBoxTipoBloque.SelectedValue = tipoSeleccionado.Value;
            }
        }

        private void ConfigurarGrid()
        {
            dataGridViewBloques.AutoGenerateColumns = false;
            dataGridViewBloques.AllowUserToAddRows = false;
            dataGridViewBloques.AllowUserToDeleteRows = false;
            dataGridViewBloques.ReadOnly = true;
            dataGridViewBloques.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBloques.MultiSelect = false;
            dataGridViewBloques.Columns.Clear();

            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "BloqueTiempoId", Name = "colBloqueId", Visible = false });
            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Titulo", Name = "colTitulo", HeaderText = Trad("ConfigBloques_Col_Titulo", "Título"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Descripcion", Name = "colDescripcion", HeaderText = Trad("ConfigBloques_Col_Descripcion", "Descripción"), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TipoBloque", Name = "colTipoBloque", HeaderText = Trad("ConfigBloques_Col_Tipo", "Tipo") });
            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Dia", Name = "colDia", HeaderText = Trad("ConfigBloques_Col_Dia", "Día") });
            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoraInicio", Name = "colHoraInicio", HeaderText = Trad("ConfigBloques_Col_Inicio", "Inicio") });
            dataGridViewBloques.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoraFin", Name = "colHoraFin", HeaderText = Trad("ConfigBloques_Col_Fin", "Fin") });
        }

        private void CargarBloques()
        {
            try
            {
                ValidarUsuario();

                List<BloqueTiempo> bloques = tiempoDisponibleBL.ListarBloquesPorUsuario(usuarioId);
                List<object> filas = new List<object>();

                foreach (BloqueTiempo bloque in bloques)
                {
                    filas.Add(new
                    {
                        BloqueTiempoId = bloque.BloqueTiempoId,
                        Titulo = bloque.Titulo,
                        Descripcion = bloque.Descripcion,
                        TipoBloque = ObtenerNombreTipoBloque(bloque.TipoBloque),
                        Dia = ObtenerNombreDia(bloque.DiaSemana),
                        HoraInicio = FormatearHora(bloque.HoraInicio),
                        HoraFin = FormatearHora(bloque.HoraFin)
                    });
                }

                dataGridViewBloques.DataSource = null;
                dataGridViewBloques.DataSource = filas;
                dataGridViewBloques.ClearSelection();
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

                BloqueTiempo bloque = new BloqueTiempo
                {
                    UsuarioId = usuarioId,
                    Titulo = txtTitulo.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                    TipoBloque = ObtenerTipoBloqueSeleccionado(),
                    DiaSemana = ObtenerDiaSeleccionado(),
                    HoraInicio = dateTimePickerHoraInicio.Value.TimeOfDay,
                    HoraFin = dateTimePickerHoraFin.Value.TimeOfDay
                };

                tiempoDisponibleBL.CrearBloqueTiempo(bloque);

                MostrarOk(Trad("ConfigBloques_Msg_GuardadoOk", "Bloque guardado correctamente."));
                LimpiarFormulario();
                CargarBloques();
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

                int? bloqueId = BloqueSeleccionadoId();

                if (!bloqueId.HasValue)
                {
                    MostrarError(Trad("ConfigBloques_Msg_SeleccioneBloque", "Seleccione un bloque."));
                    return;
                }

                BloqueTiempo bloque = tiempoDisponibleBL.ObtenerBloqueTiempo(bloqueId.Value);

                if (bloque == null)
                {
                    MostrarError(Trad("ConfigBloques_Msg_NoEncontrado", "No se encontró el bloque."));
                    return;
                }

                bloque.UsuarioId = usuarioId;
                bloque.Titulo = txtTitulo.Text.Trim();
                bloque.Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim();
                bloque.TipoBloque = ObtenerTipoBloqueSeleccionado();
                bloque.DiaSemana = ObtenerDiaSeleccionado();
                bloque.HoraInicio = dateTimePickerHoraInicio.Value.TimeOfDay;
                bloque.HoraFin = dateTimePickerHoraFin.Value.TimeOfDay;

                tiempoDisponibleBL.ActualizarBloqueTiempo(bloque);

                MostrarOk(Trad("ConfigBloques_Msg_ActualizadoOk", "Bloque actualizado correctamente."));
                LimpiarFormulario();
                CargarBloques();
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
                int? bloqueId = BloqueSeleccionadoId();

                if (!bloqueId.HasValue)
                {
                    MostrarError(Trad("ConfigBloques_Msg_SeleccioneBloque", "Seleccione un bloque."));
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    Trad("ConfigBloques_Msg_ConfirmarEliminar", "¿Desea eliminar el bloque seleccionado?"),
                    Trad("ConfigBloques_Msg_Titulo", "Bloques fijos"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes)
                {
                    return;
                }

                tiempoDisponibleBL.EliminarBloqueTiempo(bloqueId.Value);

                MostrarOk(Trad("ConfigBloques_Msg_EliminadoOk", "Bloque eliminado correctamente."));
                LimpiarFormulario();
                CargarBloques();
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

        private void dataGridViewBloques_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarSeleccionEnFormulario();
        }

        private void CargarSeleccionEnFormulario()
        {
            try
            {
                int? bloqueId = BloqueSeleccionadoId();

                if (!bloqueId.HasValue)
                {
                    return;
                }

                BloqueTiempo bloque = tiempoDisponibleBL.ObtenerBloqueTiempo(bloqueId.Value);

                if (bloque == null)
                {
                    return;
                }

                txtTitulo.Text = bloque.Titulo;
                txtDescripcion.Text = bloque.Descripcion;
                comboBoxTipoBloque.SelectedValue = bloque.TipoBloque;
                comboBoxDiaSemana.SelectedValue = bloque.DiaSemana;
                dateTimePickerHoraInicio.Value = DateTime.Today.Add(bloque.HoraInicio);
                dateTimePickerHoraFin.Value = DateTime.Today.Add(bloque.HoraFin);
            }
            catch
            {
            }
        }

        private int? BloqueSeleccionadoId()
        {
            if (dataGridViewBloques.CurrentRow == null)
            {
                return null;
            }

            DataGridViewCell celda = dataGridViewBloques.CurrentRow.Cells["colBloqueId"];

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

        private TipoBloqueTiempo ObtenerTipoBloqueSeleccionado()
        {
            if (comboBoxTipoBloque.SelectedValue is TipoBloqueTiempo tipo)
            {
                return tipo;
            }

            return TipoBloqueTiempo.Otro;
        }

        private void ValidarUsuario()
        {
            if (usuarioId <= 0)
            {
                throw new InvalidOperationException(Trad("ConfigBloques_Msg_SinUsuario", "No hay usuario seleccionado."));
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

        private string ObtenerNombreTipoBloque(TipoBloqueTiempo tipoBloque)
        {
            if (tipoBloque == TipoBloqueTiempo.Otro)
            {
                return Trad("TipoBloque_Otro", "Otro");
            }

            if (tipoBloque == TipoBloqueTiempo.Daily)
            {
                return Trad("TipoBloque_Daily", "Daily");
            }

            if (tipoBloque == TipoBloqueTiempo.Almuerzo)
            {
                return Trad("TipoBloque_Almuerzo", "Almuerzo");
            }

            if (tipoBloque == TipoBloqueTiempo.Reunion)
            {
                return Trad("TipoBloque_Reunion", "Reunión");
            }

            if (tipoBloque == TipoBloqueTiempo.Descanso)
            {
                return Trad("TipoBloque_Descanso", "Descanso");
            }

            if (tipoBloque == TipoBloqueTiempo.Capacitacion)
            {
                return Trad("TipoBloque_Capacitacion", "Capacitación");
            }

            return tipoBloque.ToString();
        }

        private string FormatearHora(TimeSpan hora)
        {
            return hora.ToString(@"hh\:mm");
        }

        private void LimpiarFormulario()
        {
            txtTitulo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            comboBoxTipoBloque.SelectedIndex = 0;
            comboBoxDiaSemana.SelectedIndex = 0;
            dateTimePickerHoraInicio.Value = DateTime.Today.AddHours(9);
            dateTimePickerHoraFin.Value = DateTime.Today.AddHours(9).AddMinutes(15);
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
            Text = Trad("ConfigBloques_Titulo", "Configurar bloques fijos");
            lblTitulo.Text = Trad("ConfigBloques_Label_Titulo", "Configurar bloques fijos");
            lblBloqueTitulo.Text = Trad("ConfigBloques_Label_BloqueTitulo", "Título");
            lblDescripcion.Text = Trad("ConfigBloques_Label_Descripcion", "Descripción");
            lblTipo.Text = Trad("ConfigBloques_Label_Tipo", "Tipo");
            lblDia.Text = Trad("ConfigBloques_Label_Dia", "Día");
            lblHoraInicio.Text = Trad("ConfigBloques_Label_Inicio", "Inicio");
            lblHoraFin.Text = Trad("ConfigBloques_Label_Fin", "Fin");

            btnGuardar.Text = Trad("ConfigBloques_Boton_Guardar", "Guardar");
            btnEditar.Text = Trad("ConfigBloques_Boton_Editar", "Editar");
            btnEliminar.Text = Trad("ConfigBloques_Boton_Eliminar", "Eliminar");
            btnCerrar.Text = Trad("ConfigBloques_Boton_Cerrar", "Cerrar");

            CargarDiasSemana();
            CargarTiposBloque();

            SetHeader(dataGridViewBloques, "colTitulo", "ConfigBloques_Col_Titulo", "Título");
            SetHeader(dataGridViewBloques, "colDescripcion", "ConfigBloques_Col_Descripcion", "Descripción");
            SetHeader(dataGridViewBloques, "colTipoBloque", "ConfigBloques_Col_Tipo", "Tipo");
            SetHeader(dataGridViewBloques, "colDia", "ConfigBloques_Col_Dia", "Día");
            SetHeader(dataGridViewBloques, "colHoraInicio", "ConfigBloques_Col_Inicio", "Inicio");
            SetHeader(dataGridViewBloques, "colHoraFin", "ConfigBloques_Col_Fin", "Fin");

            CargarBloques();
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

        private void ConfigurarBloquesTiempo_Load(object sender, EventArgs e)
        {

        }
    }
}