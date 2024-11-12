using BE;
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
                BEUsuario usuarioExistente = BuscarUsuario(usuario)
                    ?? throw new Exception("Credenciales incorrectas. Por favor vuelva a ingresar los datos correctamente.");

                string mensajeModificaciones = VerificarModificacionesYEliminacionesExternas();
                if (!string.IsNullOrEmpty(mensajeModificaciones))
                    throw new Exception($"Se detectaron modificaciones en la base de datos. {mensajeModificaciones}. Por favor contacte al administrador.");

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

        public static string VerificarModificacionesYEliminacionesExternas()
        {
            var mensajesDeCambio = new List<string>();

            string dvvBitacoraActual = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
            string dvvProductoActual = DigitoVerificador.RunVertical(BLLProducto.Listar());

            string dvvBitacoraGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_BITACORA);
            string dvvProductoGuardado = BLLDigitoVerificador.ObtenerDigitoVerificadorVertical(BEDigitoVerificador.ENTIDAD_PRODUCTO);

            bool bitacoraModificada = dvvBitacoraActual != dvvBitacoraGuardado;
            bool productoModificado = dvvProductoActual != dvvProductoGuardado;

            if (bitacoraModificada)
            {
                List<BEAuditoriaCambios> registrosAuditoria = BLLAuditoriaCambios.ListarPorEntidad(BEAuditoriaCambios.ENTIDAD_BITACORA);
                var registrosActuales = BLLBitacora.ListarTodo();

                foreach (BEBitacora registro in registrosActuales)
                {
                    var auditoria = registrosAuditoria.FirstOrDefault(a => a.IdRegistroAfectado == registro.Id);
                    if (auditoria != null && !CompararDatosCSV(auditoria.DatosAntes, auditoria.DatosDespues, registro, auditoria.Operacion))
                    {
                        mensajesDeCambio.Add($"Registro modificado en Bitacora (ID: {registro.Id}): {ObtenerCamposModificadosCSV(auditoria.DatosAntes, registro)}");
                    }
                }

                foreach (var auditoria in registrosAuditoria)
                {
                    if (!registrosActuales.Any(r => r.Id == auditoria.IdRegistroAfectado))
                        mensajesDeCambio.Add($"Registro eliminado externamente en Bitacora (ID: {auditoria.IdRegistroAfectado})");
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
                        mensajesDeCambio.Add($"Registro modificado en Producto (ID: {registro.Id}): {ObtenerCamposModificadosCSV(auditoria.DatosAntes, registro)}");
                    }
                }

                foreach (var auditoria in registrosAuditoria)
                {
                    if (!registrosActuales.Any(r => r.Id == auditoria.IdRegistroAfectado))
                        mensajesDeCambio.Add($"Registro eliminado externamente en Producto (ID: {auditoria.IdRegistroAfectado})");
                }
            }

            return mensajesDeCambio.Count > 0 ? $"Se detectaron modificaciones en la base de datos. {string.Join(" ", mensajesDeCambio)}. Por favor contacte al administrador." : string.Empty;
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
    }
}
