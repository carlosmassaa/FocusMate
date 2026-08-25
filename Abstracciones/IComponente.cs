using System.Collections.Generic;

namespace Abstracciones
{
    public interface IComponente
    {
        int Id { get; set; }
        string Nombre { get; set; }
        string Descripcion { get; set; }
        void AgregarHijo(IComponente hijo);
        void Quitar(IComponente hijo);
        IEnumerable<IComponente> ObtenerHijos();
        bool Contiene(string patenteNombre);
    }
}