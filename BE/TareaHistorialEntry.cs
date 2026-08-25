using System;

namespace BE
{
    public class TareaHistorialEntry
    {
        public int HistorialId { get; set; }
        public int TareaId { get; set; }
        
        public int UsuarioOperacionId { get; set; } 
        public DateTime FechaUtc { get; set; }
        public string Accion { get; set; }         
        public int? ProyectoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaLimite { get; set; }
        public ImportanciaTarea Importancia { get; set; }
        public EnergiaRequeridaTarea EnergiaRequerida { get; set; }
        public int DuracionEstimadaMin { get; set; }
        public decimal ScorePrioridad { get; set; }
        public EstadoTarea Estado { get; set; }
        public int UsuarioIdPropietario { get; set; } 
    }
}