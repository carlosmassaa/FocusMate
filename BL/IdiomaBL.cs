using System.Collections.Generic;
using DAL;
using BE;

namespace BL
{
    public class IdiomaBL
    {
        private readonly IdiomaDAL _idiomaDal;

        public IdiomaBL()
        {
            _idiomaDal = new IdiomaDAL();
        }

        public List<Idioma> ListarIdiomas()
        {
            return _idiomaDal.ListarIdiomas();
        }

        public Dictionary<string, string> ObtenerTraducciones(int idIdioma)
        {
            return _idiomaDal.ObtenerTraducciones(idIdioma);
        }

        public List<EtiquetaTraduccion> ObtenerTraduccionesPorClaves(int idIdioma, IEnumerable<string> claves)
        {
            return _idiomaDal.ObtenerTraduccionesPorClaves(idIdioma, claves);
        }

        public void GuardarTraducciones(int idIdioma, List<EtiquetaTraduccion> filas)
        {
            _idiomaDal.UpsertTraducciones(idIdioma, filas);
        }

        public int CrearIdiomaConPlaceholdersTextoBase(string nombre, string codigoISO)
        {
            return _idiomaDal.CrearIdiomaConPlaceholdersTextoBase(nombre, codigoISO);
        }
    }
}
