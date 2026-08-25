using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class UsuarioDAL
    {
        public UsuarioDAL()
        {
        }

        private object ToDbDateTime(DateTime value)
        {
            DateTime minDateTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            if (value < minDateTime)
            {
                return (object)DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        public int Guardar(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            if (usuario.Id == 0 && usuario.CreadoUtc < (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue)
            {
                usuario.CreadoUtc = DateTime.UtcNow;
            }

            usuario.ActualizadoUtc = DateTime.UtcNow;

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (usuario.Id != 0)
            {
                parametros.Add(new SqlParameter("@Id_Usuario", usuario.Id));
            }

            object nombreUsuarioParam;
            if (usuario.NombreUsuario == null)
            {
                nombreUsuarioParam = DBNull.Value;
            }
            else
            {
                nombreUsuarioParam = usuario.NombreUsuario;
            }

            int hashLen;
            object hashValue;
            if (usuario.PasswordHash == null)
            {
                hashLen = 0;
                hashValue = DBNull.Value;
            }
            else
            {
                hashLen = usuario.PasswordHash.Length;
                hashValue = usuario.PasswordHash;
            }

            int saltLen;
            object saltValue;
            if (usuario.PasswordSalt == null)
            {
                saltLen = 0;
                saltValue = DBNull.Value;
            }
            else
            {
                saltLen = usuario.PasswordSalt.Length;
                saltValue = usuario.PasswordSalt;
            }

            object passwordAlgParam;
            if (usuario.PasswordAlg == null)
            {
                passwordAlgParam = DBNull.Value;
            }
            else
            {
                passwordAlgParam = usuario.PasswordAlg;
            }

            object idiomaIdParam;
            if (usuario.IdiomaId.HasValue)
            {
                idiomaIdParam = usuario.IdiomaId.Value;
            }
            else
            {
                idiomaIdParam = DBNull.Value;
            }

            parametros.AddRange(new[]{new SqlParameter("@NombreUsuario", nombreUsuarioParam),new SqlParameter("@PasswordHash", SqlDbType.VarBinary, hashLen) { Value = hashValue },new SqlParameter("@PasswordSalt", SqlDbType.VarBinary, saltLen) { Value = saltValue },new SqlParameter("@PasswordAlg", passwordAlgParam),new SqlParameter("@CreadoUtc", ToDbDateTime(usuario.CreadoUtc)),new SqlParameter("@EstaActivo", usuario.EstaActivo),new SqlParameter("@FailedAttempts", usuario.FailedAttempts),new SqlParameter("@BloqueadoHastaUtc", ToDbDateTime(usuario.BloqueadoHastaUtc)),new SqlParameter("@ActualizadoUtc", ToDbDateTime(usuario.ActualizadoUtc)),new SqlParameter("@IdiomaId", idiomaIdParam)});

            AccesoBD accesoBD = new AccesoBD();
            object escalar = accesoBD.ExecuteScalarSp("spUsuario_Guardar", parametros.ToArray());

            if (escalar != null && escalar != DBNull.Value)
            {
                int id;
                if (int.TryParse(escalar.ToString(), out id))
                {
                    if (id > 0)
                    {
                        usuario.Id = id;
                        return id;
                    }
                }
            }

            int filas = accesoBD.ExecuteNonQuerySp("spUsuario_Guardar", parametros.ToArray());

            if (usuario.Id != 0)
            {
                return usuario.Id;
            }
            else
            {
                return filas;
            }
        }

        public Usuario Obtener(int usuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spUsuario_ObtenerPorId", new SqlParameter("@Id_Usuario", usuarioId));

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            Usuario u = new Usuario();
            ValorizarEntidad(u, ds.Tables[0].Rows[0]);
            return u;
        }

        public List<Usuario> Listar()
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spUsuario_Listar");
            List<Usuario> lista = new List<Usuario>();

            if (ds.Tables.Count == 0)
            {
                return lista;
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Usuario u = new Usuario();
                ValorizarEntidad(u, row);
                lista.Add(u);
            }

            return lista;
        }

        public Usuario ObtenerPorNombre(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                return null;
            }

            AccesoBD accesoBD = new AccesoBD();
            DataSet ds = accesoBD.ExecuteDataSetSp("spUsuario_ObtenerPorNombre", new SqlParameter("@NombreUsuario", nombreUsuario));

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            Usuario u = new Usuario();
            ValorizarEntidad(u, ds.Tables[0].Rows[0]);
            return u;
        }

        public int GuardarNuevo(Usuario usuario)
        {
            usuario.Id = 0;
            return Guardar(usuario);
        }

        internal void ValorizarEntidad(Usuario usuario, DataRow dataRow)
        {
            usuario.Id = Convert.ToInt32(dataRow["Id_Usuario"]);

            if (dataRow["NombreUsuario"] == DBNull.Value)
            {
                usuario.NombreUsuario = null;
            }
            else
            {
                usuario.NombreUsuario = dataRow["NombreUsuario"].ToString();
            }

            if (dataRow.Table.Columns.Contains("PasswordHash"))
            {
                usuario.PasswordHash = dataRow["PasswordHash"] as byte[];
            }
            else
            {
                usuario.PasswordHash = null;
            }

            if (dataRow.Table.Columns.Contains("PasswordSalt"))
            {
                usuario.PasswordSalt = dataRow["PasswordSalt"] as byte[];
            }
            else
            {
                usuario.PasswordSalt = null;
            }

            if (dataRow["PasswordAlg"] == DBNull.Value)
            {
                usuario.PasswordAlg = null;
            }
            else
            {
                usuario.PasswordAlg = dataRow["PasswordAlg"].ToString();
            }

            usuario.CreadoUtc = Convert.ToDateTime(dataRow["CreadoUtc"]);
            usuario.EstaActivo = Convert.ToBoolean(dataRow["EstaActivo"]);
            usuario.FailedAttempts = Convert.ToInt32(dataRow["FailedAttempts"]);

            if (dataRow["BloqueadoHastaUtc"] == DBNull.Value)
            {
                usuario.BloqueadoHastaUtc = DateTime.MinValue;
            }
            else
            {
                usuario.BloqueadoHastaUtc = Convert.ToDateTime(dataRow["BloqueadoHastaUtc"]);
            }

            if (dataRow["ActualizadoUtc"] == DBNull.Value)
            {
                usuario.ActualizadoUtc = DateTime.MinValue;
            }
            else
            {
                usuario.ActualizadoUtc = Convert.ToDateTime(dataRow["ActualizadoUtc"]);
            }

            
            if (dataRow.Table.Columns.Contains("IdiomaId") && dataRow["IdiomaId"] != DBNull.Value)
            {
                usuario.IdiomaId = Convert.ToInt32(dataRow["IdiomaId"]);
            }
            else
            {
                usuario.IdiomaId = null;
            }
        }

        public bool UsuarioTienePermisoDescripcion(int usuarioId, string descripcion)
        {
            if (usuarioId <= 0 || string.IsNullOrWhiteSpace(descripcion))
            {
                return false;
            }

            AccesoBD accesoBD = new AccesoBD();
            object esc = accesoBD.ExecuteScalarSp("spUsuario_TienePermisoDescripcion",new SqlParameter("@Id_Usuario", usuarioId),new SqlParameter("@Descripcion", descripcion));

            if (esc == null || esc == DBNull.Value)
            {
                return false;
            }

            if (int.TryParse(esc.ToString(), out int vInt))
            {
                return vInt != 0;
            }

            if (bool.TryParse(esc.ToString(), out bool vBool))
            {
                return vBool;
            }

            return false;
        }
    }
}
