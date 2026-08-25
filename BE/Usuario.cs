using System;
using System.Collections.Generic;
using Abstracciones;

namespace BE
{
    public class Usuario : IUsuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string PasswordAlg { get; set; } = "SHA-256";
        public bool EstaActivo { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime BloqueadoHastaUtc { get; set; }
        public DateTime CreadoUtc { get; set; }
        public int? IdiomaId { get; set; }

        private bool tieneActualizadoUtc;
        private DateTime fechaActualizadoUtc;

        public DateTime ActualizadoUtc
        {
            get
            {
                if (tieneActualizadoUtc)
                {
                    return fechaActualizadoUtc;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                fechaActualizadoUtc = value;
                tieneActualizadoUtc = true;
            }
        }

        public bool TieneActualizadoUtc => tieneActualizadoUtc;

        public void LimpiarActualizadoUtc()
        {
            tieneActualizadoUtc = false;
            fechaActualizadoUtc = default(DateTime);
        }

        private readonly List<Componente> componentes = new List<Componente>();

        public bool TienePermiso(string patenteNombre)
        {
            foreach (Componente componente in componentes)
            {
                if (componente != null)
                {
                    bool contiene = componente.Contiene(patenteNombre);
                    if (contiene)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void AgregarPermiso(Componente componente)
        {
            if (componente != null)
            {
                bool yaExiste = componentes.Contains(componente);
                if (!yaExiste)
                {
                    componentes.Add(componente);
                }
            }
        }

        public void LimpiarPermisos()
        {
            componentes.Clear();
        }

        public void ResetearIntentos()
        {
            FailedAttempts = 0;
        }

        public void BloquearHasta(DateTime fechaBloqueoUtc)
        {
            BloqueadoHastaUtc = fechaBloqueoUtc;
        }

        public void Activar()
        {
            EstaActivo = true;
        }

        public void Desactivar()
        {
            EstaActivo = false;
        }

        public void EstablecerCredencialesPassword(byte[] nuevoHash, byte[] nuevoSalt, string algoritmo)
        {
            PasswordHash = nuevoHash;
            PasswordSalt = nuevoSalt;

            if (algoritmo != null)
            {
                PasswordAlg = algoritmo;
            }
            else
            {
                PasswordAlg = "SHA-256";
            }
        }
    }
}
