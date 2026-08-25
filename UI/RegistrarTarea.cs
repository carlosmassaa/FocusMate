using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BL;
using Servicioss;

namespace UI
{
    public partial class RegistrarTarea : Form, IIdiomaObserver
    {
        public enum ModoFormulario
        {
            Crear,
            Editar,
            Eliminar
        }

        private readonly TareaBL _tareaBl = new TareaBL();
        private Tarea _tareaActual;
        private ModoFormulario _modoFormulario = ModoFormulario.Crear;

        private string _lblMensajeClaveActual = null;
        private object[] _lblMensajeArgsActual = null;

        public RegistrarTarea()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            ConfigurarControles();
            CargarCombosTraducidos();
        }

        public RegistrarTarea(Tarea tarea, ModoFormulario modoFormulario) : this()
        {
            _tareaActual = tarea ?? throw new ArgumentNullException(nameof(tarea));
            _modoFormulario = modoFormulario;
            CargarTareaEnUi();
            AplicarModo();
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
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void ConfigurarControles()
        {
            numericUpDownDuracionEstimada.Minimum = 1;
            numericUpDownDuracionEstimada.Maximum = 14400;
            numericUpDownDuracionEstimada.Value = 30;

            dateTimePickerFehcaLimite.ShowCheckBox = true;
            dateTimePickerFehcaLimite.Checked = false;
            dateTimePickerFehcaLimite.MinDate = DateTime.Today;

            lblMensaje.Text = string.Empty;
        }

        private void CargarTareaEnUi()
        {
            txtTitulo.Text = _tareaActual.Titulo;
            txtDescripcion.Text = _tareaActual.Descripcion;

            if (_tareaActual.FechaLimite.HasValue)
            {
                dateTimePickerFehcaLimite.Checked = true;

                DateTime fechaLimiteTarea = _tareaActual.FechaLimite.Value.Date;
                DateTime fechaMinimaPicker = dateTimePickerFehcaLimite.MinDate.Date;
                DateTime fechaLimiteParaPicker = fechaLimiteTarea < fechaMinimaPicker ? fechaMinimaPicker : fechaLimiteTarea;

                dateTimePickerFehcaLimite.Value = fechaLimiteParaPicker;
            }
            else
            {
                dateTimePickerFehcaLimite.Checked = false;
            }

            comboBoxImportancia.SelectedValue = _tareaActual.Importancia;
            comboBoxEnergiaRequerida.SelectedValue = _tareaActual.EnergiaRequerida;

            decimal duracionEstimadaMinutos = _tareaActual.DuracionEstimadaMin;

            if (duracionEstimadaMinutos < numericUpDownDuracionEstimada.Minimum)
            {
                duracionEstimadaMinutos = numericUpDownDuracionEstimada.Minimum;
            }
            else if (duracionEstimadaMinutos > numericUpDownDuracionEstimada.Maximum)
            {
                duracionEstimadaMinutos = numericUpDownDuracionEstimada.Maximum;
            }

            numericUpDownDuracionEstimada.Value = duracionEstimadaMinutos;
        }

        private void AplicarModo()
        {
            string textoTraduccion;

            if (_modoFormulario == ModoFormulario.Crear)
            {
                textoTraduccion = IdiomaService.Instancia.Traducir("RegTarea_Titulo_Crear");

                if (!string.IsNullOrWhiteSpace(textoTraduccion) && textoTraduccion != "RegTarea_Titulo_Crear")
                {
                    Text = textoTraduccion;
                }
                else
                {
                    Text = "Registrar Tarea";
                }

                textoTraduccion = IdiomaService.Instancia.Traducir("RegTarea_Boton_Guardar");

                if (!string.IsNullOrWhiteSpace(textoTraduccion) && textoTraduccion != "RegTarea_Boton_Guardar")
                {
                    btnGuardar.Text = textoTraduccion;
                }
                else
                {
                    btnGuardar.Text = "Guardar Tarea";
                }

                HabilitarEdicion(true);
            }
            else if (_modoFormulario == ModoFormulario.Editar)
            {
                textoTraduccion = IdiomaService.Instancia.Traducir("RegTarea_Titulo_Editar");

                if (!string.IsNullOrWhiteSpace(textoTraduccion) && textoTraduccion != "RegTarea_Titulo_Editar")
                {
                    Text = textoTraduccion;
                }
                else
                {
                    Text = "Editar Tarea";
                }

                textoTraduccion = IdiomaService.Instancia.Traducir("RegTarea_Boton_GuardarCambios");

                if (!string.IsNullOrWhiteSpace(textoTraduccion) && textoTraduccion != "RegTarea_Boton_GuardarCambios")
                {
                    btnGuardar.Text = textoTraduccion;
                }
                else
                {
                    btnGuardar.Text = "Guardar Cambios";
                }

                HabilitarEdicion(true);
            }
            else
            {
                textoTraduccion = IdiomaService.Instancia.Traducir("RegTarea_Titulo_Eliminar");

                if (!string.IsNullOrWhiteSpace(textoTraduccion) && textoTraduccion != "RegTarea_Titulo_Eliminar")
                {
                    Text = textoTraduccion;
                }
                else
                {
                    Text = "Eliminar Tarea";
                }

                textoTraduccion = IdiomaService.Instancia.Traducir("RegTarea_Boton_Eliminar");

                if (!string.IsNullOrWhiteSpace(textoTraduccion) && textoTraduccion != "RegTarea_Boton_Eliminar")
                {
                    btnGuardar.Text = textoTraduccion;
                }
                else
                {
                    btnGuardar.Text = "Eliminar";
                }

                HabilitarEdicion(false);
            }

            TopMost = true;
        }

        private void HabilitarEdicion(bool estaHabilitado)
        {
            txtTitulo.ReadOnly = !estaHabilitado;
            txtDescripcion.ReadOnly = !estaHabilitado;
            dateTimePickerFehcaLimite.Enabled = estaHabilitado;
            comboBoxImportancia.Enabled = estaHabilitado;
            comboBoxEnergiaRequerida.Enabled = estaHabilitado;
            numericUpDownDuracionEstimada.Enabled = estaHabilitado;
        }

        private void btnGuardar_Click(object sender, EventArgs eventArgs)
        {
            lblMensaje.ForeColor = Color.Maroon;
            lblMensaje.Text = string.Empty;

            try
            {
                if (_modoFormulario == ModoFormulario.Eliminar)
                {
                    ProcesarEliminacion();
                    return;
                }

                int usuarioId;
                DateTime? fechaLimite;

                if (!ValidarDatosFormulario(out usuarioId, out fechaLimite))
                {
                    return;
                }

                try
                {
                    if (_modoFormulario == ModoFormulario.Crear)
                    {
                        Tarea nuevaTarea = ConstruirNuevaTarea(usuarioId, fechaLimite);

                        RegistrarNuevaTarea(nuevaTarea);
                    }
                    else
                    {
                        ActualizarTareaActualDesdeFormulario(fechaLimite);
                    }
                }
                catch (Exception excepcionInterna)
                {
                    MostrarErrorClave("RegTarea_Error_General", "Error: {0}", excepcionInterna.Message);
                }

                InformarOperacionCorrecta();
            }
            catch (Exception excepcionGeneral)
            {
                MostrarErrorClave("RegTarea_Error_General", "Error: {0}", excepcionGeneral.Message);
            }
        }

        private void ProcesarEliminacion()
        {
            if (_tareaActual == null || _tareaActual.TareaId <= 0)
            {
                MostrarErrorClave("RegTarea_Error_EliminarInvalida", "Tarea inválida para eliminar.");
                return;
            }

            _tareaBl.Eliminar(_tareaActual.TareaId);
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidarDatosFormulario(out int usuarioId, out DateTime? fechaLimite)
        {
            usuarioId = 0;
            fechaLimite = null;

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MostrarErrorClave("RegTarea_Validacion_TituloObligatorio", "El título es obligatorio.");
                txtTitulo.Focus();
                return false;
            }

            if (numericUpDownDuracionEstimada.Value <= 0)
            {
                MostrarErrorClave("RegTarea_Validacion_Duracion", "La duración estimada debe ser mayor a 0.");
                numericUpDownDuracionEstimada.Focus();
                return false;
            }

            usuarioId = SesionActual.Instance.UsuarioId;

            if (usuarioId <= 0)
            {
                MostrarErrorClave("RegTarea_Error_SinSesion", "No hay sesión activa.");
                return false;
            }

            if (dateTimePickerFehcaLimite.Checked)
            {
                fechaLimite = dateTimePickerFehcaLimite.Value.Date;
            }

            if (fechaLimite.HasValue && fechaLimite.Value.Date < DateTime.Today)
            {
                MostrarErrorClave("RegTarea_Validacion_FechaLimiteMinima", "La fecha límite no puede ser anterior a hoy.");
                dateTimePickerFehcaLimite.Focus();
                return false;
            }

            return true;
        }

        private Tarea ConstruirNuevaTarea(int usuarioId, DateTime? fechaLimite)
        {
            Tarea nuevaTarea = new Tarea { UsuarioId = usuarioId, ProyectoId = null, Titulo = txtTitulo.Text.Trim(), Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(), FechaLimite = fechaLimite, Importancia = (ImportanciaTarea)comboBoxImportancia.SelectedValue, EnergiaRequerida = (EnergiaRequeridaTarea)comboBoxEnergiaRequerida.SelectedValue, DuracionEstimadaMin = (int)numericUpDownDuracionEstimada.Value, Estado = EstadoTarea.Pendiente };

            return nuevaTarea;
        }

        private void RegistrarNuevaTarea(Tarea nuevaTarea)
        {
            _tareaBl.Crear(nuevaTarea);
        }

        private void ActualizarTareaActualDesdeFormulario(DateTime? fechaLimite)
        {
            _tareaActual.Titulo = txtTitulo.Text.Trim();

            string descripcionTarea;

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                descripcionTarea = null;
            }
            else
            {
                descripcionTarea = txtDescripcion.Text.Trim();
            }

            _tareaActual.Descripcion = descripcionTarea;
            _tareaActual.FechaLimite = fechaLimite;
            _tareaActual.Importancia = (ImportanciaTarea)comboBoxImportancia.SelectedValue;
            _tareaActual.EnergiaRequerida = (EnergiaRequeridaTarea)comboBoxEnergiaRequerida.SelectedValue;
            _tareaActual.DuracionEstimadaMin = (int)numericUpDownDuracionEstimada.Value;

            _tareaBl.Actualizar(_tareaActual);
        }

