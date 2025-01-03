﻿using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Servicios.DigitoVerificador;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecalculardvController : Controller
    {
        [HttpPost]
        public IActionResult RecalcularDigitosVerificadores()
        {
            try
            {
                BLLProducto.RecalcularDVH();
                string dvvBitacoraActual = DigitoVerificador.RunVertical(BLLBitacora.ListarTodo());
                BLLDigitoVerificador.ActualizarDigitoVerificadorVertical(dvvBitacoraActual, BEDigitoVerificador.ENTIDAD_BITACORA);

                BLLBitacora.RecalcularDVH();
                string dvvProductoActual = DigitoVerificador.RunVertical(BLLProducto.Listar());
                BLLDigitoVerificador.ActualizarDigitoVerificadorVertical(dvvProductoActual, BEDigitoVerificador.ENTIDAD_PRODUCTO);

                BLLAuditoriaCambios.Eliminar();
                BLLAuditoriaCambios.InsertarProductosIniciales();
                BLLAuditoriaCambios.InsertarBitacoraIniciales();

                return Ok(new { message = "DVH y DVV recalculados y actualizados exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

    }


}
