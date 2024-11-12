using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraccion
{

    public interface IUsuario
    {
        int Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        List<int> ListaPermisos { get; set; }
    }
}
