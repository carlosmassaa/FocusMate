using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class TareaDAL
    {
        public TareaDAL()
        {
        }

        private object ToDbDateTime(DateTime? value)
        {
            if (!value.HasValue)
            {
                return DBNull.Value;
            }

            DateTime minDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            if (value.Value < minDate)
            {
                return DBNull.Value;
            }
            else
            {
                return value.Value;
            }
        }

        public int GuardarNuevo(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            List<SqlParameter> pars = new List<SqlParameter> {new SqlParameter("@UsuarioId", tarea.UsuarioId),new SqlParameter("@ProyectoId", (object)tarea.ProyectoId ?? DBNull.Value), new SqlParameter("@Titulo", tarea.Titulo ?? string.Empty), new SqlParameter("@Descripcion", (object)tarea.Descripcion ?? DBNull.Value), new SqlParameter("@FechaLimite", ToDbDateTime(tarea.FechaLimite)), new SqlParameter("@Importancia", (int)tarea.Importancia), new SqlParameter("@EnergiaRequerida", (int)tarea.EnergiaRequerida), new SqlParameter("@DuracionEstimadaMin", tarea.DuracionEstimadaMin), new SqlParameter("@ScorePrioridad", tarea.ScorePrioridad), new SqlParameter("@Estado", (int)tarea.Estado), new SqlParameter("@CreadoUtc", tarea.CreadoUtc), new SqlParameter("@ActualizadoUtc", tarea.ActualizadoUtc), new SqlParameter("@DVH", tarea.DVH) };

            AccesoBD accesoBD = new AccesoBD();
            object esc = accesoBD.ExecuteScalarSp("spTarea_Insertar", pars.ToArray());
            int id;
            if (esc != null && esc != DBNull.Value && int.TryParse(esc.ToString(), out id) && id > 0)
            {
                tarea.TareaId = id;
                return id;
            }

            int rows = accesoBD.ExecuteNonQuerySp("spTarea_Insertar", pars.ToArray());
            return rows;
        }

        public void Guardar(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            List<SqlParameter> pars = new List<SqlParameter> {new SqlParameter("@TareaId", tarea.TareaId), new SqlParameter("@UsuarioId", tarea.UsuarioId), new SqlParameter("@ProyectoId", (object)tarea.ProyectoId ?? DBNull.Value), new SqlParameter("@Titulo", tarea.Titulo ?? string.Empty), new SqlParameter("@Descripcion", (object)tarea.Descripcion ?? DBNull.Value), new SqlParameter("@FechaLimite", ToDbDateTime(tarea.FechaLimite)), new SqlParameter("@Importancia", (int)tarea.Importancia), new SqlParameter("@EnergiaRequerida", (int)tarea.EnergiaRequerida), new SqlParameter("@DuracionEstimadaMin", tarea.DuracionEstimadaMin), new SqlParameter("@ScorePrioridad", tarea.ScorePrioridad), new SqlParameter("@Estado", (int)tarea.Estado), new SqlParameter("@ActualizadoUtc", tarea.ActualizadoUtc), new SqlParameter("@DVH", tarea.DVH)};

            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spTarea_Actualizar", pars.ToArray());
        }

        public Tarea Obtener(int tareaId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spTarea_Obtener", new SqlParameter("@TareaId", tareaId));
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            Tarea tarea = new Tarea();
            ValorizarEntidad(tarea, dataSet.Tables[0].Rows[0]);
            return tarea;
        }

        public List<Tarea> Listar()
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spTarea_Listar");
            return MapearLista(dataSet);
        }

        public List<Tarea> ListarPorUsuario(int usuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spTarea_ListarPorUsuario", new SqlParameter("@UsuarioId", usuarioId));
            return MapearLista(dataSet);
        }

        public void Eliminar(int tareaId)
        {
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spTarea_Eliminar", new SqlParameter("@TareaId", tareaId));
        }

        private List<Tarea> MapearLista(DataSet dataSet)
        {
            List<Tarea> tareas = new List<Tarea>();
            if (dataSet.Tables.Count == 0)
            {
                return tareas;
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                Tarea tarea = new Tarea();
                ValorizarEntidad(tarea, dataRow);
                tareas.Add(tarea);
            }

            return tareas;
        }

        internal void ValorizarEntidad(Tarea tarea, DataRow dataRow)
        {
            tarea.TareaId = Convert.ToInt32(dataRow["TareaId"]);
            tarea.UsuarioId = Convert.ToInt32(dataRow["UsuarioId"]);

            if (dataRow["ProyectoId"] == DBNull.Value)
            {
                tarea.ProyectoId = null;
            }
            else
            {
                tarea.ProyectoId = Convert.ToInt32(dataRow["ProyectoId"]);
            }

            if (dataRow["Titulo"] == DBNull.Value)
            {
                tarea.Titulo = null;
            }
            else
            {
                tarea.Titulo = dataRow["Titulo"].ToString();
            }

            if (dataRow["Descripcion"] == DBNull.Value)
            {
                tarea.Descripcion = null;
            }
            else
            {
                tarea.Descripcion = dataRow["Descripcion"].ToString();
            }

            if (dataRow["FechaLimite"] == DBNull.Value)
            {
                tarea.FechaLimite = null;
            }
            else
            {
                tarea.FechaLimite = Convert.ToDateTime(dataRow["FechaLimite"]);
            }

            tarea.Importancia = (ImportanciaTarea)Convert.ToInt32(dataRow["Importancia"]);
            tarea.EnergiaRequerida = (EnergiaRequeridaTarea)Convert.ToInt32(dataRow["EnergiaRequerida"]);
            tarea.DuracionEstimadaMin = Convert.ToInt32(dataRow["DuracionEstimadaMin"]);
            tarea.ScorePrioridad = Convert.ToDecimal(dataRow["ScorePrioridad"]);
            tarea.Estado = (EstadoTarea)Convert.ToInt32(dataRow["Estado"]);

            DateTime creado = Convert.ToDateTime(dataRow["CreadoUtc"]);
            DateTime actualizado = Convert.ToDateTime(dataRow["ActualizadoUtc"]);
            tarea.CreadoUtc = DateTime.SpecifyKind(creado, DateTimeKind.Utc);
            tarea.ActualizadoUtc = DateTime.SpecifyKind(actualizado, DateTimeKind.Utc);

            if (dataRow.Table.Columns.Contains("DVH"))
            {
                if (dataRow["DVH"] != DBNull.Value)
                {
                    tarea.DVH = Convert.ToInt64(dataRow["DVH"]);
                }
                else
                {
                    tarea.DVH = 0L;
                }
            }
            else
            {
                tarea.DVH = 0L;
            }

        }

        public void ActualizarDVH(int tareaId, long dvh)
        {
            SqlParameter[] parameters = new SqlParameter[]{new SqlParameter("@TareaId", tareaId),new SqlParameter("@DVH", dvh)};
            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spTarea_ActualizarDVH", parameters);
        }
    }
}
