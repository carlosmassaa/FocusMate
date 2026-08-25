namespace Abstracciones
{
    public interface IAuthManager
    {
        bool Login(string usuario, string password);
        LoginResultado IntentarLogin(string usuario, string password);
        void Logout();
        bool ValidarPermiso(string patenteNombre);

        bool RegistrarUsuario(string nombreUsuario, string password);
        bool ValidarPoliticasPassword(string password);

        bool EstaAutenticado { get; }
        IUsuario UsuarioActual { get; }
    }
}