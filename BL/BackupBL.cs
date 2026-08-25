using System;
using DAL;

namespace BL
{
    public class BackupBL
    {
        private readonly BackupDAL backupDal;

        public BackupBL(BackupDAL backupDal)
        {
            this.backupDal = backupDal ?? throw new ArgumentNullException(nameof(backupDal));
        }

        public static BackupBL CrearBasico()
        {
            BackupDAL instanciaBackupDal = new BackupDAL();
            BackupBL instanciaBackupBl = new BackupBL(instanciaBackupDal);
            return instanciaBackupBl;
        }

        public void HacerBackup(string nombreArchivo)
        {
            backupDal.HacerBackup(nombreArchivo);
        }

        public string ObtenerDirectorioBackupPorDefecto()
        {
            return backupDal.ObtenerDirectorioBackupPorDefecto();
        }

        public void RestaurarBackup(string rutaArchivo, string nombreBaseDatos = "FocusMateTDFinalFinal")
        {
            backupDal.RestaurarBackup(rutaArchivo, nombreBaseDatos);
        }
    }
}
