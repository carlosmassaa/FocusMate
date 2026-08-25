using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PlanificacionDAL
    {
        public PlanificacionDAL()
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

        public int GuardarNueva(Planificacion planificacion)
        {
            if (planificacion == null)
            {
                throw new ArgumentNullException(nameof(planificacion));
            }

            List<SqlParameter> parametros = new List<SqlParameter> {new SqlParameter("@UsuarioId", planificacion.UsuarioId),new SqlParameter("@SupervisorId", (object)planificacion.SupervisorId ?? DBNull.Value),new SqlParameter("@FechaGeneracionUtc", planificacion.FechaGeneracionUtc),new SqlParameter("@Estado", (int)planificacion.Estado),new SqlParameter("@ObservacionRevision", (object)planificacion.ObservacionRevision ?? DBNull.Value),new SqlParameter("@ObservacionAprobacion", (object)planificacion.ObservacionAprobacion ?? DBNull.Value)};

            AccesoBD accesoBD = new AccesoBD();
            object resultado = accesoBD.ExecuteScalarSp("spPlanificacion_Insertar", parametros.ToArray());

            int planificacionId;

            if (resultado != null && resultado != DBNull.Value && int.TryParse(resultado.ToString(), out planificacionId) && planificacionId > 0)
            {
                planificacion.PlanificacionId = planificacionId;
                return planificacionId;
            }

            return 0;
        }

        public void Actualizar(Planificacion planificacion)
        {
            if (planificacion == null)
            {
                throw new ArgumentNullException(nameof(planificacion));
            }

            List<SqlParameter> parametros = new List<SqlParameter> {new SqlParameter("@PlanificacionId", planificacion.PlanificacionId),new SqlParameter("@UsuarioId", planificacion.UsuarioId),new SqlParameter("@SupervisorId", (object)planificacion.SupervisorId ?? DBNull.Value),new SqlParameter("@FechaGeneracionUtc", planificacion.FechaGeneracionUtc),new SqlParameter("@FechaRevisionUtc", ToDbDateTime(planificacion.FechaRevisionUtc)),new SqlParameter("@FechaAprobacionUtc", ToDbDateTime(planificacion.FechaAprobacionUtc)),new SqlParameter("@Estado", (int)planificacion.Estado),new SqlParameter("@ObservacionRevision", (object)planificacion.ObservacionRevision ?? DBNull.Value),new SqlParameter("@ObservacionAprobacion", (object)planificacion.ObservacionAprobacion ?? DBNull.Value)};

            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spPlanificacion_Actualizar", parametros.ToArray());
        }

        public Planificacion Obtener(int planificacionId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spPlanificacion_Obtener", new SqlParameter("@PlanificacionId", planificacionId));

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            Planificacion planificacion = new Planificacion();
            ValorizarEntidad(planificacion, dataSet.Tables[0].Rows[0]);

            planificacion.Detalles = ListarDetallesPorPlanificacion(planificacionId);

            return planificacion;
        }

        public List<Planificacion> ListarDisponibles()
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spPlanificacion_ListarDisponibles");

            return MapearLista(dataSet);
        }

        public List<Planificacion> ListarPorUsuario(int usuarioId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spPlanificacion_ListarPorUsuario", new SqlParameter("@UsuarioId", usuarioId));

            return MapearLista(dataSet);
        }

        public void RegistrarRevision(int planificacionId, int supervisorId, string observacionRevision, EstadoPlanificacion estado)
        {
             List<SqlParameter> parametros = new List<SqlParameter> {new SqlParameter("@PlanificacionId", planificacionId),new SqlParameter("@SupervisorId", supervisorId),new SqlParameter("@ObservacionRevision", (object)observacionRevision ?? DBNull.Value),new SqlParameter("@FechaRevisionUtc", DateTime.UtcNow),new SqlParameter("@Estado", (int)estado)};

            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spPlanificacion_RegistrarRevision", parametros.ToArray());
        }

        public void Aprobar(int planificacionId, int supervisorId, string observacionAprobacion)
        {
            List<SqlParameter> parametros = new List<SqlParameter> {new SqlParameter("@PlanificacionId", planificacionId),new SqlParameter("@SupervisorId", supervisorId),new SqlParameter("@ObservacionAprobacion", (object)observacionAprobacion ?? DBNull.Value),new SqlParameter("@FechaAprobacionUtc", DateTime.UtcNow)};

            AccesoBD accesoBD = new AccesoBD();
            accesoBD.ExecuteNonQuerySp("spPlanificacion_Aprobar", parametros.ToArray());
        }

        public int GuardarDetalle(PlanificacionDetalle detalle)
        {
            if (detalle == null)
            {
                throw new ArgumentNullException(nameof(detalle));
            }

            List<SqlParameter> parametros = new List<SqlParameter> {new SqlParameter("@PlanificacionId", detalle.PlanificacionId),new SqlParameter("@TareaId", detalle.TareaId),new SqlParameter("@Orden", detalle.Orden),new SqlParameter("@ScorePrioridad", detalle.ScorePrioridad)};

            AccesoBD accesoBD = new AccesoBD();
            object resultado = accesoBD.ExecuteScalarSp("spPlanificacionDetalle_Guardar", parametros.ToArray());

            int planificacionDetalleId;

            if (resultado != null && resultado != DBNull.Value && int.TryParse(resultado.ToString(), out planificacionDetalleId) && planificacionDetalleId > 0)
            {
                detalle.PlanificacionDetalleId = planificacionDetalleId;
                return planificacionDetalleId;
            }

            return 0;
        }

        public List<PlanificacionDetalle> ListarDetallesPorPlanificacion(int planificacionId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spPlanificacionDetalle_ListarPorPlanificacion", new SqlParameter("@PlanificacionId", planificacionId));

            List<PlanificacionDetalle> detalles = new List<PlanificacionDetalle>();

            if (dataSet.Tables.Count == 0)
            {
                return detalles;
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                PlanificacionDetalle detalle = new PlanificacionDetalle();
                ValorizarDetalle(detalle, dataRow);
                detalles.Add(detalle);
            }

            return detalles;
        }

        public List<PlanificacionDetalle> ListarDetallesConTareaPorPlanificacion(int planificacionId)
        {
            AccesoBD accesoBD = new AccesoBD();
            DataSet dataSet = accesoBD.ExecuteDataSetSp("spPlanificacionDetalle_ListarConTareaPorPlanificacion", new SqlParameter("@PlanificacionId", planificacionId));

            List<PlanificacionDetalle> detalles = new List<PlanificacionDetalle>();

            if (dataSet.Tables.Count == 0)
            {
                return detalles;
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                PlanificacionDetalle detalle = new PlanificacionDetalle();
                ValorizarDetalle(detalle, dataRow);

                Tarea tarea = new Tarea();
                ValorizarTarea(tarea, dataRow);
                detalle.Tarea = tarea;

                detalles.Add(detalle);
            }

            return detalles;
        }

        private List<Planificacion> MapearLista(DataSet dataSet)
        {
            List<Planificacion> planificaciones = new List<Planificacion>();

            if (dataSet.Tables.Count == 0)
            {
                return planificaciones;
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                Planificacion planificacion = new Planificacion();
                ValorizarEntidad(planificacion, dataRow);
                planificaciones.Add(planificacion);
            }

            return planificaciones;
        }

        private void ValorizarEntidad(Planificacion planificacion, DataRow dataRow)
        {
            planificacion.PlanificacionId = Convert.ToInt32(dataRow["PlanificacionId"]);
            planificacion.UsuarioId = Convert.ToInt32(dataRow["UsuarioId"]);

            if (dataRow["SupervisorId"] == DBNull.Value)
            {
                planificacion.SupervisorId = null;
            }
            else
            {
                planificacion.SupervisorId = Convert.ToInt32(dataRow["SupervisorId"]);
            }

            planificacion.FechaGeneracionUtc = DateTime.SpecifyKind(Convert.ToDateTime(dataRow["FechaGeneracionUtc"]), DateTimeKind.Utc);

            if (dataRow["FechaRevisionUtc"] == DBNull.Value)
            {
                planificacion.FechaRevisionUtc = null;
            }
            else
            {
                planificacion.FechaRevisionUtc = DateTime.SpecifyKind(Convert.ToDateTime(dataRow["FechaRevisionUtc"]), DateTimeKind.Utc);
            }

            if (dataRow["FechaAprobacionUtc"] == DBNull.Value)
            {
                planificacion.FechaAprobacionUtc = null;
            }
            else
            {
                planificacion.FechaAprobacionUtc = DateTime.SpecifyKind(Convert.ToDateTime(dataRow["FechaAprobacionUtc"]), DateTimeKind.Utc);
            }

            planificacion.Estado = (EstadoPlanificacion)Convert.ToInt32(dataRow["Estado"]);

            if (dataRow["ObservacionRevision"] == DBNull.Value)
            {
                planificacion.ObservacionRevision = null;
            }
            else
            {
                planificacion.ObservacionRevision = dataRow["ObservacionRevision"].ToString();
            }

            if (dataRow["ObservacionAprobacion"] == DBNull.Value)
            {
                planificacion.ObservacionAprobacion = null;
            }
            else
            {
                planificacion.ObservacionAprobacion = dataRow["ObservacionAprobacion"].ToString();
            }
        }

        private void ValorizarDetalle(PlanificacionDetalle detalle, DataRow dataRow)
        {
            detalle.PlanificacionDetalleId = Convert.ToInt32(dataRow["PlanificacionDetalleId"]);
            detalle.PlanificacionId = Convert.ToInt32(dataRow["PlanificacionId"]);
            detalle.TareaId = Convert.ToInt32(dataRow["TareaId"]);
            detalle.Orden = Convert.ToInt32(dataRow["Orden"]);
            detalle.ScorePrioridad = Convert.ToDecimal(dataRow["ScorePrioridad"]);
        }

        private void ValorizarTarea(Tarea tarea, DataRow dataRow)
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

            if (dataRow.Table.Columns.Contains("DVH") && dataRow["DVH"] != DBNull.Value)
            {
                tarea.DVH = Convert.ToInt64(dataRow["DVH"]);
            }
            else
            {
                tarea.DVH = 0L;
            }
        }
    }
}
