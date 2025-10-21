using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Modelos;
using webapicsharp.Servicios;
using webapicsharp.Servicios.Abstracciones;


namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IServicioUsuario _servicioUsuario;

        public UsuariosController(IServicioUsuario servicioUsuario)
        {
            _servicioUsuario = servicioUsuario;
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Cedula) ||
                string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Contraseña))
                {
                    return BadRequest(new { mensaje = "Datos inválidos o incompletos." }); 
                }

                var nuevoUsuario = new Usuario(
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contraseña!
                );

                var resultado = await _servicioUsuario.CrearUsuarioAsync(nuevoUsuario);

                if (resultado != "El usuario fue creado correctamente")
                {
                    return BadRequest(new { mensaje = "El usuario no se pudo crear" });
                }

                return Ok(new
                {
                    mensaje = resultado,
                    Usuario = nuevoUsuario
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarUsuarioPorCorreo([FromQuery] string correo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                var usuarioFiltrado = await _servicioUsuario.BuscarUsuarioPorCorreoAsync(correo);

                if (usuarioFiltrado == null)
                {
                    return BadRequest(new {mensaje = $"No se encontro un usuario con el correo {correo}" });
                }

                return Ok(new
                {
                    mensaje = "Encontrado exitosamente",
                    Usuario = usuarioFiltrado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarUsuarioPorCorreo([FromBody] UsuarioDto dto, [FromQuery] string correo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                var nuevosDatos = new Usuario(
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contraseña!
                );

                string respuesta = await _servicioUsuario.ActualizarUsuarioPorCorreoAsync(correo, nuevosDatos);

                if (respuesta != "Usuario actualizado correctamente")
                {
                    return BadRequest(new { mensaje = respuesta });
                }
                
                return Ok(new
                {
                    mensaje = respuesta,
                    usuario = nuevosDatos
                });
            }
            catch(Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarUsuarioPorCorreo([FromQuery] string correo)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                string respuesta = await _servicioUsuario.EliminarUsuarioPorCorreoAsync(correo);

                if (respuesta != "Usuario eliminado correctamente")
                {
                    return BadRequest(new { mensaje = respuesta });
                }

                return Ok(new { mensaje = respuesta});
            } catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
