using BLL;
using Microsoft.AspNetCore.Mvc;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        // Prueba para consumir la API de productos de fakestoreapi.com
        [HttpGet("test")]
        public async Task<IActionResult> GetTest()
        {
            var result = await BLLProducto.GetApiProductTest();

            return Content(result, "application/json");
        }
    }
}
