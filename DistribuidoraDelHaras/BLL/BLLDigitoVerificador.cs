using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLDigitoVerificador
    {
        public static void ActualizarDigitoVerificadorVertical(string dvvCalculado, string entidad)
        {
			try
			{
                MPPDigitoVerificador.ActualizarDVV(dvvCalculado, entidad);
            }
			catch (Exception ex)
			{
				throw ex;
			}
        }

        internal static string ObtenerDigitoVerificadorVertical(string entidad)
        {
            try
            {
                return MPPDigitoVerificador.ObtenerDigitoVerificadorVertical(entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
