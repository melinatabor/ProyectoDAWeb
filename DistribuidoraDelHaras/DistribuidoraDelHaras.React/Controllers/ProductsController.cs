using BE;
using BLL;
using Microsoft.AspNetCore.Mvc;
using Servicios;

namespace DistribuidoraDelHaras.React.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        [HttpPost("products")]
        public IActionResult CreateProduct([FromBody] BEProducto newProduct)
        {
            try
            {
                if (BLLProducto.ValidarProducto(newProduct))
                    return BadRequest(new { message = "Datos del producto incompletos o inválidos" });

                bool creado = BLLProducto.Agregar(newProduct);

                if (!creado)
                    return BadRequest(new { message = "No se pudo crear el producto" });

                var productResponse = new
                {
                    newProduct.Nombre,
                    newProduct.Precio,
                    newProduct.Descripcion,
                    newProduct.ImagenUrl
                };

                return CreatedAtAction(nameof(GetProducts), null, productResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("products/{id?}")]
        public IActionResult GetProducts(int? id = null)
        {
            try
            {
                if (id.HasValue)
                {
                    BEProducto product = BLLProducto.ObtenerPorId(id.Value);
                    if (product == null)
                        return NotFound(new { message = "Producto no encontrado" });

                    return Ok(new { producto = product });
                }

                List<BEProducto> products = BLLProducto.Listar();
                return Ok(new { productos = products });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("products/{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] BEProducto product)
        {
            try
            {
                BEProducto p = BLLProducto.ObtenerPorId(id);

                if (p == null || p.Id != id)
                    return NotFound(new { message = "Producto no encontrado" });

                if (!string.IsNullOrEmpty(product.Nombre))
                    p.Nombre = product.Nombre;

                if (product.Precio > 0)
                    p.Precio = product.Precio;

                if (!string.IsNullOrEmpty(product.Descripcion))
                    p.Descripcion = product.Descripcion;

                if (!string.IsNullOrEmpty(product.ImagenUrl))
                    p.ImagenUrl = product.ImagenUrl;

                bool editado = BLLProducto.Editar(p);

                if (!editado)
                    return BadRequest(new { message = "No se pudo actualizar el producto" });

                return Ok(new { message = "Producto actualizado correctamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("products/filter")]
        public IActionResult FilterProductsByName([FromQuery] string nombre)
        {
            try
            {
                List<BEProducto> products = BLLProducto.FiltrarPorNombre(nombre);

                if (products == null || products.Count == 0)
                    return NotFound(new { message = "No se encontraron productos con el nombre proporcionado" });

                return Ok(new { productos = products });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

    }
}
