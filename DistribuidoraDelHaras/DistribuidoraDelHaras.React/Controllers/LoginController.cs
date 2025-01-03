﻿using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Servicios;
using System.Reflection;
using static BE.BEBitacora;

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
                    id = user.Id,
                    nombre = user.Nombre,
                    apellido = user.Apellido,
                    email = user.Email,
                    username =  user.Username
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
                int usuarioId = SesionManager.GetUsuario().Id;

                SesionManager.Logout();

                BEBitacora bitacora = new BEBitacora()
                {
                    Usuario = usuarioId,
                    Tipo = BitacoraTipo.INFO,
                    Mensaje = "El usuario finalizó la sesión"
                };
                BLLBitacora.Agregar(bitacora);

                return Ok(new 
                { 
                    message = "El usuario ha finalizado la sesión exitosamente." 
                });
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
                //BLLUsuario.RecalcularDigitoVerificadorVertical();
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


        [HttpGet("usuarios")]
        public IActionResult Usuarios()
        {
            try
            {
                List<BEUsuario> users = BLLUsuario.Listar();
                return Ok(new { usuarios = users });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
