using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] BEUsuario model)
        {
            try
            {
                BLLUsuario.Login(model);
                return Ok(new { Message = $"Login successful. Username: {model.Username}, Password: {model.Password}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Implementa la lógica para cerrar sesión si es necesario
            return Ok();
        }
    }
}
