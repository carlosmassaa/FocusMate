using System;
using System.Collections.Generic;
using System.IO;
using BE;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace UI
{
    public class AgendaLaboralJsonSerializer
    {
        public void Guardar(string rutaArchivo, AgendaLaboralSerializada agendaSerializada)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("La ruta del archivo es obligatoria.", nameof(rutaArchivo));
            }

            if (agendaSerializada == null)
            {
                throw new InvalidOperationException("No hay una agenda generada para guardar.");
            }

            if (agendaSerializada.Bloques == null || agendaSerializada.Bloques.Count == 0)
            {
                throw new InvalidOperationException("No hay una agenda generada para guardar.");
            }

            JsonSerializerSettings settings = CrearConfiguracion();

            string json = JsonConvert.SerializeObject(agendaSerializada, Formatting.Indented, settings);

            File.WriteAllText(rutaArchivo, json);
        }

        public AgendaLaboralSerializada Cargar(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new ArgumentException("La ruta del archivo es obligatoria.", nameof(rutaArchivo));
            }

            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException("No se encontró el archivo de agenda.", rutaArchivo);
            }

            string json = File.ReadAllText(rutaArchivo);

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            JsonSerializerSettings settings = CrearConfiguracion();

            JToken token = JToken.Parse(json);

            if (token.Type != JTokenType.Object)
            {
                return null;
            }

            AgendaLaboralSerializada agenda = JsonConvert.DeserializeObject<AgendaLaboralSerializada>(json, settings);

            if (agenda == null)
            {
                return null;
            }

            if (agenda.Bloques == null)
            {
                agenda.Bloques = new List<BloqueCalendario>();
            }

            return agenda;
        }

        private JsonSerializerSettings CrearConfiguracion()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }
    }
}