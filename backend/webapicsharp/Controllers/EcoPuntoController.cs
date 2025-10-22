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
        public async Task<IActionResult> CrearEcoPunto([FromBody] EcoPunto ecoPunto)
        {
            try
            {
                var resultado = await _servicioEcoPunto.CrearEcoPuntoAsync(
                    ecoPunto.Trabajador!.Id,
                    ecoPunto.Ubicacion!.Latitud,
                    ecoPunto.Ubicacion!.Longitud,
                    ecoPunto.Ubicacion!.Direccion,
                    ecoPunto.Horario,
                    ecoPunto.MaterialesAceptados
                    );
                return Ok(new
                {
                    mensaje = resultado
                });
            } catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

    }
}
