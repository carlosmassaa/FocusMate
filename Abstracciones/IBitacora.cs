using System;

namespace Abstracciones
{
    public interface IBitacora
    {
        int Id { get; set; }
        DateTime FechaHoraUtc { get; set; }
        int UsuarioId { get; set; }
        string Usuario { get; set; }
        string Modulo { get; set; }
        string Accion { get; set; }
        string Entidad { get; set; }
        int EntidadId { get; set; }
        string Resultado { get; set; }
        string Metadatos { get; set; }
    }
}