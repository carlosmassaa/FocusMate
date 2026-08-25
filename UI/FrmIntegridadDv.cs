using System;
using System.IO;
using System.Windows.Forms;
using BL;

namespace UI
{
    public enum IntegridadDecision
    {
        AceptarCambios,
        RestaurarBackup,
        Cancelar
    }

    public partial class FrmIntegridadDv : Form
    {
        private readonly BackupBL _backupBl;
        private readonly string _detalle;
        private string _defaultBackupDirectory;

        public IntegridadDecision Decision { get; private set; } = IntegridadDecision.Cancelar;
        public string BackupRestaurado { get; private set; }

        public FrmIntegridadDv(string detalle)
        {
            InitializeComponent();
            _detalle = detalle ?? string.Empty;
            _backupBl = BackupBL.CrearBasico();
            Cargar();
        }

        private void Cargar()
        {
            txtDetalle.Text = _detalle;
            _defaultBackupDirectory = _backupBl.ObtenerDirectorioBackupPorDefecto();

            if (string.IsNullOrWhiteSpace(_defaultBackupDirectory))
            {
                _defaultBackupDirectory = @"C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup";
            }
        }

        private void btnAceptarCambios_Click(object sender, EventArgs eventArgs)
        {
            Decision = IntegridadDecision.AceptarCambios;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnRestaurarBackup_Click(object sender, EventArgs eventArgs)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Seleccionar archivo .bak para restaurar";
                openFileDialog.Filter = "Archivos de backup (*.bak)|*.bak";

                string initialDirectoryPath;

                if (Directory.Exists(_defaultBackupDirectory))
                {
                    initialDirectoryPath = _defaultBackupDirectory;
                }
                else
                {
                    initialDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }

                openFileDialog.InitialDirectory = initialDirectoryPath;

                if (openFileDialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                string rutaArchivoBackup = openFileDialog.FileName;

                DialogResult confirmar = MessageBox.Show(this, "Se restaurará el backup y se reemplazará la base actual.\n¿Confirmar?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmar != DialogResult.Yes)
                {
                    return;
                }

                try
                {
                    _backupBl.RestaurarBackup(rutaArchivoBackup, "FocusMateTDFinalFinal");
                    BackupRestaurado = rutaArchivoBackup;

                    MessageBox.Show(this, "Restauración completada.\nArchivo: " + rutaArchivoBackup + "\nInicie sesión nuevamente.", "Restauración", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Decision = IntegridadDecision.RestaurarBackup;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, "Error al restaurar el backup:\n" + exception.Message, "Restauración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs eventArgs)
        {
            Decision = IntegridadDecision.Cancelar;
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
