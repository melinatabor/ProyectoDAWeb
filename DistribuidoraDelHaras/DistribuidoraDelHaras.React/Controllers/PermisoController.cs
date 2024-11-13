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

        [HttpGet("permisos")]
        public IActionResult ObtenerTodosPermisos(int usuarioId)
        {
            List<BEPermisoXUsuario> permissions = new List<BEPermisoXUsuario>();

            permissions = BLLPermiso.ListarTodosPermisos();

            return Ok(new { permisos = permissions });
        }

        [HttpPost("asignar-permiso/{usuarioId}/{permisoId}")]
        public IActionResult AsignarPermiso(int usuarioId, int permisoId)
        {
            BEUsuario usuario = new BEUsuario() { Id = usuarioId };
            BEPermiso permiso = BLLPermiso.BuscarPermiso(permisoId);

            if (permiso is not BEPermiso)
                return StatusCode(StatusCodes.Status404NotFound, new { message = $"Permiso con ID: {permisoId} no encontrado." });

            bool asignado = BLLUsuario.AsignarPermiso(usuario, permiso);
            
            if (asignado)
                return Ok(new { message = $"Permiso {permiso.Nombre} asignado correctamente." });

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error al asignar permiso {permiso.Nombre}." });
        }

        [HttpPost("eliminar-permisos/{usuarioId}")]
        public IActionResult EliminarPermisos(int usuarioId)
        {
            BEUsuario usuario = new BEUsuario() { Id = usuarioId };

            bool eliminado = BLLUsuario.EliminarPermisos(usuario);

            if (eliminado)
                return Ok(new { message = $"Permisos del usuario con ID: {usuarioId} eliminados correctamente." });

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = $"Error al eliminar los permisos del usuario con ID: {usuarioId}." });
        }
    }
}
