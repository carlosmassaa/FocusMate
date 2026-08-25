using System;
using System.Collections.Generic;
using BE;

namespace UI
{
    public class AgendaLaboralSerializada
    {
        public int UsuarioId { get; set; }

        public string NombreUsuario { get; set; }

        public DateTime FechaGeneracion { get; set; }

        public string Resumen { get; set; }

        public List<BloqueCalendario> Bloques { get; set; }

        public AgendaLaboralSerializada()
        {
            Bloques = new List<BloqueCalendario>();
        }
    }
}