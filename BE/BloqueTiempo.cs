using System;

namespace BE
{
    public enum TipoBloqueTiempo
    {
        Otro = 0,
        Daily = 1,
        Almuerzo = 2,
        Reunion = 3,
        Descanso = 4,
        Capacitacion = 5
    }

    public class BloqueTiempo
    {
        public int BloqueTiempoId { get; set; }
        public int UsuarioId { get; set; }

        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public TipoBloqueTiempo TipoBloque { get; set; } = TipoBloqueTiempo.Otro;

        public int DiaSemana { get; set; }

        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;
        public DateTime ActualizadoUtc { get; set; } = DateTime.UtcNow;
    }
}