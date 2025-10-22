using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Servicios.Abstracciones;



namespace webapicsharp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IServicioJwt _jwt;
        private readonly IServicioCliente _servicioCliente;
    
        public JwtController(IServicioJwt jwt, IServicioCliente servicioCliente)
        {
            _jwt = jwt;
            _servicioCliente= servicioCliente;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _servicioCliente.BuscarClientePorCorreoAsync(request.Email);

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

        [HttpPost] 
        public async Task<IActionResult> Registro([FromBody] ClienteDto dto)
        {
            try
            {
                var nuevoRegistro = new Cliente(
                    0,
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contrasena!,
                    0
                );

                var clienteCreado = await _servicioCliente.CrearClienteAsync(nuevoRegistro);

                if (clienteCreado is null)
                    return BadRequest(new { mensaje = "Ocurrio un error durante el registro" });

                var token = _jwt.GenerarToken(dto.Correo!);

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    Bearer = token,
                    datosUsuario = new
                    {
                        Id = clienteCreado!.Id,
                        Nombre = clienteCreado.ObtenerNombre(),
                        Correo = clienteCreado.ObtenerCorreo(),
                        Telefono = clienteCreado.ObtenerTelefono(),
                        Rol = "Usuario"
                    }
                });


            } catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" , detalle = e.Message});
            }
        }

        //public bool EsEmpleado(string correo)
        //{
        //    List<string> split = correo.Split("@").ToList();

        //    if (split[1] == "ecomedellin.com")
        //        return true;
        //    return false;
        //}
    }
}
