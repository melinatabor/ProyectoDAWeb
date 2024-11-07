using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEProducto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
        public string DigitoVerificadorH { get; set; }

        public BEProducto()
        {

        }
    }
}
