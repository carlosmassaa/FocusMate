namespace Abstracciones
{
    public interface IAutorizacionService
    {
        bool TienePermiso(IUsuario u, string patenteNombre);
        void AsegurarPatenteBitacoraPersistente();
    }
}