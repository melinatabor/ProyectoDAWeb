﻿using BE;
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
                if (HuboModificacionesExternas())
                    throw new Exception("Se detectaron modificaciones en la base de datos. Por favor contacte al administrador.");

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

        private static bool HuboModificacionesExternas()
        {
            // Calcular los DVV actuales para Bitacora y Productos
            string dvvBitacoraActual = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
            string dvvProductoActual = DigitoVerificador.RunVertical(BLLProducto.Listar());

            string dvvCalculado = DigitoVerificador.Run(dvvBitacoraActual + dvvProductoActual);

            // Obtener el DVV guardado en la tabla DigitoVerificadorVertical
            string dvvGuardado = MPPUsuario.ObtenerDigitoVerificadorVertical();

            // Comparar el DVV calculado con el guardado
            return dvvCalculado != dvvGuardado;
        }
    }
}
