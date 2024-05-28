using BE;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BLLUsuario
    {
        public static List<BEUsuario> Listar()
        {
            return MPPUsuario.Listar();
        }

        public static void Login(BEUsuario usuario)
        {
            try
            {
                BEUsuario usuarioExistente = BuscarUsuario(usuario)
                    ?? throw new Exception("Credenciales incorrectas. Por favor vuelva a ingresar los datos correctamente.");

            }
            catch (Exception ex) { throw ex; }
        }

        public static BEUsuario BuscarUsuario(BEUsuario usuario)
        {
            try
            {
                //usuario.Password = Encriptador.Run(usuario.Password);
                return MPPUsuario.BuscarUsuario(usuario);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
