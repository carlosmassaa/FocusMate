using System;
using System.Collections.Generic;
using System.Globalization;

namespace BE
{
    public enum ImportanciaTarea { MuyBaja = 1, Baja = 2, Media = 3, Alta = 4, MuyAlta = 5 }
    public enum EnergiaRequeridaTarea { Baja = 0, Media = 1, Alta = 2 }
    public enum EstadoTarea { Pendiente = 0, EnCurso = 1, Pausada = 2, Completada = 3, Cancelada = 4 }

    public class Tarea
    {
        public int TareaId { get; set; }
        public int UsuarioId { get; set; }
        public int? ProyectoId { get; set; }

        public string Titulo { get; set; }
        public string Descripcion { get; set; }

        public DateTime? FechaLimite { get; set; }
        public ImportanciaTarea Importancia { get; set; } = ImportanciaTarea.Media;
        public EnergiaRequeridaTarea EnergiaRequerida { get; set; } = EnergiaRequeridaTarea.Media;
        public int DuracionEstimadaMin { get; set; }

        public List<TareaHistorialEntry> tareaHistorials { get; set; }

        public decimal ScorePrioridad { get; set; }
        public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;
        public DateTime ActualizadoUtc { get; set; } = DateTime.UtcNow;
        public long DVH { get; set; }

        
    }
}