        private void InformarOperacionCorrecta()
        {
            lblMensaje.ForeColor = Color.DarkGreen;
            SetMensaje("RegTarea_Ok", "Operación realizada correctamente.");
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs eventArgs)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            if (traducciones.ContainsKey("RegTarea_Label_Encabezado"))
            {
                lblEncabezado.Text = traducciones["RegTarea_Label_Encabezado"];
            }

            if (traducciones.ContainsKey("RegTarea_Label_Titulo"))
            {
                lblTitulo.Text = traducciones["RegTarea_Label_Titulo"];
            }

            if (traducciones.ContainsKey("RegTarea_Label_Descripcion"))
            {
                lblDescripcion.Text = traducciones["RegTarea_Label_Descripcion"];
            }

            if (traducciones.ContainsKey("RegTarea_Label_FechaLimite"))
            {
                lblFechaLimite.Text = traducciones["RegTarea_Label_FechaLimite"];
            }

            if (traducciones.ContainsKey("RegTarea_Label_Importancia"))
            {
                lblImportancia.Text = traducciones["RegTarea_Label_Importancia"];
            }

            if (traducciones.ContainsKey("RegTarea_Label_Energia"))
            {
                lblEnergiaRequerida.Text = traducciones["RegTarea_Label_Energia"];
            }

