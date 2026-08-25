using System;
using System.Collections.Generic;
using System.IO;
using BE;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UI
{
    public class AgendaLaboralPdfExporter
    {
        public void Exportar(string rutaArchivo, string nombreUsuario, string resumen, List<BloqueCalendario> agenda)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("La ruta del archivo PDF es obligatoria.", nameof(rutaArchivo));
            }

            if (agenda == null || agenda.Count == 0)
            {
                throw new InvalidOperationException("No hay una agenda generada para exportar.");
            }

            Document documento = new Document(PageSize.A4.Rotate(), 36, 36, 36, 36);

            using (FileStream stream = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter.GetInstance(documento, stream);

                documento.Open();

                Font fuenteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Font fuenteSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                Font fuenteNormal = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                Font fuenteTablaHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                Font fuenteTabla = FontFactory.GetFont(FontFactory.HELVETICA, 8);

                Paragraph titulo = new Paragraph("Agenda laboral generada", fuenteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 12;

                documento.Add(titulo);

                PdfPTable tablaDatos = new PdfPTable(2);
                tablaDatos.WidthPercentage = 100;
                tablaDatos.SetWidths(new float[] { 20f, 80f });

                string nombreUsuarioTexto = string.Empty;
                string resumenTexto = string.Empty;

                if (string.IsNullOrWhiteSpace(nombreUsuario))
                {
                    nombreUsuarioTexto = "-";
                }
                else
                {
                    nombreUsuarioTexto = nombreUsuario;
                }

                if (string.IsNullOrWhiteSpace(resumen))
                {
                    resumenTexto = "-";
                }
                else
                {
                    resumenTexto = resumen;
                }

                AgregarCeldaDato(tablaDatos, "Usuario", fuenteSubtitulo);
                AgregarCeldaDato(tablaDatos, nombreUsuarioTexto, fuenteNormal);

                AgregarCeldaDato(tablaDatos, "Fecha de generación", fuenteSubtitulo);
                AgregarCeldaDato(tablaDatos, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fuenteNormal);

                AgregarCeldaDato(tablaDatos, "Resumen", fuenteSubtitulo);
                AgregarCeldaDato(tablaDatos, resumenTexto, fuenteNormal);

                documento.Add(tablaDatos);

                Paragraph espacio = new Paragraph(" ");
                espacio.SpacingAfter = 8;

                documento.Add(espacio);

                Paragraph subtitulo = new Paragraph("Detalle de bloques", fuenteSubtitulo);
                subtitulo.SpacingAfter = 8;

                documento.Add(subtitulo);

                PdfPTable tabla = new PdfPTable(7);
                tabla.WidthPercentage = 100;
                tabla.SetWidths(new float[] { 12f, 11f, 12f, 14f, 31f, 10f, 10f });

                AgregarEncabezado(tabla, "Fecha", fuenteTablaHeader);
                AgregarEncabezado(tabla, "Día", fuenteTablaHeader);
                AgregarEncabezado(tabla, "Horario", fuenteTablaHeader);
                AgregarEncabezado(tabla, "Tipo", fuenteTablaHeader);
                AgregarEncabezado(tabla, "Título", fuenteTablaHeader);
                AgregarEncabezado(tabla, "Duración", fuenteTablaHeader);
                AgregarEncabezado(tabla, "Score", fuenteTablaHeader);

                List<BloqueCalendario> agendaOrdenada = new List<BloqueCalendario>(agenda);

                agendaOrdenada.Sort(CompararBloques);

                foreach (BloqueCalendario bloque in agendaOrdenada)
                {
                    string scoreTexto = string.Empty;

                    if (bloque.ScorePrioridad.HasValue)
                    {
                        scoreTexto = bloque.ScorePrioridad.Value.ToString("0.##");
                    }
                    else
                    {
                        scoreTexto = "-";
                    }

                    AgregarCelda(tabla, FormatearFecha(bloque), fuenteTabla);
                    AgregarCelda(tabla, FormatearDia(bloque), fuenteTabla);
                    AgregarCelda(tabla, FormatearHorario(bloque), fuenteTabla);
                    AgregarCelda(tabla, bloque.TipoBloque.ToString(), fuenteTabla);
                    AgregarCelda(tabla, bloque.Titulo, fuenteTabla);
                    AgregarCelda(tabla, FormatearDuracion(bloque), fuenteTabla);
                    AgregarCelda(tabla, scoreTexto, fuenteTabla);
                }

                documento.Add(tabla);
                documento.Close();
            }
        }

        private void AgregarCeldaDato(PdfPTable tabla, string texto, Font fuente)
        {
            string textoCelda = string.Empty;

            if (texto == null)
            {
                textoCelda = "-";
            }
            else
            {
                textoCelda = texto;
            }

            PdfPCell celda = new PdfPCell(new Phrase(textoCelda, fuente));
            celda.Padding = 5;
            celda.BorderWidth = 0.5f;

            tabla.AddCell(celda);
        }

        private void AgregarEncabezado(PdfPTable tabla, string texto, Font fuente)
        {
            PdfPCell celda = new PdfPCell(new Phrase(texto, fuente));
            celda.Padding = 5;
            celda.HorizontalAlignment = Element.ALIGN_CENTER;
            celda.BorderWidth = 0.5f;

            tabla.AddCell(celda);
        }

        private void AgregarCelda(PdfPTable tabla, string texto, Font fuente)
        {
            string textoCelda = string.Empty;

            if (texto == null)
            {
                textoCelda = "-";
            }
            else
            {
                textoCelda = texto;
            }

            PdfPCell celda = new PdfPCell(new Phrase(textoCelda, fuente));
            celda.Padding = 4;
            celda.BorderWidth = 0.5f;

            tabla.AddCell(celda);
        }

        private int CompararBloques(BloqueCalendario primerBloque, BloqueCalendario segundoBloque)
        {
            int comparacionFecha = primerBloque.Fecha.CompareTo(segundoBloque.Fecha);

            if (comparacionFecha != 0)
            {
                return comparacionFecha;
            }

            int comparacionHora = primerBloque.HoraInicio.CompareTo(segundoBloque.HoraInicio);

            if (comparacionHora != 0)
            {
                return comparacionHora;
            }

            return primerBloque.TipoBloque.CompareTo(segundoBloque.TipoBloque);
        }

        private string FormatearFecha(BloqueCalendario bloque)
        {
            if (bloque.Fecha == DateTime.MinValue)
            {
                return "-";
            }

            return bloque.Fecha.ToString("dd/MM/yyyy");
        }

        private string FormatearDia(BloqueCalendario bloque)
        {
            if (bloque.DiaSemana <= 0)
            {
                return "-";
            }
            else if (bloque.DiaSemana == 1)
            {
                return "Lunes";
            }
            else if (bloque.DiaSemana == 2)
            {
                return "Martes";
            }
            else if (bloque.DiaSemana == 3)
            {
                return "Miércoles";
            }
            else if (bloque.DiaSemana == 4)
            {
                return "Jueves";
            }
            else if (bloque.DiaSemana == 5)
            {
                return "Viernes";
            }
            else if (bloque.DiaSemana == 6)
            {
                return "Sábado";
            }
            else
            {
                return "Domingo";
            }
        }

        private string FormatearHorario(BloqueCalendario bloque)
        {
            if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
            {
                return "-";
            }

            return bloque.HoraInicio.ToString(@"hh\:mm") + " - " + bloque.HoraFin.ToString(@"hh\:mm");
        }

        private string FormatearDuracion(BloqueCalendario bloque)
        {
            if (bloque.TipoBloque == TipoBloqueCalendario.TareaSinUbicar)
            {
                return "-";
            }

            return bloque.DuracionMinutos.ToString() + " min";
        }
    }
}