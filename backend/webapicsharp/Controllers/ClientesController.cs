using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapicsharp.Modelos;
using webapicsharp.Servicios;
using webapicsharp.Servicios.Abstracciones;


namespace webapicsharp.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IServicioCliente _servicioCliente;

        public ClientesController(IServicioCliente servicioCliente)
        {
            _servicioCliente = servicioCliente;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] ClienteDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Cedula) ||
                string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Contrasena))
                {
                    return BadRequest(new { mensaje = "Datos inválidos o incompletos." }); 
                }

                var nuevoCliente = new Cliente(
                    0,
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contrasena!,
                    0
                );

                var clienteCreado = await _servicioCliente.CrearClienteAsync(nuevoCliente);

                if (clienteCreado is null)
                {
                    return BadRequest(new { mensaje = "El cliente no se pudo crear" });
                }

                return Ok(new
                {
                    mensaje = "El Cliente fue creado con exito",
                    Cliente = clienteCreado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> BuscarClientePorCorreo([FromQuery] string correo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                var clienteFiltrado = await _servicioCliente.BuscarClientePorCorreoAsync(correo);

                if (clienteFiltrado == null)
                {
                    return BadRequest(new {mensaje = $"No se encontro un cliente con el correo {correo}" });
                }

                return Ok(new
                {
                    mensaje = "Encontrado exitosamente",
                    Cliente = clienteFiltrado
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarClientePorCorreo([FromBody] ClienteDto dto, [FromQuery] string correo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(correo))
                {
                    return BadRequest("Debe proporcionar un correo.");
                }

                var nuevosDatos = new Cliente(
                    0,
                    dto.Nombre!,
                    dto.Cedula!,
                    dto.Correo!,
                    dto.Direccion!,
                    dto.Telefono!,
                    dto.Contrasena!,
                    dto.EcoPuntos!
                );

                var clienteActualizado = await _servicioCliente.ActualizarClientePorCorreoAsync(correo, nuevosDatos);

                if (clienteActualizado is null)
                {
                    return BadRequest("Ocurrio un error actualizando el cliente");
                }

                return Ok(new
                {
                    mensaje = "cliente actualizado con exito",
                    Cliente = clienteActualizado
                });
            }
            catch (Exception e)
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

                string respuesta = await _servicioCliente.EliminarClientePorCorreoAsync(correo);

                if (respuesta != "Cliente eliminado correctamente")
                {
                    return BadRequest(new { mensaje = respuesta });
                }

                return Ok(new { mensaje = respuesta });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = e.Message });
            }
        }
    }
}
