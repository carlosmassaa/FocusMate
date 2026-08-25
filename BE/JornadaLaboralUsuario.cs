using System;

namespace BE
{
    public class JornadaLaboralUsuario
    {
        public int JornadaLaboralUsuarioId { get; set; }
        public int UsuarioId { get; set; }

        public int DiaSemana { get; set; }

        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;
        public DateTime ActualizadoUtc { get; set; } = DateTime.UtcNow;
    }
}