using BE;
using MPP;
using Servicios;
using Servicios.DigitoVerificador;
using System;
using System.Collections.Generic;
using static BE.BEBitacora;

namespace BLL
{
    public class BLLUsuario
    {
        public static bool Agregar(BEUsuario usuario)
        {
            try
            {
                usuario.Activo = true;
                usuario.Password = Encriptador.Run(usuario.Password);
                usuario.DigitoVerificadorH = DigitoVerificador.Run(usuario);
                return MPPUsuario.Agregar(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        public static void RecalcularDigitoVerificadorVertical()
        {
            try
            {
                string dvvCalculado = DigitoVerificador.RunVertical(BLLUsuario.Listar());
                MPPUsuario.ActualizarDigitoVerificadorVertical(dvvCalculado);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool Editar(BEUsuario usuario)
        {
            try
            {
                usuario.DigitoVerificadorH = DigitoVerificador.Run(usuario);
                return MPPUsuario.Editar(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<BEUsuario> Listar()
        {
            return MPPUsuario.Listar();
        }

        public static BEUsuario Login(BEUsuario usuario)
        {
            try
            {
                BEUsuario usuarioExistente = BuscarUsuario(usuario)
                    ?? throw new Exception("Credenciales incorrectas. Por favor vuelva a ingresar los datos correctamente.");

                if (HuboModificacionesExternas(usuarioExistente))
                    throw new Exception($"El usuario {usuarioExistente.Nombre} ha sido modificado. Por favor contacte al administrador.");

                if (InformacionCorrupta())
                    throw new Exception("La base de datos ha sido modificada. Por favor contacte al administrador.");

                SesionManager.Login(usuarioExistente);

                BEBitacora bitacora = new BEBitacora()
                {
                    Usuario = usuarioExistente.Id,
                    Tipo = BitacoraTipo.INFO,
                    Mensaje = "El usuario inició sesión"
                };

                BLLBitacora.Agregar(bitacora);

                return usuarioExistente;
            }
            catch (Exception ex) { throw ex; }
        }

        public static BEUsuario BuscarUsuario(BEUsuario usuario)
        {
            try
            {
                usuario.Password = Encriptador.Run(usuario.Password);
                return MPPUsuario.BuscarUsuario(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        private static bool InformacionCorrupta()
        {
            // Generar Digito Verificador Vertical con los datos de la tabla de usuarios.
            // El valor dvvCalculado deberia coincidir con el de la tabla DigitoVerificadorVertical
            string dvvCalculado = DigitoVerificador.RunVertical(BLLUsuario.Listar());
            return dvvCalculado != MPPUsuario.ObtenerDigitoVerificadorVertical();
        }


        private static bool HuboModificacionesExternas(BEUsuario usuarioExistente)
        {
            return usuarioExistente.DigitoVerificadorH != DigitoVerificador.Run(usuarioExistente);
        }
    }
}
