using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using BE;
using DAL;
using Servicioss;
using System.Data.SqlTypes;

namespace BL
{
    public class TareaBL
    {
        private readonly IntegridadBL _integridad = new IntegridadBL();
        private readonly BitacoraBL _bitacora = BitacoraBL.CrearBasico();

        private readonly TareaDAL _tareaDal;
        private readonly TareaHistorialDAL _histDal;
        private readonly DigitoVerificadorDAL _dvDal;
        private readonly IdiomaDAL _idiomaDal;
        private readonly ComponenteDAL _compDal;
        private readonly UsuarioDAL _usuarioDal;

        public TareaBL()
        {
            _tareaDal = new TareaDAL();
            _histDal = new TareaHistorialDAL();
            _dvDal = new DigitoVerificadorDAL();
            _idiomaDal = new IdiomaDAL();
            _compDal = new ComponenteDAL();
            _usuarioDal = new UsuarioDAL();
        }

        public int Crear(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            Validar(tarea, true);

            tarea.ScorePrioridad = CalcularScore(tarea);
            tarea.CreadoUtc = DateTime.UtcNow;
            tarea.ActualizadoUtc = DateTime.UtcNow;
            tarea.DVH = CalcularDVH(tarea);

            int tareaId = _tareaDal.GuardarNuevo(tarea);

            Tarea tareaDb = _tareaDal.Obtener(tareaId);
            long dvhDb = CalcularDVH(tareaDb);

            if (dvhDb != tareaDb.DVH)
            {
                _tareaDal.ActualizarDVH(tareaId, dvhDb);
            }

            _integridad.RecalcularDVV_Tarea();
            RegistrarHistorial(tareaDb, SesionActual.Instance.UsuarioId, "CREAR");

            string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
            _bitacora.Registrar("CREAR", "Tarea", "OK", usuario, "Tareas", "TareaId=" + tareaId);

            return tareaId;
        }

        public void Actualizar(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            Validar(tarea, false);

            tarea.ScorePrioridad = CalcularScore(tarea);
            tarea.ActualizadoUtc = DateTime.UtcNow;
            tarea.DVH = CalcularDVH(tarea);

            _tareaDal.Guardar(tarea);

            Tarea tareaDb = _tareaDal.Obtener(tarea.TareaId);
            long dvhDb = CalcularDVH(tareaDb);

            if (dvhDb != tareaDb.DVH)
            {
                _tareaDal.ActualizarDVH(tarea.TareaId, dvhDb);
            }

            _integridad.RecalcularDVV_Tarea();
            RegistrarHistorial(tareaDb, SesionActual.Instance.UsuarioId, "ACTUALIZAR");

            string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
            _bitacora.Registrar("ACTUALIZAR", "Tarea", "OK", usuario, "Tareas", "TareaId=" + tarea.TareaId);
        }

        public void Eliminar(int tareaId)
        {
            if (tareaId <= 0)
            {
                throw new ArgumentException("TareaId inválido.", nameof(tareaId));
            }

            Tarea tareaEliminada = _tareaDal.Obtener(tareaId);

            if (tareaEliminada == null)
            {
                throw new InvalidOperationException("La tarea no existe.");
            }

            tareaEliminada.Estado = EstadoTarea.Cancelada;
            tareaEliminada.ScorePrioridad = CalcularScore(tareaEliminada);
            tareaEliminada.ActualizadoUtc = DateTime.UtcNow;
            tareaEliminada.DVH = CalcularDVH(tareaEliminada);

            _tareaDal.Guardar(tareaEliminada);

            Tarea tareaDb = _tareaDal.Obtener(tareaId);
            long dvhDb = CalcularDVH(tareaDb);

            if (dvhDb != tareaDb.DVH)
            {
                _tareaDal.ActualizarDVH(tareaId, dvhDb);
            }

            _integridad.RecalcularDVV_Tarea();
            RegistrarHistorial(tareaDb, SesionActual.Instance.UsuarioId, "ELIMINAR");

            string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
            _bitacora.Registrar("ELIMINAR", "Tarea", "OK", usuario, "Tareas", "TareaId=" + tareaId);
        }

