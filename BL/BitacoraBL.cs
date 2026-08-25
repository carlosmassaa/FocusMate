using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BL
{
    public class BitacoraBL
    {
        private readonly BitacoraDal _bitacoraDal;
        private readonly AutorizacionService _autorizacionService;
        private readonly UsuarioDAL _usuarioDal;
        private const string PatenteConsultarBitacora = "AUDITORIA_BITACORA";

        public BitacoraBL(BitacoraDal bitacoraDal, AutorizacionService autorizacionService)
        {
            _bitacoraDal = bitacoraDal ?? throw new ArgumentNullException(nameof(bitacoraDal));
            _autorizacionService = autorizacionService ?? throw new ArgumentNullException(nameof(autorizacionService));
            _usuarioDal = new UsuarioDAL();
        }

        public AutorizacionService Autorizacion => _autorizacionService;

        public static BitacoraBL CrearBasico()
        {
            return new BitacoraBL(new BitacoraDal(), new AutorizacionService());
        }

        public void Registrar(string accion, string entidad, string resultado, string usuario, string modulo, string metadatos)
        {
            Bitacora bitacora = new Bitacora
            {
                FechaHoraUtc = DateTime.UtcNow,
                Accion = accion ?? string.Empty,
                Entidad = entidad ?? string.Empty,
                Resultado = resultado ?? string.Empty,
                Usuario = usuario ?? string.Empty,
                Modulo = modulo ?? string.Empty,
                Metadatos = metadatos ?? string.Empty,
                UsuarioId = 0,
                EntidadId = 0
            };

            if (!string.IsNullOrWhiteSpace(usuario))
            {
                try
                {
                    Usuario usuarioEntidad = _usuarioDal.ObtenerPorNombre(usuario);
                    if (usuarioEntidad != null)
                    {
                        bitacora.UsuarioId = usuarioEntidad.Id;
                    }
                }
                catch
                {
                    bitacora.UsuarioId = 0;
                }
            }

            _bitacoraDal.Guardar(bitacora);
        }

        public List<Bitacora> Buscar()
        {
            return _bitacoraDal.Listar();
        }

        public List<Bitacora> Buscar(BitacoraFiltros filtros, int usuarioSolicitanteId)
        {
            if (!ValidarPermisosConsulta(usuarioSolicitanteId))
            {
                throw new UnauthorizedAccessException("El usuario no posee permisos para consultar la bitácora.");
            }

            if (filtros == null)
            {
                return _bitacoraDal.Listar();
            }

            if (filtros.FechaDesdeUtc.HasValue &&
                filtros.FechaHastaUtc.HasValue &&
                filtros.FechaDesdeUtc.Value > filtros.FechaHastaUtc.Value)
            {
                throw new ArgumentException("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.");
            }

            return _bitacoraDal.BuscarFiltrado(filtros);
        }

        public bool ValidarPermisosConsulta(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return false;
            }

            try
            {
                Usuario usuario = _usuarioDal.Obtener(usuarioId);
                if (usuario != null)
                {
                    _autorizacionService.CargarPermisosEnUsuario(usuario);
                    if (_autorizacionService.TienePermiso(usuario, PatenteConsultarBitacora))
                    {
                        return true;
                    }
                }

                return _usuarioDal.UsuarioTienePermisoDescripcion(usuarioId, PatenteConsultarBitacora);
            }
            catch
            {
                return false;
            }
        }
    }
}
