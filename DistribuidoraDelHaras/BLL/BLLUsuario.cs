﻿using BE;
using MPP;
using Servicios;
using Servicios.DigitoVerificador;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
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
                BEUsuario usuarioUsername = BuscarUsuarioPorUsuername(usuario);

                BEUsuario usuarioExistente = BuscarUsuario(usuario);

                if (usuarioUsername != null && usuarioUsername.Password != usuarioExistente?.Password)
                {
                    MPPUsuario.RegistrarIntentoFallido(usuarioUsername.Id);
                    if (VerificarIntentosFallidos(usuarioUsername.Id))
                        throw new Exception("Cuenta bloqueada por múltiples intentos fallidos.");
                }

                if (usuarioExistente == null)
                {
                    throw new Exception("Credenciales incorrectas. Por favor vuelva a ingresar los datos correctamente.");
                }

                if (usuarioExistente.IsLocked)
                    throw new Exception("Cuenta bloqueada. Contacte al administrador.");


                if (IsCorrupted())
                {
                    if (UsuarioEsMaster(usuarioExistente))
                    {
                        string mensajeModificaciones = VerificarModificacionesYEliminacionesExternas();
                        if (!string.IsNullOrEmpty(mensajeModificaciones))
                            throw new Exception($"{mensajeModificaciones}");
                    }
                    else
                    {
                        throw new Exception($"2");
                    }
                }

                ObtenerPermisosUsuario(usuarioExistente);

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

        private static bool UsuarioEsMaster(BEUsuario usuario)
        {
            try
            {
                return MPPUsuario.UsuarioEsMaster(usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ObtenerPermisosUsuario(BEUsuario usuario)
        {
            try
            {
                MPPUsuario.ObtenerPermisosUsuario(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<BEPermiso> ObtenerPermisosUsuario(int usuarioId)
        {
            try
            {
                return MPPUsuario.ObtenerPermisosUsuario(usuarioId);
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

        public static bool IsCorrupted()
        {
            string dvvBitacoraActual = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
            string dvvProductoActual = DigitoVerificador.RunVertical(BLLProducto.Listar());

            string dvvBitacoraGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_BITACORA);
            string dvvProductoGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_PRODUCTO);

            bool bitacoraModificada = dvvBitacoraActual != dvvBitacoraGuardado;
            bool productoModificado = dvvProductoActual != dvvProductoGuardado;

            return bitacoraModificada || productoModificado;
        }

        public static string VerificarModificacionesYEliminacionesExternas()
        {
            var mensajesDeCambio = new List<string>
            {
                "1"
            };
            string dvvBitacoraActual = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
            string dvvProductoActual = DigitoVerificador.RunVertical(BLLProducto.Listar());

            string dvvBitacoraGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_BITACORA);
            string dvvProductoGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_PRODUCTO);

            bool bitacoraModificada = dvvBitacoraActual != dvvBitacoraGuardado;
            bool productoModificado = dvvProductoActual != dvvProductoGuardado;

            if (bitacoraModificada)
            {
                List<BEAuditoriaCambios> registrosAuditoria = BLLAuditoriaCambios.ListarPorEntidad(BEAuditoriaCambios.ENTIDAD_BITACORA);
                var registrosActuales = BLLBitacora.ListarParaVerificarCambios();

                foreach (BEBitacora registro in registrosActuales)
                {
                    var auditoria = registrosAuditoria.FirstOrDefault(a => a.IdRegistroAfectado == registro.Id);
                    if (auditoria != null && !CompararDatosCSV(auditoria.DatosAntes, auditoria.DatosDespues, registro, auditoria.Operacion))
                    {
                        mensajesDeCambio.Add($"\nRegistro modificado en Bitacora (ID: {registro.Id})");
                    }
                }

                foreach (var auditoria in registrosAuditoria)
                {
                    if (!registrosActuales.Any(r => r.Id == auditoria.IdRegistroAfectado))
                        mensajesDeCambio.Add($"\nRegistro eliminado en Bitacora (ID: {auditoria.IdRegistroAfectado})");
                }
            }

            if (productoModificado)
            {
                List<BEAuditoriaCambios> registrosAuditoria = BLLAuditoriaCambios.ListarPorEntidad(BEAuditoriaCambios.ENTIDAD_PRODUCTO);
                var registrosActuales = BLLProducto.Listar();

                foreach (BEProducto registro in registrosActuales)
                {
                    var auditoria = registrosAuditoria.FirstOrDefault(a => a.IdRegistroAfectado == registro.Id);
                    if (auditoria != null && !CompararDatosCSV(auditoria.DatosAntes, auditoria.DatosDespues, registro, auditoria.Operacion))
                    {
                        mensajesDeCambio.Add($"\nRegistro modificado en Producto (ID: {registro.Id}): {ObtenerCamposModificadosCSV(auditoria.DatosAntes, registro)}");
                    }
                }

                foreach (var auditoria in registrosAuditoria)
                {
                    if (!registrosActuales.Any(r => r.Id == auditoria.IdRegistroAfectado))
                        mensajesDeCambio.Add($"\nRegistro eliminado en Producto (ID: {auditoria.IdRegistroAfectado})");
                }
            }

            return string.Join(",", mensajesDeCambio);
        }

        private static bool CompararDatosCSV(string datosAntesCSV, string datosDespuesCSV, object registroActual, string operacion)
        {
            if (operacion == BEAuditoriaCambios.OPERACION_ALTA && string.IsNullOrWhiteSpace(datosAntesCSV))
            {
                string datosActualCSV = "";

                if (registroActual is BEProducto)
                    datosActualCSV = CSVHelper.ConvertirProductoFormatoCSV((BEProducto)registroActual);
                
                if (registroActual is BEBitacora)
                    datosActualCSV = CSVHelper.ConvertirBitacoraFormatoCSV((BEBitacora)registroActual);

                return datosActualCSV == datosDespuesCSV;
            }

            if (string.IsNullOrWhiteSpace(datosAntesCSV))
                return false;

            var valoresAntes = datosAntesCSV.Split(',');

            if (registroActual is BEProducto)
            {
                if (valoresAntes[0] != ((BEProducto)registroActual).Nombre.ToString())
                    return false;

                if (valoresAntes[1] != ((BEProducto)registroActual).Precio.ToString())
                    return false;

                if (valoresAntes[2] != ((BEProducto)registroActual).Descripcion.ToString()) 
                    return false;

                if (valoresAntes[3] != ((BEProducto)registroActual).ImagenUrl.ToString())
                    return false;
            }

            if (registroActual is BEBitacora)
            {
                if (valoresAntes[0] != ((BEBitacora)registroActual).Usuario.ToString())
                    return false;

                if (valoresAntes[1] != ((BEBitacora)registroActual).Fecha.ToString())
                    return false;

                if (valoresAntes[2] != ((BEBitacora)registroActual).Mensaje.ToString())
                    return false;

                return false;
            }

            return true;
        }

        private static string ObtenerCamposModificadosCSV(string datosAntesCSV, object registroActual)
        {
            if (string.IsNullOrEmpty(datosAntesCSV))
                return string.Empty;

            var cambios = new List<string>();
            var valoresAntes = datosAntesCSV.Split(',');

            if (registroActual is BEProducto)
            {
                if (valoresAntes[0] != ((BEProducto)registroActual).Nombre.ToString())
                    cambios.Add("Nombre");

                if (valoresAntes[1] != ((BEProducto)registroActual).Precio.ToString())
                    cambios.Add("Precio");

                if (valoresAntes[2] != ((BEProducto)registroActual).Descripcion.ToString())
                    cambios.Add("Descripcion");

                if (valoresAntes[3] != ((BEProducto)registroActual).ImagenUrl.ToString())
                    cambios.Add("ImagenUrl");
            }

            return string.Join(", ", cambios);
        }

        public static bool VerificarPermiso(int permiso)
        {
            try
            {
                List<int> permisosUsuario = SesionManager.GetUsuario().ListaPermisos;
                return permisosUsuario.Contains(permiso);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool AsignarPermiso(BEUsuario usuario, BEPermiso permiso)
        {
            try
            {
                return MPPUsuario.AsignarPermiso(usuario, permiso);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool EliminarPermisos(BEUsuario usuario)
        {
            try
            {
                return MPPUsuario.EliminarPermisos(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        public static bool RealizarBackup()
        {
            try
            {
                return MPPUsuario.RealizarBackup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static BEUsuario BuscarUsuarioPorUsuername(BEUsuario usuario)
        {
            try
            {
                return MPPUsuario.BuscarUsuarioPorUsuername(usuario);
            }
            catch (Exception ex) { throw ex; }
        }

        private static bool VerificarIntentosFallidos(int id)
        {
            try
            {
                return MPPUsuario.VerificarIntentosFallidos(id);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
