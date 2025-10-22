using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Servicios;

namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AdministradorController : ControllerBase
    {
        private readonly IServicioAdministrador _servicioAdministrador;

        public AdministradorController(IServicioAdministrador servicioAdministrador)
        {
            _servicioAdministrador = servicioAdministrador;
        }

        [HttpPost]
        public async Task<IActionResult> CrearAdministrador([FromBody] AdministradorDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Cedula) ||
                string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Contrasena) ||
                !int.TryParse(dto.NivelDeAcceso.ToString(), out _) ||
                string.IsNullOrWhiteSpace(dto.CodigoDeEmpleado))
                {
                    return BadRequest(new { mensaje = "Datos inválidos o incompletos." });
                }

                var datosAdministrador = new Administrador(
                    0,
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contrasena!,
                    "",
                    dto.NivelDeAcceso!
                );

                var resultado = await _servicioAdministrador.CrearAdministradorAsync(datosAdministrador);

                if (resultado is null)
                {
                    return BadRequest(new { mensaje = "El cliente no se pudo crear" });
                }

                var nuevoAdministrador = await _servicioAdministrador.BuscarAdministradorPorCorreoAsync(dto.Correo);

                return Ok(new
                {
                    mensaje = "Administrador creado exitosamente",
                    Administrador = resultado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarAdministradorPorCorreo([FromQuery] string correo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                var administradorFiltrado = await _servicioAdministrador.BuscarAdministradorPorCorreoAsync(correo);

                if (administradorFiltrado == null)
                {
                    return BadRequest(new { mensaje = $"No se encontro un Administrador con el correo {correo}" });
                }

                return Ok(new
                {
                    mensaje = "Encontrado exitosamente",
                    Administrador = administradorFiltrado
                });

            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
