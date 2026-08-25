using System;
using Abstracciones;

namespace Servicioss
{
    public sealed class SesionActual
    {
        private static readonly Lazy<SesionActual> instancia = new Lazy<SesionActual>(CrearSesionActual);

        public static SesionActual Instance
        {
            get
            {
                return instancia.Value;
            }
        }

        private SesionActual()
        {
        }

        private static SesionActual CrearSesionActual()
        {
            return new SesionActual();
        }

        public bool EstaAutenticado { get; private set; }
        public int UsuarioId { get; private set; }
        public string NombreUsuario { get; private set; }
        public DateTime InicioUtc { get; private set; }
        public IUsuario UsuarioActual { get; private set; }

        public void IniciarPorDatos(int usuarioId, string nombreUsuario)
        {
            EstaAutenticado = true;
            UsuarioId = usuarioId;
            NombreUsuario = nombreUsuario;
            UsuarioActual = null;
            InicioUtc = DateTime.UtcNow;
        }

        public void Cerrar()
        {
            EstaAutenticado = false;
            UsuarioId = 0;
            NombreUsuario = null;
            UsuarioActual = null;
        }
    }
}
