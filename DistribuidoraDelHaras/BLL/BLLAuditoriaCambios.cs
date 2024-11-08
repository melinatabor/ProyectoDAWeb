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
    public class BLLAuditoriaCambios
    {
        public static bool Agregar(BEAuditoriaCambios auditoriaCambios)
        {
            try
            {
                return MPPAuditoriaCambios.Agregar(auditoriaCambios);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Eliminar()
        {
            try
            {
                MPPAuditoriaCambios.Eliminar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void InsertarProductosIniciales()
        {
            try
            {
                var productos = MPPProducto.Listar();

                foreach (var producto in productos)
                {
                    var auditoria = new BEAuditoriaCambios
                    {
                        Entidad = BEAuditoriaCambios.ENTIDAD_PRODUCTO,
                        IdRegistroAfectado = producto.Id,
                        Operacion = BEAuditoriaCambios.OPERACION_ALTA,
                        Fecha = DateTime.Now,
                        DatosAntes = null,
                        DatosDespues = CSVHelper.ConvertirProductoFormatoCSV(producto)
                    };

                    Agregar(auditoria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static List<BEAuditoriaCambios> ListarPorEntidad(string entidad)
        {
            try
            {
                return MPPAuditoriaCambios.ListarPorEntidad(entidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
