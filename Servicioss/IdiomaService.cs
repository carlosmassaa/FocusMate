using System;
using System.Collections.Generic;

namespace Servicioss
{
    public sealed class IdiomaService
    {
        private static readonly Lazy<IdiomaService> instanciaLazy = new Lazy<IdiomaService>(() => new IdiomaService());
        public static IdiomaService Instancia => instanciaLazy.Value;

        private readonly List<IIdiomaObserver> observadores = new List<IIdiomaObserver>();
        private Dictionary<string, string> traduccionesActuales = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private IdiomaService() { }

        public void Suscribir(IIdiomaObserver observador)
        {
            if (observador == null)
            {
                return;
            }

            if (!observadores.Contains(observador))
            {
                observadores.Add(observador);
                if (traduccionesActuales.Count > 0)
                {
                    observador.ActualizarTraducciones(new Dictionary<string, string>(traduccionesActuales));
                }
            }
        }

        public void Desuscribir(IIdiomaObserver observador)
        {
            observadores.Remove(observador);
        }

        public void CambiarIdioma(Dictionary<string, string> nuevasTraducciones)
        {
            if (nuevasTraducciones == null)
            {
                return;
            }

            traduccionesActuales = new Dictionary<string, string>(nuevasTraducciones, StringComparer.OrdinalIgnoreCase);
            NotificarObservadores();
        }

        public string Traducir(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave))
            {
                return string.Empty;
            }

            string valorTraduccion;
            if (traduccionesActuales.TryGetValue(clave, out valorTraduccion))
            {
                return valorTraduccion;
            }
            else
            {
                return clave;
            }
        }

        public Dictionary<string, string> ObtenerTraduccionesActuales()
        {
            return new Dictionary<string, string>(traduccionesActuales);
        }

        private void NotificarObservadores()
        {
            foreach (IIdiomaObserver observador in observadores.ToArray())
            {
                observador.ActualizarTraducciones(traduccionesActuales);
            }
        }
    }
}
