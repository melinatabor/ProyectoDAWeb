using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEAuditoriaCambios
    {
        public const string ENTIDAD_PRODUCTO = "Producto";
        public const string ENTIDAD_BITACORA = "Bitacora";

        public const string OPERACION_ALTA = "Alta";
        public const string OPERACION_MODIFICACION = "Modificacion";
        public const string OPERACION_ELIMINACION = "Eliminacion";

        public int Id { get; set; }
        public string Entidad { get; set; }
        public int IdRegistroAfectado { get; set; }
        public string Operacion { get; set; }
        public DateTime Fecha { get; set; }
        public string DatosAntes { get; set; }
        public string DatosDespues { get; set; }
    }
}
