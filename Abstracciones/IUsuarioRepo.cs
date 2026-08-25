using System.Collections.Generic;

namespace Abstracciones
{
    public interface IUsuarioRepo
    {
        IUsuario ObtenerPorNombre(string nombreUsuario);
        IUsuario ObtenerPorId(int id);

        int GuardarNuevo(IUsuario u);
        void ActualizarIntentosOBloqueo(IUsuario u);
        void ActualizarPassword(IUsuario u);
        void ActualizarEstado(IUsuario u);

        IEnumerable<IComponente> ObtenerComponentesDeUsuario(int usuarioId);
        void AsignarComponenteAUsuario(int usuarioId, int componenteId);

        int ObtenerIdComponentePorNombre(string nombre);            
        int CrearPatenteSiNoExiste(string nombre, string descripcion);
        bool ExisteRelacionUsuarioComponente(int usuarioId, int componenteId);
    }
}