        public Tarea Obtener(int tareaId)
        {
            return _tareaDal.Obtener(tareaId);
        }

        public List<Tarea> ListarPorUsuario(int usuarioId)
        {
            return _tareaDal.ListarPorUsuario(usuarioId);
        }

        public List<TareaHistorialEntry> ListarHistorial(int tareaId)
        {
            return _histDal.ListarPorTarea(tareaId);
        }

        public void RecalcularScoresUsuario(int usuarioId)
        {
            List<Tarea> tareas = _tareaDal.ListarPorUsuario(usuarioId);

            foreach (Tarea tareaUsuario in tareas)
            {
                tareaUsuario.ScorePrioridad = CalcularScore(tareaUsuario);
                tareaUsuario.ActualizadoUtc = DateTime.UtcNow;
                tareaUsuario.DVH = CalcularDVH(tareaUsuario);

                _tareaDal.Guardar(tareaUsuario);

                Tarea tareaDb = _tareaDal.Obtener(tareaUsuario.TareaId);
                long dvhDb = CalcularDVH(tareaDb);

                if (dvhDb != tareaDb.DVH)
                {
                    _tareaDal.ActualizarDVH(tareaUsuario.TareaId, dvhDb);
                }
            }

            _integridad.RecalcularDVV_Tarea();
        }

        public void RevertirA(int tareaId, int historialId)
        {
            TareaHistorialEntry historialEntry = _histDal.Obtener(historialId);

            if (historialEntry == null || historialEntry.TareaId != tareaId)
            {
                throw new InvalidOperationException("Historial inválido para esta tarea.");
            }

            Tarea tareaActual = _tareaDal.Obtener(tareaId);

            if (tareaActual == null)
            {
                throw new InvalidOperationException("Tarea inexistente.");
            }

            tareaActual.ProyectoId = historialEntry.ProyectoId;
            tareaActual.Titulo = historialEntry.Titulo;
            tareaActual.Descripcion = historialEntry.Descripcion;

            if (historialEntry.FechaLimite.HasValue && historialEntry.FechaLimite.Value.Date < DateTime.Today)
            {
                tareaActual.FechaLimite = DateTime.Today;
            }
            else
            {
                tareaActual.FechaLimite = historialEntry.FechaLimite;
            }

            tareaActual.Importancia = historialEntry.Importancia;
            tareaActual.EnergiaRequerida = historialEntry.EnergiaRequerida;
            tareaActual.DuracionEstimadaMin = historialEntry.DuracionEstimadaMin;
            tareaActual.Estado = historialEntry.Estado;

            tareaActual.ScorePrioridad = CalcularScore(tareaActual);
            tareaActual.ActualizadoUtc = DateTime.UtcNow;
            tareaActual.DVH = CalcularDVH(tareaActual);

            Validar(tareaActual, false);

            _tareaDal.Guardar(tareaActual);

            Tarea tareaDb = _tareaDal.Obtener(tareaActual.TareaId);
            long dvhDb = CalcularDVH(tareaDb);

            if (dvhDb != tareaDb.DVH)
            {
                _tareaDal.ActualizarDVH(tareaActual.TareaId, dvhDb);
            }

            _integridad.RecalcularDVV_Tarea();
            RegistrarHistorial(tareaDb, SesionActual.Instance.UsuarioId, "REVERTIR");

            string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
            _bitacora.Registrar("REVERTIR", "Tarea", "OK", usuario, "Tareas", "TareaId=" + tareaId + "; HistorialId=" + historialId);
        }

