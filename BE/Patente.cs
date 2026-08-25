using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class Patente : Componente
    {

        public override IEnumerable<Componente> ObtenerHijos()
        {
            return Enumerable.Empty<Componente>();
        }

        public override bool Contiene(string patenteNombre)
        {
            return !string.IsNullOrWhiteSpace(patenteNombre) && string.Equals(Nombre, patenteNombre, StringComparison.OrdinalIgnoreCase);
        }
    }
}