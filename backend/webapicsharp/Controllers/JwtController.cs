using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;



namespace webapicsharp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IServicioJwt _jwt;
        private readonly IServicioUsuario _servicioUsuario;
    
        public JwtController(IServicioJwt jwt, IServicioUsuario servicioUsuario)
        {
            _jwt = jwt;
            _servicioUsuario = servicioUsuario;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _servicioUsuario.BuscarUsuarioPorCorreoAsync(request.Email);

                if (usuario == null )
                {
                    return BadRequest(new { mensaje = "No existe esa cuenta" });
                }

                var correo = usuario.ObtenerCorreo();
                var contrasena = usuario.ObtenerContrasena();

                var resultado = usuario.LogIn(correo!, contrasena!);

                if (!resultado)
                {
                    return Unauthorized(new { mensaje = "Correo o contraseña erroneos" });
                }

                var token = _jwt.GenerarToken(correo!);

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    Bearer = token,
                    datosUsuario = new
                    {
                        Id = usuario.Id,
                        Nombre = usuario.ObtenerNombre(),
                        Correo = correo,
                        Telefono = usuario.ObtenerTelefono()
                    }
                });

            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
