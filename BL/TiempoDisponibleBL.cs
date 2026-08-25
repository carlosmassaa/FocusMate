using System;
using System.Collections.Generic;
using BE;
using DAL;
using Servicioss;

namespace BL
{
    public class TiempoDisponibleBL
    {
        private readonly JornadaLaboralDAL _jornadaDal;
        private readonly BloqueTiempoDAL _bloqueDal;
        private readonly BitacoraBL _bitacora;

        private const int DiasMaximosBusqueda = 14;
        private const int DuracionMinimaTareaAgendaMinutos = 10;

        public TiempoDisponibleBL()
        {
            _jornadaDal = new JornadaLaboralDAL();
            _bloqueDal = new BloqueTiempoDAL();
            _bitacora = BitacoraBL.CrearBasico();
        }

        public int CrearJornadaLaboral(JornadaLaboralUsuario jornada)
        {
            ValidarJornadaLaboral(jornada, true);

            jornada.Activo = true;
            jornada.CreadoUtc = DateTime.UtcNow;
            jornada.ActualizadoUtc = DateTime.UtcNow;

            ValidarJornadaNoSuperpuesta(jornada);

            int jornadaId = _jornadaDal.GuardarNueva(jornada);

            if (jornadaId <= 0)
            {
                throw new InvalidOperationException("No fue posible guardar la jornada laboral.");
            }

            RegistrarBitacora("CREAR_JORNADA_LABORAL", "JornadaLaboralUsuario", "JornadaLaboralUsuarioId=" + jornadaId + "; UsuarioId=" + jornada.UsuarioId);

            return jornadaId;
        }

        public void ActualizarJornadaLaboral(JornadaLaboralUsuario jornada)
        {
            ValidarJornadaLaboral(jornada, false);

            JornadaLaboralUsuario jornadaActual = _jornadaDal.Obtener(jornada.JornadaLaboralUsuarioId);

            if (jornadaActual == null)
            {
                throw new InvalidOperationException("La jornada laboral no existe.");
            }

            jornada.Activo = true;
            jornada.CreadoUtc = jornadaActual.CreadoUtc;
            jornada.ActualizadoUtc = DateTime.UtcNow;

            ValidarJornadaNoSuperpuesta(jornada);

            _jornadaDal.Guardar(jornada);

            RegistrarBitacora("ACTUALIZAR_JORNADA_LABORAL", "JornadaLaboralUsuario", "JornadaLaboralUsuarioId=" + jornada.JornadaLaboralUsuarioId + "; UsuarioId=" + jornada.UsuarioId);
        }

        public void EliminarJornadaLaboral(int jornadaLaboralUsuarioId)
        {
            if (jornadaLaboralUsuarioId <= 0)
            {
                throw new ArgumentException("JornadaLaboralUsuarioId inválido.", nameof(jornadaLaboralUsuarioId));
            }

            JornadaLaboralUsuario jornada = _jornadaDal.Obtener(jornadaLaboralUsuarioId);

            if (jornada == null)
            {
                throw new InvalidOperationException("La jornada laboral no existe.");
            }

            _jornadaDal.Eliminar(jornadaLaboralUsuarioId);

            RegistrarBitacora("ELIMINAR_JORNADA_LABORAL", "JornadaLaboralUsuario", "JornadaLaboralUsuarioId=" + jornadaLaboralUsuarioId + "; UsuarioId=" + jornada.UsuarioId);
        }

        public JornadaLaboralUsuario ObtenerJornadaLaboral(int jornadaLaboralUsuarioId)
        {
            if (jornadaLaboralUsuarioId <= 0)
            {
                throw new ArgumentException("JornadaLaboralUsuarioId inválido.", nameof(jornadaLaboralUsuarioId));
            }

            return _jornadaDal.Obtener(jornadaLaboralUsuarioId);
        }

        public List<JornadaLaboralUsuario> ListarJornadasPorUsuario(int usuarioId)
        {
            ValidarUsuario(usuarioId);

            return _jornadaDal.ListarPorUsuario(usuarioId);
        }

