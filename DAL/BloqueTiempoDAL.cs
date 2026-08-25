using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class BloqueTiempoDAL
    {
        public BloqueTiempoDAL()
        {
        }

        public int GuardarNuevo(BloqueTiempo bloque)
        {
            if (bloque == null)
            {
                throw new ArgumentNullException(nameof(bloque));
            }

            string titulo = string.Empty;
            object descripcion = DBNull.Value;

            if (bloque.Titulo != null)
            {
                titulo = bloque.Titulo;
            }
            else
            {
                titulo = string.Empty;
            }

            if (bloque.Descripcion != null)
            {
                descripcion = bloque.Descripcion;
            }
            else
            {
                descripcion = DBNull.Value;
            }

            List<SqlParameter> parametros = new List<SqlParameter> { new SqlParameter("@UsuarioId", bloque.UsuarioId), new SqlParameter("@Titulo", titulo), new SqlParameter("@Descripcion", descripcion), new SqlParameter("@TipoBloque", (int)bloque.TipoBloque), new SqlParameter("@DiaSemana", bloque.DiaSemana), new SqlParameter("@HoraInicio", bloque.HoraInicio), new SqlParameter("@HoraFin", bloque.HoraFin), new SqlParameter("@Activo", bloque.Activo), new SqlParameter("@CreadoUtc", bloque.CreadoUtc), new SqlParameter("@ActualizadoUtc", bloque.ActualizadoUtc) };

            AccesoBD accesoBD = new AccesoBD();
            object resultado = accesoBD.ExecuteScalarSp("spBloqueTiempo_Insertar", parametros.ToArray());

            int bloqueTiempoId;

            if (resultado != null && resultado != DBNull.Value && int.TryParse(resultado.ToString(), out bloqueTiempoId) && bloqueTiempoId > 0)
            {
                bloque.BloqueTiempoId = bloqueTiempoId;

                return bloqueTiempoId;
            }

            return 0;
        }

        public void Guardar(BloqueTiempo bloque)
        {
            if (bloque == null)
            {
                throw new ArgumentNullException(nameof(bloque));
            }

            string titulo = string.Empty;
            object descripcion = DBNull.Value;

            if (bloque.Titulo != null)
            {
                titulo = bloque.Titulo;
            }
            else
            {
                titulo = string.Empty;
            }

            if (bloque.Descripcion != null)
            {
                descripcion = bloque.Descripcion;
            }
            else
            {
                descripcion = DBNull.Value;
            }

            List<SqlParameter> parametros = new List<SqlParameter> { new SqlParameter("@BloqueTiempoId", bloque.BloqueTiempoId), new SqlParameter("@UsuarioId", bloque.UsuarioId), new SqlParameter("@Titulo", titulo), new SqlParameter("@Descripcion", descripcion), new SqlParameter("@TipoBloque", (int)bloque.TipoBloque), new SqlParameter("@DiaSemana", bloque.DiaSemana), new SqlParameter("@HoraInicio", bloque.HoraInicio), new SqlParameter("@HoraFin", bloque.HoraFin), new SqlParameter("@Activo", bloque.Activo), new SqlParameter("@ActualizadoUtc", bloque.ActualizadoUtc) };

            AccesoBD accesoBD = new AccesoBD();

            accesoBD.ExecuteNonQuerySp("spBloqueTiempo_Actualizar", parametros.ToArray());
        }

        public void Eliminar(int bloqueTiempoId)
        {
            AccesoBD accesoBD = new AccesoBD();

            accesoBD.ExecuteNonQuerySp("spBloqueTiempo_Eliminar", new SqlParameter("@BloqueTiempoId", bloqueTiempoId), new SqlParameter("@ActualizadoUtc", DateTime.UtcNow));
        }

        public BloqueTiempo Obtener(int bloqueTiempoId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spBloqueTiempo_Obtener", new SqlParameter("@BloqueTiempoId", bloqueTiempoId));

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            BloqueTiempo bloque = new BloqueTiempo();

            ValorizarEntidad(bloque, dataSet.Tables[0].Rows[0]);

            return bloque;
        }

        public List<BloqueTiempo> ListarPorUsuario(int usuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spBloqueTiempo_ListarPorUsuario", new SqlParameter("@UsuarioId", usuarioId));

            return MapearLista(dataSet);
        }

        public List<BloqueTiempo> ListarPorUsuarioYDia(int usuarioId, int diaSemana)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spBloqueTiempo_ListarPorUsuarioYDia", new SqlParameter("@UsuarioId", usuarioId), new SqlParameter("@DiaSemana", diaSemana));

            return MapearLista(dataSet);
        }

        private List<BloqueTiempo> MapearLista(DataSet dataSet)
        {
            List<BloqueTiempo> bloques = new List<BloqueTiempo>();

            if (dataSet.Tables.Count == 0)
            {
                return bloques;
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                BloqueTiempo bloque = new BloqueTiempo();

                ValorizarEntidad(bloque, dataRow);

                bloques.Add(bloque);
            }

            return bloques;
        }

        private void ValorizarEntidad(BloqueTiempo bloque, DataRow dataRow)
        {
            bloque.BloqueTiempoId = Convert.ToInt32(dataRow["BloqueTiempoId"]);
            bloque.UsuarioId = Convert.ToInt32(dataRow["UsuarioId"]);

            if (dataRow["Titulo"] == DBNull.Value)
            {
                bloque.Titulo = null;
            }
            else
            {
                bloque.Titulo = dataRow["Titulo"].ToString();
            }

            if (dataRow["Descripcion"] == DBNull.Value)
            {
                bloque.Descripcion = null;
            }
            else
            {
                bloque.Descripcion = dataRow["Descripcion"].ToString();
            }

            bloque.TipoBloque = (TipoBloqueTiempo)Convert.ToInt32(dataRow["TipoBloque"]);
            bloque.DiaSemana = Convert.ToInt32(dataRow["DiaSemana"]);
            bloque.HoraInicio = ObtenerTimeSpan(dataRow["HoraInicio"]);
            bloque.HoraFin = ObtenerTimeSpan(dataRow["HoraFin"]);
            bloque.Activo = Convert.ToBoolean(dataRow["Activo"]);

            DateTime creado = Convert.ToDateTime(dataRow["CreadoUtc"]);
            DateTime actualizado = Convert.ToDateTime(dataRow["ActualizadoUtc"]);

            bloque.CreadoUtc = DateTime.SpecifyKind(creado, DateTimeKind.Utc);
            bloque.ActualizadoUtc = DateTime.SpecifyKind(actualizado, DateTimeKind.Utc);
        }

        private TimeSpan ObtenerTimeSpan(object value)
        {
            if (value is TimeSpan)
            {
                return (TimeSpan)value;
            }

            return TimeSpan.Parse(value.ToString());
        }
    }
}