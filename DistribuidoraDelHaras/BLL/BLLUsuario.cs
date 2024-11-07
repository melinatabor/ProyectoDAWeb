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
                return MPPUsuario.Agregar(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool Editar(BEUsuario usuario)
        {
            try
            {
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

                //Verificar si hubo modificaciones en Bitacora o Productos
                string mensajeModificaciones = HuboModificacionesExternas();
                if (!string.IsNullOrEmpty(mensajeModificaciones))
                    throw new Exception($"Se detectaron modificaciones en la base de datos. {mensajeModificaciones}. Por favor contacte al administrador.");

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

        private static string HuboModificacionesExternas()
        {
            // Calcular los DVV actuales de Bitacora y Productos con los guardados
            string dvvBitacoraActual = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
            string dvvBitacoraGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_BITACORA);

            string dvvProductoActual = DigitoVerificador.RunVertical(BLLProducto.Listar());
            string dvvProductoGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_PRODUCTO);

            List<string> modificaciones = new List<string>();

            if (dvvBitacoraActual != dvvBitacoraGuardado)
                modificaciones.Add(BEDigitoVerificador.ENTIDAD_BITACORA);

            if (dvvProductoActual != dvvProductoGuardado)
                modificaciones.Add(BEDigitoVerificador.ENTIDAD_PRODUCTO);

            if (modificaciones.Count > 0)
                return $"Las siguientes tablas han sido modificadas externamente: {string.Join(" y ", modificaciones)}";

            return string.Empty;
        }
    }
}
