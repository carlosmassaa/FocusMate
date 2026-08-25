using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class TareaHistorialDAL
    {
        public TareaHistorialDAL()
        {
        }

        private object ToDbDateTime(DateTime value)
        {
            DateTime minDateTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            if (value < minDateTime)
            {
                return DBNull.Value;
            }
            else
            {
                return value;
            }
        }

        public int Guardar(TareaHistorialEntry historialEntry)
        {
            if (historialEntry == null) throw new ArgumentNullException(nameof(historialEntry));

            List<SqlParameter> parametros = new List<SqlParameter>();

            if (historialEntry.HistorialId != 0)
            {
                parametros.Add(new SqlParameter("@HistorialId", historialEntry.HistorialId));
            }

            parametros.AddRange(new[] { new SqlParameter("@TareaId", historialEntry.TareaId), new SqlParameter("@UsuarioOperacionId", historialEntry.UsuarioOperacionId), new SqlParameter("@FechaUtc", ToDbDateTime(historialEntry.FechaUtc)), new SqlParameter("@Accion", (object)historialEntry.Accion ?? DBNull.Value), new SqlParameter("@ProyectoId", (object)historialEntry.ProyectoId ?? DBNull.Value), new SqlParameter("@Titulo", (object)historialEntry.Titulo ?? DBNull.Value), new SqlParameter("@Descripcion", (object)historialEntry.Descripcion ?? DBNull.Value), new SqlParameter("@FechaLimite", (object)historialEntry.FechaLimite ?? DBNull.Value), new SqlParameter("@Importancia", (int)historialEntry.Importancia), new SqlParameter("@EnergiaRequerida", (int)historialEntry.EnergiaRequerida), new SqlParameter("@DuracionEstimadaMin", historialEntry.DuracionEstimadaMin), new SqlParameter("@ScorePrioridad", historialEntry.ScorePrioridad), new SqlParameter("@Estado", (int)historialEntry.Estado), new SqlParameter("@UsuarioIdPropietario", historialEntry.UsuarioIdPropietario) });

            AccesoBD accesoBD = new AccesoBD();
            object resultadoEscalar = accesoBD.ExecuteScalarSp("spTareaHistorial_Guardar", parametros.ToArray());
            if (resultadoEscalar != null && resultadoEscalar != DBNull.Value)
            {
                int idSalida;
                if (int.TryParse(resultadoEscalar.ToString(), out idSalida) && idSalida > 0)
                {
                    historialEntry.HistorialId = idSalida;
                    return idSalida;
                }
            }

            int filasAfectadas = accesoBD.ExecuteNonQuerySp("spTareaHistorial_Guardar", parametros.ToArray());
            if (historialEntry.HistorialId != 0)
            {
                return historialEntry.HistorialId;
            }
            else
            {
                return filasAfectadas;
            }
        }

        public List<TareaHistorialEntry> ListarPorTarea(int tareaId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spTareaHistorial_ListarPorTarea", new SqlParameter("@TareaId", tareaId));

            List<TareaHistorialEntry> listaHistorial = new List<TareaHistorialEntry>();
            if (dataSet.Tables.Count == 0) return listaHistorial;

            foreach (DataRow fila in dataSet.Tables[0].Rows)
            {
                TareaHistorialEntry historial = new TareaHistorialEntry();
                Valorizar(historial, fila);
                listaHistorial.Add(historial);
            }
            return listaHistorial;
        }

        public TareaHistorialEntry Obtener(int historialId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spTareaHistorial_Obtener", new SqlParameter("@HistorialId", historialId));

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0) return null;

            TareaHistorialEntry historial = new TareaHistorialEntry();
            Valorizar(historial, dataSet.Tables[0].Rows[0]);
            return historial;
        }

        private void Valorizar(TareaHistorialEntry historialEntry, DataRow dataRow)
        {
            historialEntry.HistorialId = Convert.ToInt32(dataRow["HistorialId"]);
            historialEntry.TareaId = Convert.ToInt32(dataRow["TareaId"]);
            historialEntry.UsuarioOperacionId = Convert.ToInt32(dataRow["UsuarioOperacionId"]);
            historialEntry.FechaUtc = Convert.ToDateTime(dataRow["FechaUtc"]);

            if (dataRow["Accion"] == DBNull.Value)
            {
                historialEntry.Accion = null;
            }
            else
            {
                historialEntry.Accion = dataRow["Accion"].ToString();
            }

            if (dataRow["ProyectoId"] == DBNull.Value)
            {
                historialEntry.ProyectoId = null;
            }
            else
            {
                historialEntry.ProyectoId = Convert.ToInt32(dataRow["ProyectoId"]);
            }

            if (dataRow["Titulo"] == DBNull.Value)
            {
                historialEntry.Titulo = null;
            }
            else
            {
                historialEntry.Titulo = dataRow["Titulo"].ToString();
            }

            if (dataRow["Descripcion"] == DBNull.Value)
            {
                historialEntry.Descripcion = null;
            }
            else
            {
                historialEntry.Descripcion = dataRow["Descripcion"].ToString();
            }

            if (dataRow["FechaLimite"] == DBNull.Value)
            {
                historialEntry.FechaLimite = null;
            }
            else
            {
                historialEntry.FechaLimite = Convert.ToDateTime(dataRow["FechaLimite"]);
            }

            historialEntry.Importancia = (ImportanciaTarea)Convert.ToInt32(dataRow["Importancia"]);
            historialEntry.EnergiaRequerida = (EnergiaRequeridaTarea)Convert.ToInt32(dataRow["EnergiaRequerida"]);
            historialEntry.DuracionEstimadaMin = Convert.ToInt32(dataRow["DuracionEstimadaMin"]);
            historialEntry.ScorePrioridad = Convert.ToDecimal(dataRow["ScorePrioridad"]);
            historialEntry.Estado = (EstadoTarea)Convert.ToInt32(dataRow["Estado"]);
            historialEntry.UsuarioIdPropietario = Convert.ToInt32(dataRow["UsuarioIdPropietario"]);

        }
    }
}
