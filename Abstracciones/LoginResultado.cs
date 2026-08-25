using System;

namespace Abstracciones
{
    public enum LoginStatus
    {
        Exito,
        UsuarioInexistente,
        UsuarioBloqueado,
        CredencialesInvalidas,
        ParametrosInvalidos
    }

    public class LoginResultado
    {
        public LoginStatus Status { get; set; }
        public string Mensaje { get; set; }
        public DateTime? DesbloqueoUtc { get; set; }
        public int IntentosFallidos { get; set; }
        public int? MinutosBloqueoAplicados { get; set; }

        public bool Exito
        {
            get
            {
                return Status == LoginStatus.Exito;
            }
        }

        public bool Bloqueado
        {
            get
            {
                return Status == LoginStatus.UsuarioBloqueado;
            }
        }
    }
}
