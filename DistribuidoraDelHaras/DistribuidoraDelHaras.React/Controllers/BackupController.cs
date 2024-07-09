using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System.Reflection;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        [HttpGet("backup")]
        public async Task<IActionResult> DownloadBackup()
        {
            try
            {
                string backupFilePath = @"C:\DistHaras.bak";

                if (System.IO.File.Exists(backupFilePath))
                {
                    byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(backupFilePath);

                    return File(fileBytes, "application/octet-stream", "base-de-datos.bak");
                }
                else
                {
                    return NotFound("No se encontró el archivo de respaldo.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al descargar el archivo de respaldo: {ex.Message}");
            }
        }
    }
}