        public int CrearBloqueTiempo(BloqueTiempo bloque)
        {
            ValidarBloqueTiempo(bloque, true);

            bloque.Activo = true;
            bloque.CreadoUtc = DateTime.UtcNow;
            bloque.ActualizadoUtc = DateTime.UtcNow;

            ValidarBloqueDentroDeJornada(bloque);
            ValidarBloqueNoSuperpuesto(bloque);

            int bloqueId = _bloqueDal.GuardarNuevo(bloque);

            if (bloqueId <= 0)
            {
                throw new InvalidOperationException("No fue posible guardar el bloque de tiempo.");
            }

            RegistrarBitacora("CREAR_BLOQUE_TIEMPO", "BloqueTiempo", "BloqueTiempoId=" + bloqueId + "; UsuarioId=" + bloque.UsuarioId);

            return bloqueId;
        }

        public void ActualizarBloqueTiempo(BloqueTiempo bloque)
        {
            ValidarBloqueTiempo(bloque, false);

            BloqueTiempo bloqueActual = _bloqueDal.Obtener(bloque.BloqueTiempoId);

            if (bloqueActual == null)
            {
                throw new InvalidOperationException("El bloque de tiempo no existe.");
            }

            bloque.Activo = true;
            bloque.CreadoUtc = bloqueActual.CreadoUtc;
            bloque.ActualizadoUtc = DateTime.UtcNow;

            ValidarBloqueDentroDeJornada(bloque);
            ValidarBloqueNoSuperpuesto(bloque);

            _bloqueDal.Guardar(bloque);

            RegistrarBitacora("ACTUALIZAR_BLOQUE_TIEMPO", "BloqueTiempo", "BloqueTiempoId=" + bloque.BloqueTiempoId + "; UsuarioId=" + bloque.UsuarioId);
        }

        public void EliminarBloqueTiempo(int bloqueTiempoId)
        {
            if (bloqueTiempoId <= 0)
            {
                throw new ArgumentException("BloqueTiempoId inválido.", nameof(bloqueTiempoId));
            }

            BloqueTiempo bloque = _bloqueDal.Obtener(bloqueTiempoId);

            if (bloque == null)
            {
                throw new InvalidOperationException("El bloque de tiempo no existe.");
            }

            _bloqueDal.Eliminar(bloqueTiempoId);

            RegistrarBitacora("ELIMINAR_BLOQUE_TIEMPO", "BloqueTiempo", "BloqueTiempoId=" + bloqueTiempoId + "; UsuarioId=" + bloque.UsuarioId);
        }

        public BloqueTiempo ObtenerBloqueTiempo(int bloqueTiempoId)
        {
            if (bloqueTiempoId <= 0)
            {
                throw new ArgumentException("BloqueTiempoId inválido.", nameof(bloqueTiempoId));
            }

            return _bloqueDal.Obtener(bloqueTiempoId);
        }

        public List<BloqueTiempo> ListarBloquesPorUsuario(int usuarioId)
        {
            ValidarUsuario(usuarioId);

            return _bloqueDal.ListarPorUsuario(usuarioId);
        }

        public List<BloqueCalendario> GenerarAgendaLaboral(int usuarioId, Planificacion planificacion)
        {
            ValidarUsuario(usuarioId);

            if (planificacion == null)
            {
                throw new ArgumentNullException(nameof(planificacion));
            }

            if (planificacion.Detalles == null || planificacion.Detalles.Count == 0)
            {
                throw new InvalidOperationException("La planificación no posee tareas para ubicar.");
            }

            List<JornadaLaboralUsuario> jornadas = _jornadaDal.ListarPorUsuario(usuarioId);

            if (jornadas.Count == 0)
            {
                throw new InvalidOperationException("El usuario no tiene jornada laboral configurada.");
            }

            List<BloqueTiempo> bloques = _bloqueDal.ListarPorUsuario(usuarioId);
            List<BloqueCalendario> calendario = CrearBloquesBaseCalendario(jornadas, bloques);
            List<HuecoDisponible> huecos = CalcularHuecosDisponiblesDesdeAhora(jornadas, bloques);

            UbicarTareasEnHuecos(planificacion, huecos, calendario);
            OrdenarCalendario(calendario);

            RegistrarBitacora("GENERAR_AGENDA_LABORAL", "TiempoDisponible", "PlanificacionId=" + planificacion.PlanificacionId + "; UsuarioId=" + usuarioId);

            return calendario;
        }

