using System;

namespace Abstracciones
{
    
    public interface IFiltrosBitacora
    {
        DateTime? FechaDesdeUtc { get; set; }
        DateTime? FechaHastaUtc { get; set; }
        int? UsuarioId { get; set; }
        string Usuario { get; set; }
        string Modulo { get; set; }
        string Accion { get; set; }
        string Entidad { get; set; }
        int? EntidadId { get; set; }
        string Resultado { get; set; }
        string TextoLibre { get; set; }
    }
}