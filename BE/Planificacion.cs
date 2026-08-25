using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Planificacion
    {
        public int PlanificacionId { get; set; }
        public int UsuarioId { get; set; }
        public int? SupervisorId { get; set; }

        public DateTime FechaGeneracionUtc { get; set; }
        public DateTime? FechaRevisionUtc { get; set; }
        public DateTime? FechaAprobacionUtc { get; set; }

        public EstadoPlanificacion Estado { get; set; }

        public string ObservacionRevision { get; set; }
        public string ObservacionAprobacion { get; set; }

        public List<PlanificacionDetalle> Detalles { get; set; }

    }
}
