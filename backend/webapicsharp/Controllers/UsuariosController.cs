using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webapicsharp.Servicios;
using webapicsharp.Servicios.Abstracciones;             


namespace webapicsharp.Controllers
{
    [Route("api/[controller]")]
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
            if (string.IsNullOrWhiteSpace(dto.Nombre) ||
                string.IsNullOrWhiteSpace(dto.Cedula) ||
                string.IsNullOrWhiteSpace(dto.Correo) ||
                string.IsNullOrWhiteSpace(dto.Direccion) ||
                string.IsNullOrWhiteSpace(dto.Telefono) ||
                string.IsNullOrWhiteSpace(dto.Contraseña))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            var filas = await _servicioUsuario.CrearUsuarioAsync(dto.Nombre, dto.Cedula, dto.Correo, dto.Direccion, dto.Telefono, dto.Contraseña);

            return filas > 0
                ? Ok("Usuario creado correctamente.")
                : StatusCode(500, "No se pudo insertar el usuario.");
        }
    }

    public class UsuarioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;
    }
}
