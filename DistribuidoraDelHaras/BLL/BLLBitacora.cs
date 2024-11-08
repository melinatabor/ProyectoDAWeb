using BE;
using MPP;
using Servicios;
using Servicios.DigitoVerificador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBitacora
    {
        public static bool Agregar(BEBitacora bitacora)
        {
            bitacora.DigitoVerificadorH = DigitoVerificador.Run(bitacora);
            bitacora.Fecha = DateTime.Now;
            bool agregado = MPPBitacora.Agregar(bitacora);
            RecalcularDigitoVerificadorVertical();

            if (agregado)
            {
                int idBitacora = MPPBitacora.ObtenerUltimoId();

                bitacora.Id = idBitacora;
                string datosDespuesString = CSVHelper.ConvertirBitacoraFormatoCSV(bitacora);

                BLLAuditoriaCambios.Agregar(new BEAuditoriaCambios
                {
                    Entidad = BEAuditoriaCambios.ENTIDAD_BITACORA,
                    IdRegistroAfectado = idBitacora,
                    Operacion = BEAuditoriaCambios.OPERACION_ALTA,
                    Fecha = DateTime.Now,
                    DatosAntes = null,
                    DatosDespues = datosDespuesString
                });
            }

            return agregado;
        }

        public static List<BEBitacoraFiltrada> Filtrar(BEBitacoraCriteria criteria)
        {
            return MPPBitacora.Filtrar(criteria);
        }

        private static void RecalcularDigitoVerificadorVertical()
        {
            try
            {
                string dvvBitacora = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
                BLLDigitoVerificador.ActualizarDigitoVerificadorVertical(dvvBitacora, BEDigitoVerificador.ENTIDAD_BITACORA);
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<BEBitacora> ListarTodo()
        {
            try
            {
                return MPPBitacora.ListarTodo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RecalcularDVH()
        {
            List<BEBitacora> bitacoras = ListarTodo();

            foreach (var bitacora in bitacoras)
            {
                bitacora.DigitoVerificadorH = DigitoVerificador.Run(bitacora);
                MPPBitacora.ActualizarDVH(bitacora);
            }
        }
    }
}
