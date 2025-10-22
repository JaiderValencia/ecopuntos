using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly IServicioMaterial _servicioMaterial;

        public MaterialController(IServicioMaterial servicioMaterial)
        {
            _servicioMaterial = servicioMaterial;
        }

        [HttpPost]
        public async Task<IActionResult> CrearMaterial([FromBody] Material material)
        {
            try
            {
                var materialCreado = await _servicioMaterial.CrearMaterialAsync(material);

                if (materialCreado is null)
                    return BadRequest("Error interno del servidor" );

                return Ok(
                new
                {
                    Mensaje = "Materia registrado con exito",
                    Material = materialCreado
                });

            } catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMateriales([FromQuery] int limite)
        {
            try
            {
                if (!int.TryParse(limite.ToString(), out _) || limite < 1)
                {
                    throw new Exception("Debe proporcionar un limite positivo.");
                }

                var lista = await _servicioMaterial.ObtenerMaterialesAsync(limite);

                return Ok(
                    new
                    {
                        Mensaje = "Lista de materiales actuales",
                        Limite = limite,
                        Total = lista.Count(),
                        Materiales = lista
                    });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMaterialPorNombre([FromQuery] string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return BadRequest("El nombre del material es obligatorio.");

                var material = await _servicioMaterial.ObtenerMaterioalPorNombreAsync(nombre);

                if (material == null)
                    return BadRequest("Algo salio mal buscando el material");

                return Ok(
                    new
                    {
                        Mensaje = "Material obtenido exitosamente",
                        Material = material
                    });
            } catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
