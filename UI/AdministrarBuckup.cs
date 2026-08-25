using System;
using System.IO;
using System.Windows.Forms;
using BL;
using System.Collections.Generic;
using Servicioss;

namespace UI
{
    public partial class AdministrarBuckup : Form, IIdiomaObserver
    {
        private readonly AuthManager _authManager;
        private readonly BackupBL _backupBl = BackupBL.CrearBasico();

        private string _defaultBackupDir;
        private const string RutaFijaDefault = @"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup";

        private const string PermisoAcceder = "ACCEDER_GESTIONAR_BACKUP";
        private const string PermisoHacer = "BACKUP_HACER";
        private const string PermisoRestaurar = "BACKUP_RESTAURAR";
        private const string PermisoSeleccionar = "BACKUP_SELECCIONAR_ARCHIVO";
        private const string PermisoCancelar = "BACKUP_CANCELAR";
        private const string PermisoCancelarSeleccion = "BACKUP_CANCELAR_SELECCION";

        public AdministrarBuckup(AuthManager authManager)
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
        }

        private void AdministrarBuckup_Load(object sender, EventArgs eventArgs)
        {
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

            if (_authManager == null || !_authManager.EstaAutenticado || !_authManager.ValidarPermiso(PermisoAcceder))
            {
                MessageBox.Show(this, string.Format(Trad("AdminBkp_Msg_SinPermisoAcceso", "No tiene permiso para acceder a la administración de backups ({0})."), PermisoAcceder), Trad("AdminBkp_Msg_TituloPermisos", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            _defaultBackupDir = _backupBl.ObtenerDirectorioBackupPorDefecto();
            if (string.IsNullOrWhiteSpace(_defaultBackupDir))
            {
                _defaultBackupDir = RutaFijaDefault;
            }

            txtDirectorio.Text = _defaultBackupDir;

            txtNombreArchivo.Text = "FocusMateTDFinal_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
            txtNombreArchivo.SelectAll();

            AplicarPermisosAControles();
        }

        protected override void OnFormClosed(FormClosedEventArgs eventArgs)
        {
            IdiomaService.Instancia.Desuscribir(this);
            base.OnFormClosed(eventArgs);
        }

        private void AplicarPermisosAControles()
        {
            btnGuardar.Enabled = _authManager.ValidarPermiso(PermisoHacer);
            btnRestaurar.Enabled = _authManager.ValidarPermiso(PermisoRestaurar);
            btnSeleccionarBackup.Enabled = _authManager.ValidarPermiso(PermisoSeleccionar);
            btnCancelar.Enabled = _authManager.ValidarPermiso(PermisoCancelar);
            btnCancelarRestore.Enabled = _authManager.ValidarPermiso(PermisoCancelarSeleccion);

            txtNombreArchivo.Enabled = btnGuardar.Enabled;
            txtDirectorio.Enabled = true;
            txtArchivoSeleccionado.Enabled = btnRestaurar.Enabled || btnSeleccionarBackup.Enabled;
        }

        private bool VerificarPermisoOAdvertir(string patente)
        {
            if (_authManager.ValidarPermiso(patente))
            {
                return true;
            }

            MessageBox.Show(this, string.Format(Trad("AdminBkp_Permiso_AccionRequerida", "No tiene permiso para realizar esta acción ({0})."), patente), Trad("AdminBkp_Msg_TituloPermisos", "Permisos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AplicarPermisosAControles();
            return false;
        }

        private void btnGuardar_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoHacer))
            {
                return;
            }

            string nombreArchivoBackup = (txtNombreArchivo.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombreArchivoBackup))
            {
                MessageBox.Show(this, Trad("AdminBkp_Msg_IngreseNombreArchivo", "Debe ingresar un nombre de archivo."), Trad("AdminBkp_Msg_TituloBackup", "Backup"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombreArchivo.Focus();
                return;
            }

            nombreArchivoBackup = Path.GetFileName(nombreArchivoBackup);

            if (!nombreArchivoBackup.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
            {
                nombreArchivoBackup += ".bak";
            }

            try
            {
                _backupBl.HacerBackup(nombreArchivoBackup);

                string rutaCompleta = Path.Combine(_defaultBackupDir, nombreArchivoBackup);
                MessageBox.Show(this, string.Format(Trad("AdminBkp_Msg_BackupOk", "Backup realizado correctamente.\n\nUbicación:\n{0}"), rutaCompleta), Trad("AdminBkp_Msg_TituloBackup", "Backup"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("AdminBkp_Msg_BackupError", "Error al realizar el backup:\n{0}"), exception.Message), Trad("AdminBkp_Msg_TituloBackup", "Backup"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoCancelar))
            {
                return;
            }

            Close();
        }

        private void btnSeleccionarBackup_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoSeleccionar))
            {
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = Trad("AdminBkp_Dialogo_SeleccionarBackupTitulo", "Seleccionar archivo de backup (.bak)");
                openFileDialog.Filter = Trad("AdminBkp_Dialogo_FiltroBak", "Archivos de backup (*.bak)|*.bak");

                if (Directory.Exists(_defaultBackupDir))
                {
                    openFileDialog.InitialDirectory = _defaultBackupDir;
                }
                else
                {
                    openFileDialog.InitialDirectory = RutaFijaDefault;
                }

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtArchivoSeleccionado.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoRestaurar))
            {
                return;
            }

            string rutaArchivoBackup = txtArchivoSeleccionado.Text.Trim();
            if (string.IsNullOrWhiteSpace(rutaArchivoBackup))
            {
                MessageBox.Show(this, Trad("AdminBkp_Msg_SeleccioneArchivoBak", "Debe seleccionar un archivo .bak."), Trad("AdminBkp_Msg_TituloRestaurar", "Restaurar"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(this, Trad("AdminBkp_Msg_ConfirmarRestauracion", "Esto reemplazará la base de datos actual.\n¿Desea continuar?"), Trad("AdminBkp_Msg_Confirmar", "Confirmar restauración"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _backupBl.RestaurarBackup(rutaArchivoBackup, "FocusMateTDFinalFinal");

                MessageBox.Show(this, Trad("AdminBkp_Msg_RestoreOk", "Restauración completada correctamente.\nSe reemplazó la base de datos FocusMateTDFinalFinal."), Trad("AdminBkp_Msg_TituloRestaurar", "Restaurar"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, string.Format(Trad("AdminBkp_Msg_RestoreError", "Error al restaurar el backup:\n{0}"), exception.Message), Trad("AdminBkp_Msg_TituloRestaurar", "Restaurar"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelarRestore_Click(object sender, EventArgs eventArgs)
        {
            if (!VerificarPermisoOAdvertir(PermisoCancelarSeleccion))
            {
                return;
            }

            txtArchivoSeleccionado.Text = string.Empty;
        }

        public void ActualizarTraducciones(Dictionary<string, string> traducciones)
        {
            if (traducciones == null)
            {
                return;
            }

            AplicarTraduccionesEstaticas();
        }

        private void AplicarTraduccionesEstaticas()
        {
            this.Text = Trad("AdminBkp_Titulo", "Administrar Buckup");

            if (grpCrear != null)
            {
                grpCrear.Text = Trad("AdminBkp_Group_Crear", "Crear backup");
            }

            if (lblDirCaption != null)
            {
                lblDirCaption.Text = Trad("AdminBkp_Label_Directorio", "Directorio de backup (en el servidor):");
            }

            if (lblNombreCaption != null)
            {
                lblNombreCaption.Text = Trad("AdminBkp_Label_NombreArchivo", "Nombre de archivo (sin ruta):");
            }

            if (lblHint != null)
            {
                lblHint.Text = Trad("AdminBkp_Label_HintBak", "Sugerencia: se agregará .bak si no lo especifica.");
            }

            if (btnGuardar != null)
            {
                btnGuardar.Text = Trad("AdminBkp_Boton_Guardar", "Guardar backup");
            }

            if (btnCancelar != null)
            {
                btnCancelar.Text = Trad("AdminBkp_Boton_Cerrar", "Cerrar");
            }

            if (grpRestaurar != null)
            {
                grpRestaurar.Text = Trad("AdminBkp_Group_Restaurar", "Restaurar backup (reemplaza la base de datos)");
            }

            if (lblArchivoSeleccionado != null)
            {
                lblArchivoSeleccionado.Text = Trad("AdminBkp_Label_ArchivoSeleccionado", "Archivo .bak elegido:");
            }

            if (btnSeleccionarBackup != null)
            {
                btnSeleccionarBackup.Text = Trad("AdminBkp_Boton_SeleccionarArchivo", "Seleccionar archivo...");
            }

            if (btnRestaurar != null)
            {
                btnRestaurar.Text = Trad("AdminBkp_Boton_Restaurar", "Restaurar");
            }

            if (btnCancelarRestore != null)
            {
                btnCancelarRestore.Text = Trad("AdminBkp_Boton_Cancelar", "Cancelar");
            }
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
