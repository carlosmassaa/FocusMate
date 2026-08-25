using System;

namespace BE
{
    public enum TipoBloqueCalendario
    {
        Jornada = 0,
        BloqueFijo = 1,
        TareaPlanificada = 2,
        TareaSinUbicar = 3
    }

    public class BloqueCalendario
    {
        public string Titulo { get; set; }

        public DateTime Fecha { get; set; }

        public int DiaSemana { get; set; }

        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }

        public TipoBloqueCalendario TipoBloque { get; set; }

        public int? TareaId { get; set; }
        public int? BloqueTiempoId { get; set; }

        public decimal? ScorePrioridad { get; set; }

        public int DuracionMinutos
        {
            get
            {
                return Convert.ToInt32((HoraFin - HoraInicio).TotalMinutes);
            }
        }
    }
}