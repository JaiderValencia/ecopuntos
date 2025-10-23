using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;

namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EcoPuntoController : ControllerBase
    {
        private readonly IServicioEcoPunto _servicioEcoPunto;

        public EcoPuntoController(IServicioEcoPunto servicioEcoPunto)
        {
            _servicioEcoPunto = servicioEcoPunto;
        }

        [HttpPost]
        public async Task<IActionResult> BuscarEcoPuntoPorID([FromQuery] int id)
        {
            try
            {
                var resultado = await _servicioEcoPunto.BuscarEcoPuntoPorIDAsync(id);
                return Ok(new
                {
                    mensaje = "EcoPunto registrado correctamente",
                    EcoPunto = resultado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearEcoPunto([FromBody] EcoPuntoDto ecoPunto)
        {
            try
            {
                var resultado = await _servicioEcoPunto.CrearEcoPuntoAsync(
                    ecoPunto.Trabajador!.CodigoDeEmpleado!,
                    ecoPunto.Ubicacion!.Latitud!,
                    ecoPunto.Ubicacion!.Longitud!,
                    ecoPunto.Ubicacion!.Direccion!,
                    ecoPunto.Horario!,
                    ecoPunto.MaterialesAceptados!
                    );
                return Ok(new
                {
                    mensaje = "EcoPunto registrado correctamente",
                    EcoPunto = resultado
                });
            } catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEcoPuntoPorID([FromBody] ActualizarEcoPuntoDto dto, [FromQuery]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var ecoPuntoActualizado = await _servicioEcoPunto.ActualizarEcoPuntoPorIDAsync(
                    idEcoPunto: id,
                    codigoDeEmpleado: dto.CodigoDeEmpleado!,
                    latitud: dto.Latitud!,
                    longitud: dto.Longitud!,
                    direccion: dto.Direccion!,
                    horario: dto.Horario!,
                    materiales: dto.Materiales
                );

                return Ok(new
                {
                    Mensaje = "EcoPunto creado correctamente",
                    EcoPunto = ecoPuntoActualizado
                });

            }
            catch (Exception ex)
            {
                // Aquí puedes personalizar el mensaje de error
                return StatusCode(500, new { mensaje = $"Error al actualizar EcoPunto: {ex.Message}" });
            }
        }

    }
}
