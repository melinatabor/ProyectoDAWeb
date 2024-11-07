using BE;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servicios.DigitoVerificador
{
    public class DigitoVerificador
    {
        private static string _digitoVerificador;
        private static StringBuilder _digitoVerificadorVertical;

        public static string Run(BEBitacora bitacora)
        {
            try
            {
                _digitoVerificador = bitacora.Mensaje + bitacora.Fecha.ToString("o");

                return Encriptador.Run(_digitoVerificador);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al generar el dígito verificador de bitácora: {ex.Message}");
            }
        }

        public static string Run(BEProducto producto)
        {
            try
            {
                _digitoVerificador = producto.Nombre + producto.Precio + producto.Descripcion + producto.ImagenUrl;

                return Encriptador.Run(_digitoVerificador);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al generar el dígito verificador de producto: {ex.Message}");
            }
        }

        public static string RunVertical(List<BEBitacora> bitacoras)
        {
            try
            {
                _digitoVerificadorVertical = new StringBuilder();

                foreach (var bitacora in bitacoras)
                    _digitoVerificadorVertical.Append(Run(bitacora));

                return Encriptador.Run(_digitoVerificadorVertical.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al generar el dígito verificador vertical para bitácoras: {ex.Message}");
            }
        }

        public static string RunVertical(List<BEProducto> productos)
        {
            try
            {
                _digitoVerificadorVertical = new StringBuilder();

                foreach (var producto in productos)
                    _digitoVerificadorVertical.Append(Run(producto));

                return Encriptador.Run(_digitoVerificadorVertical.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al generar el dígito verificador vertical para productos: {ex.Message}");
            }
        }
    }
}
