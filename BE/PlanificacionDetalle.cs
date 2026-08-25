using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PlanificacionDetalle
    {
        public int PlanificacionDetalleId { get; set; }
        public int PlanificacionId { get; set; }
        public int TareaId { get; set; }
        public int Orden { get; set; }
        public decimal ScorePrioridad { get; set; }
        public Tarea Tarea { get; set; }
    }
}
