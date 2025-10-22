using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Servicios;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TrabajadorController : ControllerBase
    {
        private readonly IServicioTrabajador _servicioTrabajador;

        public TrabajadorController(IServicioTrabajador servicioTrabajador)
        {
            _servicioTrabajador = servicioTrabajador;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTrabajador([FromBody] TrabajadorDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Cedula) ||
                string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Contrasena)||
                string.IsNullOrWhiteSpace(dto.Horario)||
                string.IsNullOrWhiteSpace(dto.CodigoDeEmpleado))
                {
                    return BadRequest(new { mensaje = "Datos inválidos o incompletos." });
                }

                var datosTrabajador = new Trabajador(
                    0,
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contrasena!,
                    "",
                    dto.Horario!
                );

                var resultado = await _servicioTrabajador.CrearTrabajadorAsync(datosTrabajador);

                if (resultado != "El Trabajador fue creado exitosamente")
                {
                    return BadRequest(new { mensaje = "El cliente no se pudo crear" });
                }

                var nuevoTrabajador = await _servicioTrabajador.BuscarTrabajadorPorCorreoAsync(dto.Correo);

                return Ok(new
                {
                    mensaje = resultado,
                    Trabajador = nuevoTrabajador
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTrabajadorPorCorreo([FromQuery] string correo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                var trabajadorFiltrado = await _servicioTrabajador.BuscarTrabajadorPorCorreoAsync(correo);

                if (trabajadorFiltrado == null)
                {
                    return BadRequest(new { mensaje = $"No se encontro un Trabajador con el correo {correo}" });
                }

                return Ok(new
                {
                    mensaje = "Encontrado exitosamente",
                    Cliente = trabajadorFiltrado
                });

            } catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTrabajadores([FromQuery] int limit)
        {
            try
            {
                if (!int.TryParse(limit.ToString(), out _) || limit < 1)
                {
                    return BadRequest("Debe proporcionar un limite positivo.");
                }

                var lista = await _servicioTrabajador.ObtenerTrabajadoresAsync(limit);

                return Ok(new
                {
                    limit = limit,
                    total = lista.Count(),
                    mensaje = "Lista de trabajadores",
                    Cliente = lista
                });
            }
            catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
