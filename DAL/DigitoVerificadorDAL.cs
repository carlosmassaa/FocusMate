using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DigitoVerificadorDAL
    {
        public DigitoVerificadorDAL()
        {
        }

        public long ObtenerDVV(string tabla)
        {
            AccesoBD accesoBD = new AccesoBD();
            object esc = accesoBD.ExecuteScalarSp("spDV_ObtenerDVV", new SqlParameter("@Tabla", tabla));
            long dvvOut;
            if (esc != null && esc != DBNull.Value && long.TryParse(esc.ToString(), out dvvOut))
            {
                return dvvOut;
            }

            DataSet ds = accesoBD.ExecuteDataSetSp("spDV_ObtenerDVV", new SqlParameter("@Tabla", tabla));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Columns.Contains("DVV"))
                {
                    object v = ds.Tables[0].Rows[0]["DVV"];
                    if (v != DBNull.Value && long.TryParse(v.ToString(), out dvvOut))
                    {
                        return dvvOut;
                    }
                }
                object v0 = ds.Tables[0].Rows[0][0];
                if (v0 != DBNull.Value && long.TryParse(v0.ToString(), out dvvOut))
                {
                    return dvvOut;
                }
            }
            return 0L;
        }

        public void UpsertDVV(string tabla, long dvv)
        {
            AccesoBD accesoBD = new AccesoBD();
            int rows = accesoBD.ExecuteNonQuerySp("spDV_EditarDVV", new SqlParameter("@Tabla", tabla), new SqlParameter("@DVV", dvv));

            if (rows == 0)
            {
                accesoBD.ExecuteNonQuerySp("spDV_GuardarDVV", new SqlParameter("@Tabla", tabla), new SqlParameter("@DVV", dvv));
            }
        }
    }
}