using BE;
using MPP;
using Servicios.DigitoVerificador;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLProducto
    {
        public static bool Agregar(BEProducto producto)
        {
            try
            {
                producto.DigitoVerificadorH = DigitoVerificador.Run(producto);
                bool agregado = MPPProducto.Agregar(producto);
                RecalcularDigitoVerificadorVertical();
                return agregado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void RecalcularDigitoVerificadorVertical()
        {
            try
            {
                string dvvBitacora = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
                string dvvProducto = DigitoVerificador.RunVertical(BLLProducto.Listar());

                string dvvCalculado = DigitoVerificador.Run(dvvBitacora + dvvProducto);

                MPPProducto.ActualizarDigitoVerificadorVertical(dvvCalculado);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool Editar(BEProducto producto)
        {
            try
            {
                producto.DigitoVerificadorH = DigitoVerificador.Run(producto);
                bool editado = MPPProducto.Editar(producto);
                RecalcularDigitoVerificadorVertical();
                return editado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEProducto> FiltrarPorNombre(string nombre)
        {
            try
            {
                return MPPProducto.FiltrarPorNombre(nombre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BEProducto> Listar()
        {
            try
            {
                return MPPProducto.Listar();
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }

        public static BEProducto ObtenerPorId(int id)
        {
            try
            {
                return MPPProducto.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidarProducto(BEProducto newProduct)
        {
            try
            {
                return newProduct == null || string.IsNullOrEmpty(newProduct.Nombre) || newProduct.Precio <= 0 || string.IsNullOrEmpty(newProduct.Descripcion) || string.IsNullOrEmpty(newProduct.ImagenUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> GetApiProductTest()
        {
            var service = new TestAPIWebService();
            return await service.GetProducts();
        }

        public static void ActualizarDVHProductos()
        {
            try
            {
                List<BEProducto> productos = Listar(); 

                foreach (var producto in productos)
                {
                    if (string.IsNullOrEmpty(producto.DigitoVerificadorH))
                    {
                        producto.DigitoVerificadorH = DigitoVerificador.Run(producto);
                        MPPProducto.ActualizarDVH(producto);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar DVH de productos: {ex.Message}");
            }
        }

    }
}
