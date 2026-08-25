using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class Familia : Componente
    {
        private readonly List<Componente> _hijos = new List<Componente>();

        public void AgregarHijo(Componente componente)
        {
            if (componente != null && !_hijos.Any(childComponent => childComponent.Id == componente.Id))
            {
                _hijos.Add(componente);
            }                
        }

        public void QuitarHijo(int componenteId)
        {
            _hijos.RemoveAll(childComponent => childComponent.Id == componenteId);
        }

        public override IEnumerable<Componente> ObtenerHijos() { return _hijos; }

        public override bool Contiene(string patenteNombre)
        {
            if (string.IsNullOrEmpty(patenteNombre)) 
            {
                return false;
            }
            foreach (Componente childComponent in _hijos)
            {
                if (childComponent.Contiene(patenteNombre))
                {
                    return true;
                }
            }
                
            return false;
        }
    }
}