using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : Controller
    {
        [HttpGet("verificar-permiso/{permisoId}")]
        public bool VerificarPermiso(int permisoId)
        {
            // Verifica si el usuario logeado tiene el permisoId asignado.
            return BLLUsuario.VerificarPermiso(permisoId);
        }

        [HttpGet("permisos-usuario/{usuarioId}")]
        public IActionResult ObtenerPermisosUsuario(int usuarioId)
        {
            // Devuelve todos los permisos de usuario deseado
            List<BEPermiso> permisos = new List<BEPermiso>();

            permisos = BLLUsuario.ObtenerPermisosUsuario(usuarioId);

            return Ok(new { permisos = permisos });
        }
    }


}
