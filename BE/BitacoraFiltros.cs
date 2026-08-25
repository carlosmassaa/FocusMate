using System;
using Abstracciones;

namespace BE
{
    public class BitacoraFiltros : IFiltrosBitacora
    {
        public DateTime? FechaDesdeUtc { get; set; }
        public DateTime? FechaHastaUtc { get; set; }
        public int? UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public int? EntidadId { get; set; }
        public string Resultado { get; set; }
        public string TextoLibre { get; set; }

        public BitacoraFiltros()
        {
            Usuario = string.Empty;
            Modulo = string.Empty;
            Accion = string.Empty;
            Entidad = string.Empty;
            Resultado = string.Empty;
            TextoLibre = string.Empty;
        }

        public bool TieneFiltros()
        {
            return FechaDesdeUtc.HasValue || FechaHastaUtc.HasValue || UsuarioId.HasValue || !string.IsNullOrWhiteSpace(Usuario) || !string.IsNullOrWhiteSpace(Modulo) || !string.IsNullOrWhiteSpace(Accion) || !string.IsNullOrWhiteSpace(Entidad) || EntidadId.HasValue || !string.IsNullOrWhiteSpace(Resultado) || !string.IsNullOrWhiteSpace(TextoLibre);
        }
    }
}