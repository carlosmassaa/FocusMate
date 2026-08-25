using System.Collections.Generic;

namespace Servicioss
{
    public interface IIdiomaObserver
    {
        void ActualizarTraducciones(Dictionary<string, string> traducciones);
    }
}