        public int CalcularMinutosJornadaSemanal(int usuarioId)
        {
            ValidarUsuario(usuarioId);

            List<JornadaLaboralUsuario> jornadas = _jornadaDal.ListarPorUsuario(usuarioId);
            int minutos = 0;

            foreach (JornadaLaboralUsuario jornada in jornadas)
            {
                minutos += ObtenerDuracionMinutos(jornada.HoraInicio, jornada.HoraFin);
            }

            return minutos;
        }

        public int CalcularMinutosBloqueadosSemanales(int usuarioId)
        {
            ValidarUsuario(usuarioId);

            List<BloqueTiempo> bloques = _bloqueDal.ListarPorUsuario(usuarioId);
            int minutos = 0;

            foreach (BloqueTiempo bloque in bloques)
            {
                minutos += ObtenerDuracionMinutos(bloque.HoraInicio, bloque.HoraFin);
            }

            return minutos;
        }

        public int CalcularMinutosDisponiblesSemanales(int usuarioId)
        {
            int minutosJornada = CalcularMinutosJornadaSemanal(usuarioId);
            int minutosBloqueados = CalcularMinutosBloqueadosSemanales(usuarioId);
            int disponibles = minutosJornada - minutosBloqueados;

            if (disponibles < 0)
            {
                return 0;
            }

            return disponibles;
        }

