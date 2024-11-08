using Abstraccion;
using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class CSVHelper
    {
        public static string ConvertirProductoFormatoCSV(BEProducto producto)
        {
            return $"{producto.Nombre},{producto.Precio},{producto.Descripcion},{producto.ImagenUrl}";
        }

        public static string ConvertirBitacoraFormatoCSV(BEBitacora bitacora)
        {
            return $"{bitacora.Usuario},{bitacora.Fecha},{bitacora.Mensaje}";
        }
    }
}
