using System.Collections.Generic;

namespace Abstracciones
{
    public interface IBitacoraRepo
    {
        int Guardar(IBitacora registro);
        List<IBitacora> Listar();
    }
}