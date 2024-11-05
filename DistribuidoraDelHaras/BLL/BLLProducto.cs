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
                return MPPProducto.Agregar(producto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Editar(BEProducto producto)
        {
            try
            {
                return MPPProducto.Editar(producto);
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
    }
}