        private void RegistrarHistorial(Tarea tarea, int usuarioOperacionId, string accion)
        {
            int usuarioOperacionIdFinal;

            if (usuarioOperacionId <= 0)
            {
                usuarioOperacionIdFinal = tarea.UsuarioId;
            }
            else
            {
                usuarioOperacionIdFinal = usuarioOperacionId;
            }

            TareaHistorialEntry historial = new TareaHistorialEntry { TareaId = tarea.TareaId, UsuarioOperacionId = usuarioOperacionIdFinal, FechaUtc = DateTime.UtcNow, Accion = accion, ProyectoId = tarea.ProyectoId, Titulo = tarea.Titulo, Descripcion = tarea.Descripcion, FechaLimite = tarea.FechaLimite, Importancia = tarea.Importancia, EnergiaRequerida = tarea.EnergiaRequerida, DuracionEstimadaMin = tarea.DuracionEstimadaMin, ScorePrioridad = tarea.ScorePrioridad, Estado = tarea.Estado, UsuarioIdPropietario = tarea.UsuarioId };

            _histDal.Guardar(historial);
        }

        public decimal CalcularScore(Tarea tarea)
        {
            int importancia = (int)tarea.Importancia;
            decimal baseImp = importancia * 2.0m;
            decimal urgencia = 0m;

            if (tarea.FechaLimite.HasValue)
            {
                double dias = (tarea.FechaLimite.Value.Date - DateTime.UtcNow.Date).TotalDays;

                if (dias <= 0)
                {
                    urgencia = 4m;
                }
                else if (dias <= 1)
                {
                    urgencia = 3m;
                }
                else if (dias <= 3)
                {
                    urgencia = 2m;
                }
                else if (dias <= 7)
                {
                    urgencia = 1m;
                }
            }

            decimal factorEnergia;

            if (tarea.EnergiaRequerida == EnergiaRequeridaTarea.Baja)
            {
                factorEnergia = 1.0m;
            }
            else if (tarea.EnergiaRequerida == EnergiaRequeridaTarea.Media)
            {
                factorEnergia = 0.9m;
            }
            else
            {
                factorEnergia = 0.8m;
            }

            int duracionMinutosAjustada = Math.Max(1, tarea.DuracionEstimadaMin);

            decimal factorDuracion;

            if (duracionMinutosAjustada <= 30)
            {
                factorDuracion = 1.0m;
            }
            else if (duracionMinutosAjustada <= 60)
            {
                factorDuracion = 0.9m;
            }
            else if (duracionMinutosAjustada <= 120)
            {
                factorDuracion = 0.7m;
            }
            else
            {
                factorDuracion = 0.5m;
            }

            decimal score = (baseImp + urgencia) * factorEnergia * factorDuracion;

            if (tarea.Estado == EstadoTarea.Completada || tarea.Estado == EstadoTarea.Cancelada)
            {
                score = 0m;
            }

            return Math.Round(score, 4, MidpointRounding.AwayFromZero);
        }

        private void Validar(Tarea tarea, bool creando)
        {
            if (string.IsNullOrWhiteSpace(tarea.Titulo))
            {
                throw new ArgumentException("El título es obligatorio.", nameof(tarea.Titulo));
            }

            if (tarea.UsuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(tarea.UsuarioId));
            }

            if (!Enum.IsDefined(typeof(ImportanciaTarea), tarea.Importancia))
            {
                throw new ArgumentException("Importancia inválida.", nameof(tarea.Importancia));
            }

            if (!Enum.IsDefined(typeof(EnergiaRequeridaTarea), tarea.EnergiaRequerida))
            {
                throw new ArgumentException("Energía requerida inválida.", nameof(tarea.EnergiaRequerida));
            }

            if (!Enum.IsDefined(typeof(EstadoTarea), tarea.Estado))
            {
                throw new ArgumentException("Estado inválido.", nameof(tarea.Estado));
            }

            if (tarea.DuracionEstimadaMin <= 0 || tarea.DuracionEstimadaMin > 14400)
            {
                throw new ArgumentException("Duración estimada fuera de rango (1-14400).", nameof(tarea.DuracionEstimadaMin));
            }

