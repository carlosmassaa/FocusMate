using System.Collections.Generic;

namespace Abstracciones
{
    public interface IServicioBitacora
    {
        void Registrar(string accion, string entidad, string resultado, string usuario, string modulo, string metadatos);
        List<IBitacora> Buscar();
        List<IBitacora> Buscar(IFiltrosBitacora filtros);
        bool ValidarPermisosConsulta(int usuarioId);
    }
}