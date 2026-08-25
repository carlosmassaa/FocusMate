using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class ComponenteDAL
    {
        public ComponenteDAL()
        {
        }        

        private Componente VerificarTipo(DataRow row)
        {
            string tipo = null;
            if (row.Table.Columns.Contains("Tipo") && row["Tipo"] != DBNull.Value)
            {
                tipo = row["Tipo"].ToString();
            }

            bool esFamilia = string.Equals(tipo.Trim(), "Familia", StringComparison.OrdinalIgnoreCase);

            Componente componente;
            if (esFamilia)
            {
                componente = new Familia();
            }
            else
            {
                componente = new Patente();
            }

            ValorizarEntidad(componente, row);
            return componente;
        }


        public Componente Obtener(int componenteId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spComponente_Obtener", new SqlParameter("@Id", componenteId));

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            return VerificarTipo(ds.Tables[0].Rows[0]);
        }

        public int CrearPatente(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return 0;
            }

            AccesoBD accesoBD = new AccesoBD();
            object esc = accesoBD.ExecuteScalarSp("spComponente_CrearPatenteSiNoExiste", new SqlParameter("@Descripcion", descripcion));

            int id;
            if (esc != null && esc != DBNull.Value && int.TryParse(esc.ToString(), out id))
            {
                return id;
            }

            return 0;
        }

        public int CrearFamilia(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return 0;
            }

            AccesoBD accesoBD = new AccesoBD();
            object esc = accesoBD.ExecuteScalarSp("spComponente_CrearFamiliaSiNoExiste", new SqlParameter("@Descripcion", descripcion));

            int id;
            if (esc != null && esc != DBNull.Value && int.TryParse(esc.ToString(), out id))
            {
                return id;
            }

            return 0;
        }

        public List<Componente> ObtenerTodasFamilias()
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spComponente_ListarFamilias");
            List<Componente> list = new List<Componente>();

            if (ds.Tables.Count == 0)
            {
                return list;
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                list.Add(VerificarTipo(r));
            }

            return list;
        }

        public List<Componente> ObtenerTodasPatentes()
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spComponente_ListarPatentes");
            List<Componente> list = new List<Componente>();

            if (ds.Tables.Count == 0)
            {
                return list;
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                list.Add(VerificarTipo(r));
            }

            return list;
        }

        public List<Componente> ObtenerHijosDeFamilia(int idFamilia)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spFamilia_ListarHijos", new SqlParameter("@IdFamilia", idFamilia));

            List<Componente> list = new List<Componente>();

            if (ds.Tables.Count == 0)
            {
                return list;
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                list.Add(VerificarTipo(r));
            }

            return list;
        }

        public List<Componente> ObtenerRaicesDeUsuario(int usuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spUsuario_ListarComponentesRaiz", new SqlParameter("@IdUsuario", usuarioId));

            List<Componente> list = new List<Componente>();

            if (ds.Tables.Count == 0)
            {
                return list;
            }

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                list.Add(VerificarTipo(r));
            }

            return list;
        }

        public void AgregarHijoAFamilia(int idFamilia, int idHijo)
        {
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spFamilia_AgregarHijo", new SqlParameter("@IdFamilia", idFamilia), new SqlParameter("@IdHijo", idHijo));
        }

        public void QuitarHijoDeFamilia(int idFamilia, int idHijo)
        {
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spFamilia_QuitarHijo", new SqlParameter("@IdFamilia", idFamilia), new SqlParameter("@IdHijo", idHijo));
        }

        public void AsignarComponenteAUsuario(int usuarioId, int componenteId)
        {
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spUsuario_AsignarComponente", new SqlParameter("@IdUsuario", usuarioId), new SqlParameter("@IdComponente", componenteId));
        }

        public void QuitarComponenteDeUsuario(int usuarioId, int componenteId)
        {
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spUsuario_QuitarComponente", new SqlParameter("@IdUsuario", usuarioId), new SqlParameter("@IdComponente", componenteId));
        }

        public void ActualizarFamilia(int idFamilia, string descripcion)
        {            
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spFamilia_ActualizarDescripcion", new SqlParameter("@IdFamilia", idFamilia), new SqlParameter("@Descripcion", descripcion));

        }

        public void EliminarFamiliaConAsociaciones(int idFamilia)
        {
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spFamilia_EliminarConAsociaciones", new SqlParameter("@IdFamilia", idFamilia));
        }

        private void ValorizarEntidad(Componente componente, DataRow row)
        {
            int id = 0;
            if (row.Table.Columns.Contains("Id_Componente") && row["Id_Componente"] != DBNull.Value)
            {
                int.TryParse(row["Id_Componente"].ToString(), out id);
            }

            string descripcion = string.Empty;
            if (row.Table.Columns.Contains("Descripcion") && row["Descripcion"] != DBNull.Value)
            {
                descripcion = row["Descripcion"].ToString();
            }

            componente.Id = id;
            componente.Nombre = descripcion;
            componente.Descripcion = descripcion;
        }
    }
}
