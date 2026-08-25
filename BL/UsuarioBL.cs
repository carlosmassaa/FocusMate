using System;
using System.Collections.Generic;
using BE;
using DAL;
using Servicioss;

namespace BL
{
    public class UsuarioBL
    {
        private readonly UsuarioDAL _usuarioDal;

        public UsuarioBL()
        {
            _usuarioDal = new UsuarioDAL();
        }

        public List<Usuario> Listar()
        {
            return _usuarioDal.Listar();
        }

        public Usuario Obtener(int usuarioId)
        {
            return _usuarioDal.Obtener(usuarioId);
        }

        public bool RegistrarUsuario(string nombreUsuario, string password)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            Usuario usuarioExistente = _usuarioDal.ObtenerPorNombre(nombreUsuario);
            if (usuarioExistente != null)
            {
                return false;
            }

            CryptoService cryptoService = new CryptoService();
            byte[] salt = cryptoService.GenerarSalt();
            byte[] hash = cryptoService.CalcularHash(password, salt);

            Usuario usuarioNuevo = new Usuario();
            usuarioNuevo.NombreUsuario = nombreUsuario;
            usuarioNuevo.EstaActivo = true;
            usuarioNuevo.CreadoUtc = DateTime.UtcNow;
            usuarioNuevo.FailedAttempts = 0;
            usuarioNuevo.BloqueadoHastaUtc = DateTime.MinValue;
            usuarioNuevo.EstablecerCredencialesPassword(hash, salt, "SHA-256");

            _usuarioDal.GuardarNuevo(usuarioNuevo);
            return true;
        }

        public bool EliminarUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return false;
            }

            Usuario usuario = _usuarioDal.Obtener(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            usuario.EstaActivo = false;
            usuario.ActualizadoUtc = DateTime.UtcNow;
            _usuarioDal.Guardar(usuario);
            return true;
        }

        public bool BloquearUsuario(int usuarioId, DateTime? hastaUtc = null)
        {
            if (usuarioId <= 0)
            {
                return false;
            }

            Usuario usuario = _usuarioDal.Obtener(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            DateTime fechaBloqueoDestino = hastaUtc ?? DateTime.UtcNow.AddYears(100);
            usuario.BloqueadoHastaUtc = fechaBloqueoDestino;

            if (usuario.FailedAttempts < 1)
            {
                usuario.FailedAttempts = 1;
            }

            usuario.ActualizadoUtc = DateTime.UtcNow;

            _usuarioDal.Guardar(usuario);
            return true;
        }

        public bool DesbloquearUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return false;
            }

            Usuario usuario = _usuarioDal.Obtener(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            usuario.BloqueadoHastaUtc = DateTime.MinValue;
            usuario.FailedAttempts = 0;
            usuario.ActualizadoUtc = DateTime.UtcNow;

            _usuarioDal.Guardar(usuario);
            return true;
        }

        public bool ActivarUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return false;
            }

            Usuario usuario = _usuarioDal.Obtener(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            if (usuario.EstaActivo)
            {
                return true;
            }

            usuario.EstaActivo = true;
            usuario.ActualizadoUtc = DateTime.UtcNow;
            _usuarioDal.Guardar(usuario);
            return true;
        }

        public bool EstablecerIdiomaPorDefecto(int usuarioId, int idiomaId)
        {
            if (usuarioId <= 0 || idiomaId <= 0)
            {
                return false;
            }

            Usuario usuario = _usuarioDal.Obtener(usuarioId);
            if (usuario == null)
            {
                return false;
            }

            usuario.IdiomaId = idiomaId;
            usuario.ActualizadoUtc = DateTime.UtcNow;
            _usuarioDal.Guardar(usuario);
            return true;
        }
    }
}
