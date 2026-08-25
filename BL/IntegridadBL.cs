using BE;
using DAL;
using Servicioss;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BL
{
    public class IntegridadBL
    {
        private readonly CryptoService cryptoService = new CryptoService();
        private readonly BitacoraBL servicioBitacora = BitacoraBL.CrearBasico();

        private readonly TareaDAL tareaDal;
        private readonly DigitoVerificadorDAL digitoVerificadorDal;

        public IntegridadBL()
        {
            tareaDal = new TareaDAL();
            digitoVerificadorDal = new DigitoVerificadorDAL();
        }

        public void RecalcularDVV_Tarea()
        {
            List<Tarea> tareas = tareaDal.Listar();

            string[] dvhStringsOrdenados = tareas
                .OrderBy(tarea => tarea.TareaId)
                .Select(tarea => tarea.DVH.ToString(CultureInfo.InvariantCulture))
                .ToArray();

            long digitoVerificadorVertical = cryptoService.CalcularDVV(dvhStringsOrdenados);

            digitoVerificadorDal.UpsertDVV("Tarea", digitoVerificadorVertical);

            string nombreUsuario = ObtenerNombreUsuarioActual();
            servicioBitacora.Registrar("RECALCULAR_DVV", "Tarea", "OK", nombreUsuario, "Integridad", "DVV=" + digitoVerificadorVertical);
        }

        public bool VerificarTarea(out string detalle)
        {
            detalle = string.Empty;

            List<Tarea> tareas = tareaDal.Listar();

            string[] dvhStringsOrdenados = tareas
                .OrderBy(tarea => tarea.TareaId)
                .Select(tarea => tarea.DVH.ToString(CultureInfo.InvariantCulture))
                .ToArray();

            long digitoVerificadorVerticalCalculado = cryptoService.CalcularDVV(dvhStringsOrdenados);
            long digitoVerificadorVerticalGuardado = digitoVerificadorDal.ObtenerDVV("Tarea");

            if (digitoVerificadorVerticalCalculado != digitoVerificadorVerticalGuardado)
            {
                detalle = "DVV inconsistente (calc=" + digitoVerificadorVerticalCalculado + ", guardado=" + digitoVerificadorVerticalGuardado + ").";

                string nombreUsuario = ObtenerNombreUsuarioActual();
                servicioBitacora.Registrar("VERIFICAR_DVV", "Tarea", "FAIL", nombreUsuario, "Integridad", detalle);

                return false;
            }

            return true;
        }

        public void RepararTarea()
        {
            TareaBL tareaBl = new TareaBL();
            tareaBl.RepararDVH_Tarea();
            RecalcularDVV_Tarea();

            string nombreUsuario = ObtenerNombreUsuarioActual();

            servicioBitacora.Registrar("REPARAR_INTEGRIDAD", "Tarea", "OK", nombreUsuario, "Integridad", "Reparadas DVH y DVV recalculado");
        }

        private string ObtenerNombreUsuarioActual()
        {
            if (SesionActual.Instance == null)
            {
                return string.Empty;
            }

            return SesionActual.Instance.NombreUsuario;
        }
    }
}
