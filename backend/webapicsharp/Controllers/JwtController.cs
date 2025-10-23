using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;



namespace webapicsharp.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IServicioJwt _jwt;
        private readonly IRepositorioBusquedaPorCampoTabla _repoBuscar;
        private readonly IServicioCliente _servicioCliente;

        public JwtController(IServicioJwt jwt, IRepositorioBusquedaPorCampoTabla repoBuscar, IServicioCliente servicioCliente)
        {
            _jwt = jwt;
            _repoBuscar = repoBuscar;
            _servicioCliente = servicioCliente;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var rol = _jwt.ValidacionCorreoRol(request.Email!);
                if (string.IsNullOrEmpty(rol))
                    return BadRequest(new {mensaje = "Debe proporcionar un correo valido" });

                var usuario = await _repoBuscar.BuscarPorCampoAsync("Usuario", "Correo", request.Email);

                if (usuario == null )
                {
                    return BadRequest(new { mensaje = "No existe esa cuenta" });
                }

                var resultado = _jwt.CompararContrasenas(usuario[0]["Contrasena"]!.ToString()!, request.Password);

                if (!resultado)
                {
                    return Unauthorized(new { mensaje = "Correo o contraseña erroneos" });
                }


                var token = _jwt.GenerarToken(request.Email!);

                return Ok(new
                {
                    mensaje = "Inicio de sesión exitoso.",
                    Bearer = token,
                    datosUsuario = new
                    {
                        Id = usuario[0]["Id"],
                        Nombre = usuario[0]["Nombre"],
                        Correo = request.Email!,
                        Rol = rol,
                        Telefono = usuario[0]["Telefono"]
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
                var hash = _jwt.HashearContrasena(dto.Contrasena!);

                var nuevoRegistro = new Cliente(
                    0,
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    hash!,
                    0
                );


                var clienteCreado = await _servicioCliente.CrearClienteAsync(nuevoRegistro);

                if (clienteCreado is null)
                    return BadRequest(new { mensaje = "Ocurrio un error durante el registro" });

                var token = _jwt.GenerarToken(dto.Correo!);

                return Ok(new
                {
                    mensaje = "Registro exitoso.",
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
    }
}
