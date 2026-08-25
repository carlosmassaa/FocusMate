using System;

namespace Abstracciones
{
    public interface IUsuario
    {
        int Id { get; set; }
        string NombreUsuario { get; set; }
        byte[] PasswordHash { get; set; }
        byte[] PasswordSalt { get; set; }
        string PasswordAlg { get; set; }
        bool EstaActivo { get; set; }
        int FailedAttempts { get; set; }
        DateTime BloqueadoHastaUtc { get; set; }
        DateTime CreadoUtc { get; set; }

        bool TienePermiso(string patenteNombre);
    }
}