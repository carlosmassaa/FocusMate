using System;
using Abstracciones;


namespace BE
{
    public class Bitacora
    {
        public int Id { get; set; }
        public DateTime FechaHoraUtc { get; set; }
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public int EntidadId { get; set; }
        public string Resultado { get; set; }
        public string Metadatos { get; set; }
    }
}