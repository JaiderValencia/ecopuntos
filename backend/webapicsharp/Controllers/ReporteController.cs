using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;

namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ReporteController : ControllerBase
    {
        private readonly IServicioReporte _servicioReporte;

        public ReporteController(IServicioReporte servicioReporte)
        {
            _servicioReporte = servicioReporte;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReportesPorIdCliente([FromQuery] int idCliente)
        {
            try
            {
                var resultado = await _servicioReporte.ObtenerReportesPorIdClienteAsync(idCliente);

                return Ok(new
                {
                    Mensaje = "Reportes obtenidos correctamente",
                    Reportes = resultado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReportesPorIdTrabajador([FromQuery] int idTrabajador)
        {
            try
            {
                var resultado = await _servicioReporte.ObtenerReportesPorIdTrabajadorAsync(idTrabajador);

                return Ok(new
                {
                    Mensaje = "Reportes obtenidos correctamente",
                    Reportes = resultado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerInformacionReportePorId([FromQuery] int idReporte)
        {
            try
            {
                var resultado = await _servicioReporte.ObtenerInformacionReportePorIDAsync(idReporte);

                return Ok(new
                {
                    Mensaje = "Reporte obtenido correctamente",
                    Reporte = resultado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
