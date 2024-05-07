using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDelHaras.React.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<BEUsuario> usuarios = BLLUsuario.Listar();
                return StatusCode(StatusCodes.Status200OK, usuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving data from the database", error = ex.Message });
            }
        }
    }
}
