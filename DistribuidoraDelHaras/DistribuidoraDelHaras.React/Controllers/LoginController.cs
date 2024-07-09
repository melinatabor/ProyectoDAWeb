using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System.Reflection;

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
                BEUsuario user = BLLUsuario.Login(model);
                return Ok(new
                {
                    nombre = user.Nombre,
                    apellido = user.Apellido,
                    email = user.Email,
                    username =  user.Username,
                    rol = user.Rol,
                });
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
            try
            {
                SesionManager.Logout();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] BEUsuario model)
        {
            try
            {
                BLLUsuario.Agregar(model);
                return Ok(new
                {
                    username = model.Username,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("bitacora")]
        public IActionResult Bitacora()
        {
            try
            {
                DateTime desde = new DateTime(2024, 1, 1, 0, 0, 0);
                DateTime hasta = new DateTime(2024, 7, 7, 23, 59, 59);

                BEBitacoraCriteria criteria = new BEBitacoraCriteria()
                {
                    Desde = desde,
                    Hasta = hasta,
                    Tipo = 1,
                    Usuario = "admin",
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
