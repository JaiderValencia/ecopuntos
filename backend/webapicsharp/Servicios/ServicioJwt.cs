using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioJwt : IServicioJwt
    {
        private readonly IConfiguration _config;
        private readonly IRepositorioBusquedaPorCampoTabla _repoBusqueda;

        public ServicioJwt(IConfiguration config,
            IRepositorioBusquedaPorCampoTabla repoBusqueda)
        {
            _config = config;
            _repoBusqueda = repoBusqueda;
        }

        public string GenerarToken(string correo)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var rol = ValidacionCorreoRol(correo);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, correo),
                new Claim(ClaimTypes.Role, rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ValidacionCorreoRol(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo) || !correo.Contains("@"))
            {
                throw new ArgumentException("Correo inválido", nameof(correo));
            }

            var dominio = correo.Split("@")[1].ToUpperInvariant();

            return dominio switch
            {
                "ECOMEDELLIN.COM" => "Empleado",
                "ADMIN.COM" => "Admin",
                _ => "Cliente"
            };
        }

        public string HashearContrasena(string contrasena)
        {
            return BCrypt.Net.BCrypt.HashPassword(contrasena);
        }

        public bool CompararContrasenas(string hashContrasena, string contrasenaPlana)
        {
            return BCrypt.Net.BCrypt.Verify(contrasenaPlana, hashContrasena);
        }
    }
}