            if (traducciones.ContainsKey("RegTarea_Label_Duracion"))
            {
                lblDuracionEstimada.Text = traducciones["RegTarea_Label_Duracion"];
            }

            if (traducciones.ContainsKey("RegTarea_Boton_Cancelar"))
            {
                btnCancelar.Text = traducciones["RegTarea_Boton_Cancelar"];
            }

            AplicarModo();
            CargarCombosTraducidos();

            if (_lblMensajeClaveActual != null)
            {
                string textoTraducido = IdiomaService.Instancia.Traducir(_lblMensajeClaveActual);

                if (!string.IsNullOrWhiteSpace(textoTraducido) && textoTraducido != _lblMensajeClaveActual)
                {
                    lblMensaje.Text = string.Format(textoTraducido, _lblMensajeArgsActual ?? Array.Empty<object>());
                }
            }
        }

        private void CargarCombosTraducidos()
        {
            ImportanciaTarea seleccionImportancia;

            if (comboBoxImportancia.SelectedValue is ImportanciaTarea importanciaValor)
            {
                seleccionImportancia = importanciaValor;
            }
            else if (comboBoxImportancia.SelectedItem is KeyValuePair<ImportanciaTarea, string> importanciaPar)
            {
                seleccionImportancia = importanciaPar.Key;
            }
            else
            {
                seleccionImportancia = ImportanciaTarea.Media;
            }

            List<KeyValuePair<ImportanciaTarea, string>> itemsImportancia = new List<KeyValuePair<ImportanciaTarea, string>> { new KeyValuePair<ImportanciaTarea, string>(ImportanciaTarea.MuyBaja, IdiomaService.Instancia.Traducir("RegTarea_Importancia_MuyBaja")), new KeyValuePair<ImportanciaTarea, string>(ImportanciaTarea.Baja, IdiomaService.Instancia.Traducir("RegTarea_Importancia_Baja")), new KeyValuePair<ImportanciaTarea, string>(ImportanciaTarea.Media, IdiomaService.Instancia.Traducir("RegTarea_Importancia_Media")), new KeyValuePair<ImportanciaTarea, string>(ImportanciaTarea.Alta, IdiomaService.Instancia.Traducir("RegTarea_Importancia_Alta")), new KeyValuePair<ImportanciaTarea, string>(ImportanciaTarea.MuyAlta, IdiomaService.Instancia.Traducir("RegTarea_Importancia_MuyAlta")) };

            comboBoxImportancia.DisplayMember = "Value";
            comboBoxImportancia.ValueMember = "Key";
            comboBoxImportancia.DataSource = itemsImportancia;
            comboBoxImportancia.SelectedValue = seleccionImportancia;

            EnergiaRequeridaTarea seleccionEnergia;

            if (comboBoxEnergiaRequerida.SelectedValue is EnergiaRequeridaTarea energiaValor)
            {
                seleccionEnergia = energiaValor;
            }
            else if (comboBoxEnergiaRequerida.SelectedItem is KeyValuePair<EnergiaRequeridaTarea, string> energiaPar)
            {
                seleccionEnergia = energiaPar.Key;
            }
            else
            {
                seleccionEnergia = EnergiaRequeridaTarea.Media;
            }

            List<KeyValuePair<EnergiaRequeridaTarea, string>> itemsEnergia = new List<KeyValuePair<EnergiaRequeridaTarea, string>> { new KeyValuePair<EnergiaRequeridaTarea, string>(EnergiaRequeridaTarea.Baja, IdiomaService.Instancia.Traducir("RegTarea_Energia_Baja")), new KeyValuePair<EnergiaRequeridaTarea, string>(EnergiaRequeridaTarea.Media, IdiomaService.Instancia.Traducir("RegTarea_Energia_Media")), new KeyValuePair<EnergiaRequeridaTarea, string>(EnergiaRequeridaTarea.Alta, IdiomaService.Instancia.Traducir("RegTarea_Energia_Alta")) };

            comboBoxEnergiaRequerida.DisplayMember = "Value";
            comboBoxEnergiaRequerida.ValueMember = "Key";
            comboBoxEnergiaRequerida.DataSource = itemsEnergia;
            comboBoxEnergiaRequerida.SelectedValue = seleccionEnergia;
        }

        private void SetMensaje(string clave, string fallback, params object[] args)
        {
            _lblMensajeClaveActual = clave;
            _lblMensajeArgsActual = args;

            string textoTraduccion = IdiomaService.Instancia.Traducir(clave);

            if (string.IsNullOrEmpty(textoTraduccion) || textoTraduccion == clave)
            {
                textoTraduccion = fallback ?? string.Empty;
            }

            lblMensaje.Text = string.Format(textoTraduccion, args ?? Array.Empty<object>());
        }

        private void MostrarErrorClave(string clave, string fallback, params object[] args)
        {
            lblMensaje.ForeColor = Color.Maroon;
            SetMensaje(clave, fallback, args);
        }

        private void RegistrarTarea_Load(object sender, EventArgs e)
        {

        }
    }
}