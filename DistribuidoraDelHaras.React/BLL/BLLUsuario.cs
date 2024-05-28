using BE;
using MPP;
using System.Collections.Generic;

namespace BLL
{
    public class BLLUsuario
    {
        public static List<BEUsuario> Listar()
        {
            return MPPUsuario.Listar();
        }
    }
}
