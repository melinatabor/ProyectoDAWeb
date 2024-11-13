using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BEPermisoXUsuario
    {
        public int UsuarioId { get; set; }
        public string PermisosPadre { get; set; }
        public string PermisosHijos { get; set; }

    }
}
