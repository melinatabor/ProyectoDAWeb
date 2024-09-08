using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System.Reflection;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        [HttpGet("bitacora")]
        public IActionResult Bitacora()
        {
            try
            {
                DateTime desde = new DateTime(2024, 1, 1, 0, 0, 0);
                DateTime hasta = new DateTime(2024, 12, 31, 23, 59, 59);

                BEBitacoraCriteria criteria = new BEBitacoraCriteria()
                {
                    Desde = desde,
                    Hasta = hasta,
                    Tipo = 1,
                    Usuario = null,
                    Page = 1,
                    RowPerPage = 5
                };

                List<BEBitacoraFiltrada> bitacora = BLLBitacora.Filtrar(criteria);

                return Ok(new
                {
                    filas = bitacora
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
