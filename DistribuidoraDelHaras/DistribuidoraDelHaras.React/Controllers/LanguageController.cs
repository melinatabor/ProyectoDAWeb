using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : Controller
    {
        [HttpGet("translations/{language?}")]
        public IActionResult GetTranslations(int? language = 1)
        {
            List<BETraduccion> traducciones = BLLTraduccion.Listar(language ?? 1);
            return Ok(new { traduccion = traducciones });
        }

        [HttpGet("list")]
        public IActionResult GetLanguages()
        {
            List<BEIdioma> idiomas = BLLIdioma.Listar();
            return Ok(new { idioma = idiomas });
        }
    }
}