            if (tarea.FechaLimite.HasValue)
            {
                DateTime fecha = tarea.FechaLimite.Value.Date;

                if (fecha < DateTime.Today)
                {
                    throw new ArgumentException("La fecha límite no puede ser anterior a hoy.", nameof(tarea.FechaLimite));
                }

                DateTime sqlMin = (DateTime)SqlDateTime.MinValue;
                DateTime sqlMax = (DateTime)SqlDateTime.MaxValue;

                if (fecha < sqlMin || fecha > sqlMax)
                {
                    throw new ArgumentException("La fecha límite no es válida para la base de datos.", nameof(tarea.FechaLimite));
                }
            }

            if (!creando && tarea.TareaId <= 0)
            {
                throw new ArgumentException("TareaId inválido para actualizar.", nameof(tarea.TareaId));
            }
        }

        private string ToDVHString(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            string fechaLimiteFormateada;

            if (tarea.FechaLimite.HasValue)
            {
                fechaLimiteFormateada = tarea.FechaLimite.Value.ToUniversalTime().ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            else
            {
                fechaLimiteFormateada = string.Empty;
            }

            return string.Join("|", tarea.TareaId.ToString(CultureInfo.InvariantCulture), tarea.UsuarioId.ToString(CultureInfo.InvariantCulture), tarea.ProyectoId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty, tarea.Titulo ?? string.Empty, tarea.Descripcion ?? string.Empty, fechaLimiteFormateada, ((int)tarea.Importancia).ToString(CultureInfo.InvariantCulture), ((int)tarea.EnergiaRequerida).ToString(CultureInfo.InvariantCulture), tarea.DuracionEstimadaMin.ToString(CultureInfo.InvariantCulture), tarea.ScorePrioridad.ToString("0.####", CultureInfo.InvariantCulture), ((int)tarea.Estado).ToString(CultureInfo.InvariantCulture), tarea.CreadoUtc.ToUniversalTime().ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), tarea.ActualizadoUtc.ToUniversalTime().ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture));
        }

        private long CalcularDVH(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }

            string cadena = ToDVHString(tarea);

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(cadena));
                long dvh = BitConverter.ToInt64(hash, 0);

                if (dvh < 0)
                {
                    return -dvh;
                }
                else
                {
                    return dvh;
                }
            }
        }

        public bool VerificarDVH_Tarea(out string detalle)
        {
            detalle = string.Empty;

            List<Tarea> tareas = _tareaDal.Listar();
            bool esValido = true;
            List<int> idsInconsistentes = new List<int>();

            foreach (Tarea tarea in tareas)
            {
                long dvhCalculado = CalcularDVH(tarea);

                if (dvhCalculado != tarea.DVH)
                {
                    esValido = false;
                    idsInconsistentes.Add(tarea.TareaId);
                    detalle += "DVH inconsistente en TareaId=" + tarea.TareaId + ". ";
                }
            }

            if (!esValido)
            {
                string usuario = SesionActual.Instance?.NombreUsuario ?? string.Empty;
                string metadatos = "Cantidad=" + idsInconsistentes.Count + "; IDs=" + string.Join(",", idsInconsistentes);
                _bitacora.Registrar("VERIFICAR_DVH", "Tarea", "FAIL", usuario, "Integridad", metadatos);
            }

            return esValido;
        }

        public int ActualizarFechasVencidasAHoy(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(usuarioId));
            }

            List<Tarea> tareas = _tareaDal.ListarPorUsuario(usuarioId);
            int cantidadActualizada = 0;

            foreach (Tarea tarea in tareas)
            {
                if ((tarea.Estado == EstadoTarea.Pendiente || tarea.Estado == EstadoTarea.EnCurso || tarea.Estado == EstadoTarea.Pausada)
                    && tarea.FechaLimite.HasValue
                    && tarea.FechaLimite.Value.Date < DateTime.Today)
                {
                    tarea.FechaLimite = DateTime.Today;
                    tarea.ScorePrioridad = CalcularScore(tarea);
                    tarea.ActualizadoUtc = DateTime.UtcNow;
                    tarea.DVH = CalcularDVH(tarea);

                    _tareaDal.Guardar(tarea);

                    Tarea tareaDb = _tareaDal.Obtener(tarea.TareaId);
                    long dvhDb = CalcularDVH(tareaDb);

                    if (dvhDb != tareaDb.DVH)
                    {
                        _tareaDal.ActualizarDVH(tarea.TareaId, dvhDb);
                    }

                    RegistrarHistorial(tareaDb, SesionActual.Instance.UsuarioId, "ACTUALIZAR_FECHA_VENCIDA");
                    cantidadActualizada++;
                }
            }

            if (cantidadActualizada > 0)
            {
                _integridad.RecalcularDVV_Tarea();

                string usuario = SesionActual.Instance.NombreUsuario ?? string.Empty;
                _bitacora.Registrar("ACTUALIZAR_FECHAS_VENCIDAS", "Tarea", "OK", usuario, "Tareas", "UsuarioId=" + usuarioId + "; Cantidad=" + cantidadActualizada);
            }

            return cantidadActualizada;
        }

        public void CambiarEstadoDesdeTop10(int tareaId, int usuarioId, EstadoTarea nuevoEstado)
        {
            ValidarDatosCambioEstadoTop10(tareaId, usuarioId, nuevoEstado);

            Tarea tarea = ObtenerTareaParaCambioEstado(tareaId);

            ValidarTareaParaCambioEstado(tarea, usuarioId);

            AsignarNuevoEstado(tarea, nuevoEstado);

            Actualizar(tarea);
        }

        private void ValidarDatosCambioEstadoTop10(int tareaId, int usuarioId, EstadoTarea nuevoEstado)
        {
            if (tareaId <= 0)
            {
                throw new ArgumentException("TareaId inválido.", nameof(tareaId));
            }

            if (usuarioId <= 0)
            {
                throw new ArgumentException("UsuarioId inválido.", nameof(usuarioId));
            }

            if (!Enum.IsDefined(typeof(EstadoTarea), nuevoEstado))
            {
                throw new ArgumentException("Estado inválido.", nameof(nuevoEstado));
            }
        }

        private Tarea ObtenerTareaParaCambioEstado(int tareaId)
        {
            return _tareaDal.Obtener(tareaId);
        }

        private void ValidarTareaParaCambioEstado(Tarea tarea, int usuarioId)
        {
            if (tarea == null)
            {
                throw new InvalidOperationException("La tarea no existe.");
            }

            if (tarea.UsuarioId != usuarioId)
            {
                throw new InvalidOperationException("No puede modificar una tarea que pertenece a otro usuario.");
            }
        }

        private void AsignarNuevoEstado(Tarea tarea, EstadoTarea nuevoEstado)
        {
            tarea.Estado = nuevoEstado;
        }

        public void RepararDVH_Tarea()
        {
            List<Tarea> tareas = _tareaDal.Listar();
            List<int> reparadas = new List<int>();

            foreach (Tarea tarea in tareas)
            {
                long dvhActual = tarea.DVH;
                long dvhNuevo = CalcularDVH(tarea);

                if (dvhNuevo != dvhActual)
                {
                    tarea.DVH = dvhNuevo;
                    _tareaDal.Guardar(tarea);
                    reparadas.Add(tarea.TareaId);
                }
            }

            if (reparadas.Count > 0)
            {
                string usuario = SesionActual.Instance?.NombreUsuario ?? string.Empty;
                string metadatos = "Cantidad=" + reparadas.Count + "; IDs=" + string.Join(",", reparadas);
                _bitacora.Registrar("REPARAR_DVH", "Tarea", "OK", usuario, "Integridad", metadatos);
            }
        }
    }
}