        private void ValidarUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(usuarioId));
            }
        }

        private void ValidarJornadaLaboral(JornadaLaboralUsuario jornada, bool creando)
        {
            if (jornada == null)
            {
                throw new ArgumentNullException(nameof(jornada));
            }

            if (!creando && jornada.JornadaLaboralUsuarioId <= 0)
            {
                throw new ArgumentException("JornadaLaboralUsuarioId inválido.", nameof(jornada.JornadaLaboralUsuarioId));
            }

            ValidarUsuario(jornada.UsuarioId);
            ValidarDiaSemana(jornada.DiaSemana);
            ValidarRangoHorario(jornada.HoraInicio, jornada.HoraFin);
        }

        private void ValidarBloqueTiempo(BloqueTiempo bloque, bool creando)
        {
            if (bloque == null)
            {
                throw new ArgumentNullException(nameof(bloque));
            }

            if (!creando && bloque.BloqueTiempoId <= 0)
            {
                throw new ArgumentException("BloqueTiempoId inválido.", nameof(bloque.BloqueTiempoId));
            }

            ValidarUsuario(bloque.UsuarioId);
            ValidarDiaSemana(bloque.DiaSemana);
            ValidarRangoHorario(bloque.HoraInicio, bloque.HoraFin);

            if (string.IsNullOrWhiteSpace(bloque.Titulo))
            {
                throw new ArgumentException("El título del bloque es obligatorio.", nameof(bloque.Titulo));
            }

            if (!Enum.IsDefined(typeof(TipoBloqueTiempo), bloque.TipoBloque))
            {
                throw new ArgumentException("Tipo de bloque inválido.", nameof(bloque.TipoBloque));
            }
        }

        private void ValidarDiaSemana(int diaSemana)
        {
            if (diaSemana < 1 || diaSemana > 7)
            {
                throw new ArgumentException("Día de semana inválido.", nameof(diaSemana));
            }
        }

        private void ValidarRangoHorario(TimeSpan horaInicio, TimeSpan horaFin)
        {
            if (horaInicio >= horaFin)
            {
                throw new ArgumentException("La hora de inicio debe ser menor que la hora de fin.");
            }
        }

        private void ValidarJornadaNoSuperpuesta(JornadaLaboralUsuario jornada)
        {
            List<JornadaLaboralUsuario> jornadasDia = _jornadaDal.ListarPorUsuarioYDia(jornada.UsuarioId, jornada.DiaSemana);

            foreach (JornadaLaboralUsuario jornadaExistente in jornadasDia)
            {
                if (jornadaExistente.JornadaLaboralUsuarioId != jornada.JornadaLaboralUsuarioId)
                {
                    if (HaySolapamiento(jornada.HoraInicio, jornada.HoraFin, jornadaExistente.HoraInicio, jornadaExistente.HoraFin))
                    {
                        throw new InvalidOperationException("Ya existe una jornada laboral superpuesta para ese usuario y día.");
                    }
                }
            }
        }

        private void ValidarBloqueDentroDeJornada(BloqueTiempo bloque)
        {
            List<JornadaLaboralUsuario> jornadasDia = _jornadaDal.ListarPorUsuarioYDia(bloque.UsuarioId, bloque.DiaSemana);
            bool estaDentro = false;

            foreach (JornadaLaboralUsuario jornada in jornadasDia)
            {
                if (bloque.HoraInicio >= jornada.HoraInicio && bloque.HoraFin <= jornada.HoraFin)
                {
                    estaDentro = true;

                    break;
                }
            }

            if (!estaDentro)
            {
                throw new InvalidOperationException("El bloque debe estar dentro de una jornada laboral configurada.");
            }
        }

        private void ValidarBloqueNoSuperpuesto(BloqueTiempo bloque)
        {
            List<BloqueTiempo> bloquesDia = _bloqueDal.ListarPorUsuarioYDia(bloque.UsuarioId, bloque.DiaSemana);

            foreach (BloqueTiempo bloqueExistente in bloquesDia)
            {
                if (bloqueExistente.BloqueTiempoId != bloque.BloqueTiempoId)
                {
                    if (HaySolapamiento(bloque.HoraInicio, bloque.HoraFin, bloqueExistente.HoraInicio, bloqueExistente.HoraFin))
                    {
                        throw new InvalidOperationException("Ya existe un bloque de tiempo superpuesto para ese usuario y día.");
                    }
                }
            }
        }

        private bool HaySolapamiento(TimeSpan inicioA, TimeSpan finA, TimeSpan inicioB, TimeSpan finB)
        {
            return inicioA < finB && inicioB < finA;
        }

        private int ObtenerDuracionMinutos(TimeSpan horaInicio, TimeSpan horaFin)
        {
            return Convert.ToInt32((horaFin - horaInicio).TotalMinutes);
        }

        private int ObtenerDuracionTareaParaAgenda(int duracionOriginalMinutos)
        {
            if (duracionOriginalMinutos < DuracionMinimaTareaAgendaMinutos)
            {
                return DuracionMinimaTareaAgendaMinutos;
            }

            return duracionOriginalMinutos;
        }

        private List<BloqueCalendario> CrearBloquesBaseCalendario(List<JornadaLaboralUsuario> jornadas, List<BloqueTiempo> bloques)
        {
            List<BloqueCalendario> calendario = new List<BloqueCalendario>();
            DateTime fechaInicio = DateTime.Now.Date;

            for (int i = 0; i < DiasMaximosBusqueda; i++)
            {
                DateTime fechaEvaluada = fechaInicio.AddDays(i);
                int diaSemana = ObtenerDiaSemanaLaboral(fechaEvaluada);

                foreach (JornadaLaboralUsuario jornada in jornadas)
                {
                    if (jornada.DiaSemana == diaSemana && jornada.Activo)
                    {
                        calendario.Add(new BloqueCalendario { Titulo = "Jornada laboral", Fecha = fechaEvaluada, DiaSemana = jornada.DiaSemana, HoraInicio = jornada.HoraInicio, HoraFin = jornada.HoraFin, TipoBloque = TipoBloqueCalendario.Jornada, TareaId = null, BloqueTiempoId = null, ScorePrioridad = null });
                    }
                }

                foreach (BloqueTiempo bloque in bloques)
                {
                    if (bloque.DiaSemana == diaSemana && bloque.Activo)
                    {
                        calendario.Add(new BloqueCalendario { Titulo = bloque.Titulo, Fecha = fechaEvaluada, DiaSemana = bloque.DiaSemana, HoraInicio = bloque.HoraInicio, HoraFin = bloque.HoraFin, TipoBloque = TipoBloqueCalendario.BloqueFijo, TareaId = null, BloqueTiempoId = bloque.BloqueTiempoId, ScorePrioridad = null });
                    }
                }
            }

            return calendario;
        }

        private List<HuecoDisponible> CalcularHuecosDisponiblesDesdeAhora(List<JornadaLaboralUsuario> jornadas, List<BloqueTiempo> bloques)
        {
            List<HuecoDisponible> huecos = new List<HuecoDisponible>();
            DateTime fechaHoraActual = DateTime.Now;
            DateTime fechaInicio = fechaHoraActual.Date;

            for (int i = 0; i < DiasMaximosBusqueda; i++)
            {
                DateTime fechaEvaluada = fechaInicio.AddDays(i);
                int diaSemana = ObtenerDiaSemanaLaboral(fechaEvaluada);

                List<JornadaLaboralUsuario> jornadasDia = FiltrarJornadasPorDia(jornadas, diaSemana);
                List<BloqueTiempo> bloquesDia = FiltrarBloquesPorDia(bloques, diaSemana);

                OrdenarJornadas(jornadasDia);
                OrdenarBloques(bloquesDia);

                foreach (JornadaLaboralUsuario jornada in jornadasDia)
                {
                    TimeSpan inicioHueco = jornada.HoraInicio;

                    if (i == 0)
                    {
                        if (fechaHoraActual.TimeOfDay >= jornada.HoraFin)
                        {
                            continue;
                        }

                        if (fechaHoraActual.TimeOfDay > jornada.HoraInicio)
                        {
                            inicioHueco = RedondearHoraActual(fechaHoraActual.TimeOfDay);
                        }
                    }

                    if (inicioHueco < jornada.HoraFin)
                    {
                        AgregarHuecosDeJornada(huecos, fechaEvaluada, diaSemana, inicioHueco, jornada.HoraFin, bloquesDia);
                    }
                }
            }

            return huecos;
        }

        private void AgregarHuecosDeJornada(List<HuecoDisponible> huecos, DateTime fecha, int diaSemana, TimeSpan inicioJornada, TimeSpan finJornada, List<BloqueTiempo> bloquesDia)
        {
            TimeSpan cursor = inicioJornada;

            foreach (BloqueTiempo bloque in bloquesDia)
            {
                if (bloque.HoraFin <= cursor)
                {
                    continue;
                }

                if (bloque.HoraInicio >= finJornada)
                {
                    break;
                }

                TimeSpan inicioBloque = bloque.HoraInicio;
                TimeSpan finBloque = bloque.HoraFin;

                if (inicioBloque < inicioJornada)
                {
                    inicioBloque = inicioJornada;
                }

                if (finBloque > finJornada)
                {
                    finBloque = finJornada;
                }

                if (cursor < inicioBloque)
                {
                    huecos.Add(new HuecoDisponible { Fecha = fecha, DiaSemana = diaSemana, HoraInicio = cursor, HoraFin = inicioBloque });
                }

                if (cursor < finBloque)
                {
                    cursor = finBloque;
                }
            }

            if (cursor < finJornada)
            {
                huecos.Add(new HuecoDisponible { Fecha = fecha, DiaSemana = diaSemana, HoraInicio = cursor, HoraFin = finJornada });
            }
        }

        private void UbicarTareasEnHuecos(Planificacion planificacion, List<HuecoDisponible> huecos, List<BloqueCalendario> calendario)
        {
            List<PlanificacionDetalle> detallesPendientes = ObtenerDetallesOrdenados(planificacion);

            foreach (HuecoDisponible hueco in huecos)
            {
                bool seUbicoAlgunaTarea = true;

                while (seUbicoAlgunaTarea)
                {
                    seUbicoAlgunaTarea = false;

                    PlanificacionDetalle detalle = BuscarPrimeraTareaQueEntra(detallesPendientes, hueco);

                    if (detalle == null)
                    {
                        break;
                    }

                    int duracion = ObtenerDuracionTareaParaAgenda(detalle.Tarea.DuracionEstimadaMin);

                    TimeSpan inicioTarea = hueco.HoraInicio;
                    TimeSpan finTarea = inicioTarea.Add(TimeSpan.FromMinutes(duracion));

                    calendario.Add(new BloqueCalendario { Titulo = detalle.Tarea.Titulo, Fecha = hueco.Fecha, DiaSemana = hueco.DiaSemana, HoraInicio = inicioTarea, HoraFin = finTarea, TipoBloque = TipoBloqueCalendario.TareaPlanificada, TareaId = detalle.TareaId, BloqueTiempoId = null, ScorePrioridad = detalle.ScorePrioridad });

                    hueco.HoraInicio = finTarea;

                    detallesPendientes.Remove(detalle);

                    seUbicoAlgunaTarea = true;
                }
            }

            foreach (PlanificacionDetalle detallePendiente in detallesPendientes)
            {
                if (detallePendiente.Tarea == null)
                {
                    continue;
                }

                calendario.Add(new BloqueCalendario { Titulo = detallePendiente.Tarea.Titulo, Fecha = DateTime.MinValue, DiaSemana = 0, HoraInicio = TimeSpan.Zero, HoraFin = TimeSpan.Zero, TipoBloque = TipoBloqueCalendario.TareaSinUbicar, TareaId = detallePendiente.TareaId, BloqueTiempoId = null, ScorePrioridad = detallePendiente.ScorePrioridad });
            }
        }

        private PlanificacionDetalle BuscarPrimeraTareaQueEntra(List<PlanificacionDetalle> detallesPendientes, HuecoDisponible hueco)
        {
            int duracionHueco = ObtenerDuracionMinutos(hueco.HoraInicio, hueco.HoraFin);

            foreach (PlanificacionDetalle detalle in detallesPendientes)
            {
                if (detalle.Tarea == null)
                {
                    continue;
                }

                int duracionTarea = ObtenerDuracionTareaParaAgenda(detalle.Tarea.DuracionEstimadaMin);

                if (duracionTarea <= duracionHueco)
                {
                    return detalle;
                }
            }

            return null;
        }

        private List<PlanificacionDetalle> ObtenerDetallesOrdenados(Planificacion planificacion)
        {
            List<PlanificacionDetalle> detalles = new List<PlanificacionDetalle>();

            foreach (PlanificacionDetalle detalle in planificacion.Detalles)
            {
                if (detalle.Tarea == null)
                {
                    continue;
                }

                if (!EsTareaPendienteParaAgenda(detalle.Tarea))
                {
                    continue;
                }

                detalles.Add(detalle);
            }

            detalles.Sort(CompararDetallesPorOrden);

            return detalles;
        }

        private bool EsTareaPendienteParaAgenda(Tarea tarea)
        {
            return tarea.Estado == EstadoTarea.Pendiente ||
                   tarea.Estado == EstadoTarea.EnCurso ||
                   tarea.Estado == EstadoTarea.Pausada;
        }

        private int CompararDetallesPorOrden(PlanificacionDetalle primerDetalle, PlanificacionDetalle segundoDetalle)
        {
            return primerDetalle.Orden.CompareTo(segundoDetalle.Orden);
        }

        private int ObtenerDiaSemanaLaboral(DateTime fecha)
        {
            if (fecha.DayOfWeek == DayOfWeek.Monday)
            {
                return 1;
            }

            if (fecha.DayOfWeek == DayOfWeek.Tuesday)
            {
                return 2;
            }

            if (fecha.DayOfWeek == DayOfWeek.Wednesday)
            {
                return 3;
            }

            if (fecha.DayOfWeek == DayOfWeek.Thursday)
            {
                return 4;
            }

            if (fecha.DayOfWeek == DayOfWeek.Friday)
            {
                return 5;
            }

            if (fecha.DayOfWeek == DayOfWeek.Saturday)
            {
                return 6;
            }

            return 7;
        }

        private TimeSpan RedondearHoraActual(TimeSpan horaActual)
        {
            int minutosTotales = Convert.ToInt32(Math.Ceiling(horaActual.TotalMinutes));

            return TimeSpan.FromMinutes(minutosTotales);
        }

        private List<JornadaLaboralUsuario> FiltrarJornadasPorDia(List<JornadaLaboralUsuario> jornadas, int diaSemana)
        {
            List<JornadaLaboralUsuario> jornadasDia = new List<JornadaLaboralUsuario>();

            foreach (JornadaLaboralUsuario jornada in jornadas)
            {
                if (jornada.DiaSemana == diaSemana && jornada.Activo)
                {
                    jornadasDia.Add(jornada);
                }
            }

            return jornadasDia;
        }

        private List<BloqueTiempo> FiltrarBloquesPorDia(List<BloqueTiempo> bloques, int diaSemana)
        {
            List<BloqueTiempo> bloquesDia = new List<BloqueTiempo>();

            foreach (BloqueTiempo bloque in bloques)
            {
                if (bloque.DiaSemana == diaSemana && bloque.Activo)
                {
                    bloquesDia.Add(bloque);
                }
            }

            return bloquesDia;
        }

        private void OrdenarJornadas(List<JornadaLaboralUsuario> jornadas)
        {
            jornadas.Sort(CompararJornadasPorHora);
        }

        private int CompararJornadasPorHora(JornadaLaboralUsuario primeraJornada, JornadaLaboralUsuario segundaJornada)
        {
            return primeraJornada.HoraInicio.CompareTo(segundaJornada.HoraInicio);
        }

        private void OrdenarBloques(List<BloqueTiempo> bloques)
        {
            bloques.Sort(CompararBloquesPorHora);
        }

        private int CompararBloquesPorHora(BloqueTiempo primerBloque, BloqueTiempo segundoBloque)
        {
            return primerBloque.HoraInicio.CompareTo(segundoBloque.HoraInicio);
        }

        private void OrdenarCalendario(List<BloqueCalendario> calendario)
        {
            calendario.Sort(CompararBloquesCalendario);
        }

        private int CompararBloquesCalendario(BloqueCalendario primerBloque, BloqueCalendario segundoBloque)
        {
            int comparacionFecha = primerBloque.Fecha.CompareTo(segundoBloque.Fecha);

            if (comparacionFecha != 0)
            {
                return comparacionFecha;
            }

            int comparacionDia = primerBloque.DiaSemana.CompareTo(segundoBloque.DiaSemana);

            if (comparacionDia != 0)
            {
                return comparacionDia;
            }

            int comparacionHora = primerBloque.HoraInicio.CompareTo(segundoBloque.HoraInicio);

            if (comparacionHora != 0)
            {
                return comparacionHora;
            }

            return primerBloque.TipoBloque.CompareTo(segundoBloque.TipoBloque);
        }

        private void RegistrarBitacora(string accion, string entidad, string metadatos)
        {
            string usuario = string.Empty;

            if (SesionActual.Instance != null)
            {
                if (SesionActual.Instance.NombreUsuario != null)
                {
                    usuario = SesionActual.Instance.NombreUsuario;
                }
                else
                {
                    usuario = string.Empty;
                }
            }

            _bitacora.Registrar(accion, entidad, "OK", usuario, "TiempoDisponible", metadatos);
        }

        private class HuecoDisponible
        {
            public DateTime Fecha { get; set; }
            public int DiaSemana { get; set; }
            public TimeSpan HoraInicio { get; set; }
            public TimeSpan HoraFin { get; set; }
        }
    }
}