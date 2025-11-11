using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;

namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EntregaController : ControllerBase
    {
        private readonly IServicioEntrega _servicioEntrega;

        public EntregaController(IServicioEntrega servicioEntrega)
        {
            _servicioEntrega = servicioEntrega;
        }

        [HttpPost]
        public async Task<IActionResult> CrearEntrega([FromBody] CrearEntregaDto entrega)
        {
            try
            {
                var resultado = await _servicioEntrega.CrearEntregAsync(entrega);

                return Ok(new
                {
                    Mensaje = "Entrega registrada correctamente",
                    Entrega = resultado
                });
            } catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
