using BE;
using DAL;
using Servicioss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class PlanificacionBL
    {
        private readonly PlanificacionDAL _planificacionDal;
        private readonly TareaBL _tareaBL;
        private readonly BitacoraBL _bitacora;

        public PlanificacionBL()
        {
            _planificacionDal = new PlanificacionDAL();
            _tareaBL = new TareaBL();
            _bitacora = BitacoraBL.CrearBasico();
        }

        public Planificacion GenerarPlanificacion(int usuarioId)
        {
            ValidarUsuarioParaGenerarPlanificacion(usuarioId);

            List<Tarea> tareasUsuario = ObtenerTareasUsuario(usuarioId);
            List<Tarea> tareasActivas = CrearListaTareasActivas();

            ProcesarTareasUsuario(tareasUsuario, tareasActivas);

            ValidarExistenciaTareasActivas(tareasActivas);

            OrdenarTareasPriorizadas(tareasActivas);

            Planificacion planificacion = CrearPlanificacionGenerada(usuarioId);

            int planificacionId = GuardarPlanificacionGenerada(planificacion);

            GuardarDetallesTop10(planificacionId, tareasActivas, planificacion);

            RegistrarBitacoraGeneracion(planificacionId, usuarioId);

            return ObtenerPlanificacionGenerada(planificacionId);
        }

        public Planificacion Obtener(int planificacionId)
        {
            ValidarPlanificacionId(planificacionId);

            Planificacion planificacion = ObtenerPlanificacionDesdeRepositorio(planificacionId);

            if (planificacion == null)
            {
                return null;
            }

            CargarDetallesConTarea(planificacion);

            return planificacion;
        }

        public List<Planificacion> ListarDisponibles()
        {
            return ObtenerPlanificacionesDisponibles();
        }

        public List<Planificacion> ListarPorUsuario(int usuarioId)
        {
            ValidarUsuarioParaListado(usuarioId);

            return ObtenerPlanificacionesPorUsuario(usuarioId);
        }

        public void RegistrarRevision(int planificacionId, int supervisorId, string observacionRevision, bool planificacionAdecuada)
        {
            ValidarDatosRevisionPlanificacion(planificacionId, supervisorId);

            Planificacion planificacion = ObtenerPlanificacionParaRevision(planificacionId);

            ValidarPlanificacionParaRevision(planificacion);

            EstadoPlanificacion estadoRevision = DeterminarEstadoRevision(planificacionAdecuada);

            GuardarRevisionPlanificacion(planificacionId, supervisorId, observacionRevision, estadoRevision);

            RegistrarBitacoraRevision(planificacionId, supervisorId);
        }

        public void AprobarPlanificacion(int planificacionId, int supervisorId, string observacionAprobacion)
        {
            ValidarDatosAprobacionPlanificacion(planificacionId, supervisorId);

            Planificacion planificacion = ObtenerPlanificacionParaAprobacion(planificacionId);

            ValidarPlanificacionDisponible(planificacion);

            ValidarPlanificacionRevisada(planificacion);

            GuardarAprobacionPlanificacion(planificacionId, supervisorId, observacionAprobacion);

            RegistrarBitacoraAprobacion(planificacionId, supervisorId);
        }

        public Planificacion ObtenerUltimaAprobadaPorUsuario(int usuarioId)
        {
            ValidarUsuarioParaConsultaAprobada(usuarioId);

            List<Planificacion> planificaciones = ObtenerPlanificacionesAprobadasPorUsuario(usuarioId);

            Planificacion planificacionAprobada = BuscarUltimaPlanificacionAprobada(planificaciones);

            if (planificacionAprobada == null)
            {
                return null;
            }

            return ObtenerPlanificacionAprobadaConDetalle(planificacionAprobada.PlanificacionId);
        }

        private void ValidarPlanificacionId(int planificacionId)
        {
            if (planificacionId <= 0)
            {
                throw new ArgumentException("PlanificacionId inválido.", nameof(planificacionId));
            }
        }

        private Planificacion ObtenerPlanificacionDesdeRepositorio(int planificacionId)
        {
            return _planificacionDal.Obtener(planificacionId);
        }

        private void CargarDetallesConTarea(Planificacion planificacion)
        {
            planificacion.Detalles = _planificacionDal.ListarDetallesConTareaPorPlanificacion(planificacion.PlanificacionId);
        }

        private List<Planificacion> ObtenerPlanificacionesDisponibles()
        {
            return _planificacionDal.ListarDisponibles();
        }

        private void ValidarUsuarioParaListado(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(usuarioId));
            }
        }

        private List<Planificacion> ObtenerPlanificacionesPorUsuario(int usuarioId)
        {
            return _planificacionDal.ListarPorUsuario(usuarioId);
        }

        private void ValidarDatosRevisionPlanificacion(int planificacionId, int supervisorId)
        {
            if (planificacionId <= 0)
            {
                throw new ArgumentException("PlanificacionId inválido.", nameof(planificacionId));
            }

            if (supervisorId <= 0)
            {
                throw new ArgumentException("SupervisorId inválido.", nameof(supervisorId));
            }
        }

        private Planificacion ObtenerPlanificacionParaRevision(int planificacionId)
        {
            return _planificacionDal.Obtener(planificacionId);
        }

        private void ValidarPlanificacionParaRevision(Planificacion planificacion)
        {
            if (planificacion == null)
            {
                throw new InvalidOperationException("La planificación no existe.");
            }

            if (planificacion.Estado == EstadoPlanificacion.Aprobada)
            {
                throw new InvalidOperationException("La planificación ya se encuentra aprobada.");
            }
        }

        private EstadoPlanificacion DeterminarEstadoRevision(bool planificacionAdecuada)
        {
            EstadoPlanificacion estadoRevision;

            if (planificacionAdecuada)
            {
                estadoRevision = EstadoPlanificacion.Revisada;
            }
            else
            {
                estadoRevision = EstadoPlanificacion.Observada;
            }

            return estadoRevision;
        }

        private void GuardarRevisionPlanificacion(int planificacionId, int supervisorId, string observacionRevision, EstadoPlanificacion estadoRevision)
        {
            _planificacionDal.RegistrarRevision(planificacionId, supervisorId, observacionRevision, estadoRevision);
        }

        private void RegistrarBitacoraRevision(int planificacionId, int supervisorId)
        {
            string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
            _bitacora.Registrar("REVISAR_PLANIFICACION", "Planificacion", "OK", usuario, "Planificacion", "PlanificacionId=" + planificacionId + "; SupervisorId=" + supervisorId);
        }

        private void ValidarDatosAprobacionPlanificacion(int planificacionId, int supervisorId)
        {
            if (planificacionId <= 0)
            {
                throw new ArgumentException("PlanificacionId inválido.", nameof(planificacionId));
            }

            if (supervisorId <= 0)
            {
                throw new ArgumentException("SupervisorId inválido.", nameof(supervisorId));
            }
        }

        private Planificacion ObtenerPlanificacionParaAprobacion(int planificacionId)
        {
            return _planificacionDal.Obtener(planificacionId);
        }

        private void ValidarPlanificacionDisponible(Planificacion planificacion)
        {
            if (planificacion == null)
            {
                throw new InvalidOperationException("La planificación no existe.");
            }
        }

        private void ValidarPlanificacionRevisada(Planificacion planificacion)
        {
            if (planificacion.Estado == EstadoPlanificacion.Aprobada)
            {
                throw new InvalidOperationException("La planificación ya se encuentra aprobada.");
            }

            if (planificacion.Estado != EstadoPlanificacion.Revisada)
            {
                throw new InvalidOperationException("La planificación debe estar revisada antes de aprobarse.");
            }
        }

        private void GuardarAprobacionPlanificacion(int planificacionId, int supervisorId, string observacionAprobacion)
        {
            _planificacionDal.Aprobar(planificacionId, supervisorId, observacionAprobacion);
        }

        private void RegistrarBitacoraAprobacion(int planificacionId, int supervisorId)
        {
            string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
            _bitacora.Registrar("APROBAR_PLANIFICACION", "Planificacion", "OK", usuario, "Planificacion", "PlanificacionId=" + planificacionId + "; SupervisorId=" + supervisorId);
        }

        private void ValidarUsuarioParaGenerarPlanificacion(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(usuarioId));
            }
        }

        private List<Tarea> ObtenerTareasUsuario(int usuarioId)
        {
            return _tareaBL.ListarPorUsuario(usuarioId);
        }

        private List<Tarea> CrearListaTareasActivas()
        {
            return new List<Tarea>();
        }

        private void ProcesarTareasUsuario(List<Tarea> tareasUsuario, List<Tarea> tareasActivas)
        {
            foreach (Tarea tarea in tareasUsuario)
            {
                if (EsTareaActiva(tarea))
                {
                    AgregarTareaActivaPriorizada(tareasActivas, tarea);
                }
            }
        }

        private bool EsTareaActiva(Tarea tarea)
        {
            return tarea.Estado == EstadoTarea.Pendiente || tarea.Estado == EstadoTarea.EnCurso || tarea.Estado == EstadoTarea.Pausada;
        }

        private void AgregarTareaActivaPriorizada(List<Tarea> tareasActivas, Tarea tarea)
        {
            ValidarTareaParaPlanificacion(tarea);
            decimal score = CalcularScoreTarea(tarea);
            AsignarScoreYAgregarTareaActiva(tareasActivas, tarea, score);
        }

        private decimal CalcularScoreTarea(Tarea tarea)
        {
            return _tareaBL.CalcularScore(tarea);
        }

        private void AsignarScoreYAgregarTareaActiva(List<Tarea> tareasActivas, Tarea tarea, decimal score)
        {
            tarea.ScorePrioridad = score;
            tareasActivas.Add(tarea);
        }

        private void ValidarExistenciaTareasActivas(List<Tarea> tareasActivas)
        {
            if (tareasActivas.Count == 0)
            {
                throw new InvalidOperationException("No existen tareas activas para generar la planificación.");
            }
        }

        private void OrdenarTareasPriorizadas(List<Tarea> tareasActivas)
        {
            tareasActivas.Sort(CompararTareasPriorizadas);
        }

        private Planificacion CrearPlanificacionGenerada(int usuarioId)
        {
            Planificacion planificacion = new Planificacion { UsuarioId = usuarioId, SupervisorId = null, FechaGeneracionUtc = DateTime.UtcNow, FechaRevisionUtc = null, FechaAprobacionUtc = null, Estado = EstadoPlanificacion.Generada, ObservacionRevision = null, ObservacionAprobacion = null, Detalles = new List<PlanificacionDetalle>() };

            return planificacion;
        }

        private int GuardarPlanificacionGenerada(Planificacion planificacion)
        {
            int planificacionId = _planificacionDal.GuardarNueva(planificacion);

            if (planificacionId <= 0)
            {
                throw new InvalidOperationException("No fue posible generar la planificación.");
            }

            return planificacionId;
        }

        private void GuardarDetallesTop10(int planificacionId, List<Tarea> tareasActivas, Planificacion planificacion)
        {
            int orden = 1;

            foreach (Tarea tarea in tareasActivas)
            {
                if (orden <= 10)
                {
                    PlanificacionDetalle detalle = CrearDetallePlanificacion(planificacionId, tarea, orden);
                    GuardarDetallePlanificacion(detalle);
                    AgregarDetalleAPlanificacion(planificacion, detalle);
                    orden++;
                }
            }
        }

        private PlanificacionDetalle CrearDetallePlanificacion(int planificacionId, Tarea tarea, int orden)
        {
            PlanificacionDetalle detalle = new PlanificacionDetalle { PlanificacionId = planificacionId, TareaId = tarea.TareaId, Orden = orden, ScorePrioridad = tarea.ScorePrioridad, Tarea = tarea };

            return detalle;
        }

        private void GuardarDetallePlanificacion(PlanificacionDetalle detalle)
        {
            _planificacionDal.GuardarDetalle(detalle);
        }

        private void AgregarDetalleAPlanificacion(Planificacion planificacion, PlanificacionDetalle detalle)
        {
            planificacion.Detalles.Add(detalle);
        }

        private void RegistrarBitacoraGeneracion(int planificacionId, int usuarioId)
        {
            string usuario = ObtenerNombreUsuarioActual();
            _bitacora.Registrar("GENERAR_PLANIFICACION", "Planificacion", "OK", usuario, "Planificacion", "PlanificacionId=" + planificacionId + "; UsuarioId=" + usuarioId);
        }

        private string ObtenerNombreUsuarioActual()
        {
            return SesionActual.Instance.NombreUsuario ?? string.Empty;
        }

        private Planificacion ObtenerPlanificacionGenerada(int planificacionId)
        {
            return Obtener(planificacionId);
        }

        private void ValidarTareaParaPlanificacion(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            if (tarea.TareaId <= 0)
            {
                throw new ArgumentException("TareaId inválido.", nameof(tarea.TareaId));
            }

            if (tarea.UsuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(tarea.UsuarioId));
            }

            if (string.IsNullOrWhiteSpace(tarea.Titulo))
            {
                throw new ArgumentException("Existen tareas sin título.");
            }

            if (!Enum.IsDefined(typeof(ImportanciaTarea), tarea.Importancia))
            {
                throw new ArgumentException("Existen tareas con importancia inválida.");
            }

            if (!Enum.IsDefined(typeof(EnergiaRequeridaTarea), tarea.EnergiaRequerida))
            {
                throw new ArgumentException("Existen tareas con energía requerida inválida.");
            }

            if (!Enum.IsDefined(typeof(EstadoTarea), tarea.Estado))
            {
                throw new ArgumentException("Existen tareas con estado inválido.");
            }

            if (tarea.DuracionEstimadaMin <= 0 || tarea.DuracionEstimadaMin > 14400)
            {
                throw new ArgumentException("Existen tareas con duración estimada inválida.");
            }

            if (tarea.FechaLimite.HasValue && tarea.FechaLimite.Value.Date < DateTime.Today)
            {
                throw new ArgumentException("Existen tareas con fecha límite anterior a hoy.");
            }
        }

        private void ValidarUsuarioParaConsultaAprobada(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(usuarioId));
            }
        }

        private List<Planificacion> ObtenerPlanificacionesAprobadasPorUsuario(int usuarioId)
        {
            return _planificacionDal.ListarPorUsuario(usuarioId);
        }

        private Planificacion BuscarUltimaPlanificacionAprobada(List<Planificacion> planificaciones)
        {
            foreach (Planificacion planificacion in planificaciones)
            {
                if (planificacion.Estado == EstadoPlanificacion.Aprobada)
                {
                    return planificacion;
                }
            }

            return null;
        }

        private Planificacion ObtenerPlanificacionAprobadaConDetalle(int planificacionId)
        {
            return Obtener(planificacionId);
        }

        private int CompararTareasPriorizadas(Tarea primeraTarea, Tarea segundaTarea)
        {
            int comparacionScore = segundaTarea.ScorePrioridad.CompareTo(primeraTarea.ScorePrioridad);

            if (comparacionScore != 0)
            {
                return comparacionScore;
            }

            DateTime fechaPrimera;

            if (primeraTarea.FechaLimite.HasValue)
            {
                fechaPrimera = primeraTarea.FechaLimite.Value;
            }
            else
            {
                fechaPrimera = DateTime.MaxValue;
            }

            DateTime fechaSegunda;

            if (segundaTarea.FechaLimite.HasValue)
            {
                fechaSegunda = segundaTarea.FechaLimite.Value;
            }
            else
            {
                fechaSegunda = DateTime.MaxValue;
            }

            int comparacionFecha = fechaPrimera.CompareTo(fechaSegunda);

            if (comparacionFecha != 0)
            {
                return comparacionFecha;
            }

            int comparacionImportancia = segundaTarea.Importancia.CompareTo(primeraTarea.Importancia);

            if (comparacionImportancia != 0)
            {
                return comparacionImportancia;
            }

            int comparacionDuracion = primeraTarea.DuracionEstimadaMin.CompareTo(segundaTarea.DuracionEstimadaMin);

            if (comparacionDuracion != 0)
            {
                return comparacionDuracion;
            }

            return primeraTarea.TareaId.CompareTo(segundaTarea.TareaId);
        }
    }
}
