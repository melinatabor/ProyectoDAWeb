using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : Controller
    {
        [HttpGet("session")]
        public IActionResult GetSession()
        {
            if (SesionManager.GetSession() != null)
            {
                var usuario = SesionManager.GetUsuario();
                return Ok(new { 
                    IsAuthenticated = true,
                    Username = usuario.Username
                });
            }
            else
            {
                return Ok(new { IsAuthenticated = false, Username = false });
            }
        }
    }
}
