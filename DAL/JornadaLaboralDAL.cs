using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class JornadaLaboralDAL
    {
        public JornadaLaboralDAL()
        {
        }

        public int GuardarNueva(JornadaLaboralUsuario jornada)
        {
            if (jornada == null)
            {
                throw new ArgumentNullException(nameof(jornada));
            }

            List<SqlParameter> parametros = new List<SqlParameter> { new SqlParameter("@UsuarioId", jornada.UsuarioId), new SqlParameter("@DiaSemana", jornada.DiaSemana), new SqlParameter("@HoraInicio", jornada.HoraInicio), new SqlParameter("@HoraFin", jornada.HoraFin), new SqlParameter("@Activo", jornada.Activo), new SqlParameter("@CreadoUtc", jornada.CreadoUtc), new SqlParameter("@ActualizadoUtc", jornada.ActualizadoUtc) };

            AccesoBD accesoBD = new AccesoBD();
            object resultado = accesoBD.ExecuteScalarSp("spJornadaLaboral_Insertar", parametros.ToArray());

            int jornadaLaboralUsuarioId;

            if (resultado != null && resultado != DBNull.Value && int.TryParse(resultado.ToString(), out jornadaLaboralUsuarioId) && jornadaLaboralUsuarioId > 0)
            {
                jornada.JornadaLaboralUsuarioId = jornadaLaboralUsuarioId;

                return jornadaLaboralUsuarioId;
            }

            return 0;
        }

        public void Guardar(JornadaLaboralUsuario jornada)
        {
            if (jornada == null)
            {
                throw new ArgumentNullException(nameof(jornada));
            }

            List<SqlParameter> parametros = new List<SqlParameter> { new SqlParameter("@JornadaLaboralUsuarioId", jornada.JornadaLaboralUsuarioId), new SqlParameter("@UsuarioId", jornada.UsuarioId), new SqlParameter("@DiaSemana", jornada.DiaSemana), new SqlParameter("@HoraInicio", jornada.HoraInicio), new SqlParameter("@HoraFin", jornada.HoraFin), new SqlParameter("@Activo", jornada.Activo), new SqlParameter("@ActualizadoUtc", jornada.ActualizadoUtc) };

            AccesoBD accesoBD = new AccesoBD();

            accesoBD.ExecuteNonQuerySp("spJornadaLaboral_Actualizar", parametros.ToArray());
        }

        public void Eliminar(int jornadaLaboralUsuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();

            accesoBD.ExecuteNonQuerySp("spJornadaLaboral_Eliminar", new SqlParameter("@JornadaLaboralUsuarioId", jornadaLaboralUsuarioId), new SqlParameter("@ActualizadoUtc", DateTime.UtcNow));
        }

        public JornadaLaboralUsuario Obtener(int jornadaLaboralUsuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spJornadaLaboral_Obtener", new SqlParameter("@JornadaLaboralUsuarioId", jornadaLaboralUsuarioId));

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            JornadaLaboralUsuario jornada = new JornadaLaboralUsuario();

            ValorizarEntidad(jornada, dataSet.Tables[0].Rows[0]);

            return jornada;
        }

        public List<JornadaLaboralUsuario> ListarPorUsuario(int usuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spJornadaLaboral_ListarPorUsuario", new SqlParameter("@UsuarioId", usuarioId));

            return MapearLista(dataSet);
        }

        public List<JornadaLaboralUsuario> ListarPorUsuarioYDia(int usuarioId, int diaSemana)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spJornadaLaboral_ListarPorUsuarioYDia", new SqlParameter("@UsuarioId", usuarioId), new SqlParameter("@DiaSemana", diaSemana));

            return MapearLista(dataSet);
        }

        private List<JornadaLaboralUsuario> MapearLista(DataSet dataSet)
        {
            List<JornadaLaboralUsuario> jornadas = new List<JornadaLaboralUsuario>();

            if (dataSet.Tables.Count == 0)
            {
                return jornadas;
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                JornadaLaboralUsuario jornada = new JornadaLaboralUsuario();

                ValorizarEntidad(jornada, dataRow);

                jornadas.Add(jornada);
            }

            return jornadas;
        }

        private void ValorizarEntidad(JornadaLaboralUsuario jornada, DataRow dataRow)
        {
            jornada.JornadaLaboralUsuarioId = Convert.ToInt32(dataRow["JornadaLaboralUsuarioId"]);
            jornada.UsuarioId = Convert.ToInt32(dataRow["UsuarioId"]);
            jornada.DiaSemana = Convert.ToInt32(dataRow["DiaSemana"]);
            jornada.HoraInicio = ObtenerTimeSpan(dataRow["HoraInicio"]);
            jornada.HoraFin = ObtenerTimeSpan(dataRow["HoraFin"]);
            jornada.Activo = Convert.ToBoolean(dataRow["Activo"]);

            DateTime creado = Convert.ToDateTime(dataRow["CreadoUtc"]);
            DateTime actualizado = Convert.ToDateTime(dataRow["ActualizadoUtc"]);

            jornada.CreadoUtc = DateTime.SpecifyKind(creado, DateTimeKind.Utc);
            jornada.ActualizadoUtc = DateTime.SpecifyKind(actualizado, DateTimeKind.Utc);
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