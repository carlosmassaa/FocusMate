using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BackupDAL
    {
        public BackupDAL()
        {
        }

        public void HacerBackup(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
            {
                throw new ArgumentException("El nombre de archivo es inválido.", nameof(nombreArchivo));
            }

            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spBaseDatos_HacerBackup", new SqlParameter("@RutaArchivo", nombreArchivo));
        }

        public void RestaurarBackup(string rutaArchivo, string nombreBaseDatos)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("La ruta del archivo es inválida.", nameof(rutaArchivo));
            }

            if (string.IsNullOrWhiteSpace(nombreBaseDatos))
            {
                throw new ArgumentException("El nombre de la base es inválido.", nameof(nombreBaseDatos));
            }

            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySpMaster("spBaseDatos_RestaurarBackup",new SqlParameter("@RutaArchivo", rutaArchivo),new SqlParameter("@NombreDb", nombreBaseDatos));
        }

        public void RestaurarBackup(string rutaArchivo)
        {
            RestaurarBackup(rutaArchivo, "FocusMateTDFinalFinal");
        }

        public string ObtenerDirectorioBackupPorDefecto()
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spBaseDatos_ObtenerDirectorioBackup");

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return string.Empty;
            }

            return dataSet.Tables[0].Rows[0][0]?.ToString() ?? string.Empty;
        }
    }
}
