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
        [HttpPost("bitacora")]
        public IActionResult Bitacora([FromBody] BEBitacoraCriteria criteria)
        {
            try
            {
                criteria.Usuario = null;
                criteria.Page = 1;
                criteria.RowPerPage = 5;

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
