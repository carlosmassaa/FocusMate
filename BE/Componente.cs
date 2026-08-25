using System.Collections.Generic;

namespace BE
{
    public abstract class Componente
    {
        public int Id { get; set; }                 
        public string Nombre { get; set; }          
        public string Descripcion { get; set; }     

        public virtual IEnumerable<Componente> ObtenerHijos() { yield break; }
        
        public abstract bool Contiene(string patenteNombre);
    }
}