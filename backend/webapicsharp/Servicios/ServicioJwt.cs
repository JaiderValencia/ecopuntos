using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;

namespace webapicsharp.Servicios
{
    public class ServicioJwt : IServicioJwt
    {
        private readonly IConfiguration _config;

        public ServicioJwt(IConfiguration config)
        {
            _config = config;
        }

        public string GenerarToken(string correo)
        {
            try
            {
                //clave secreta del appsettings.json
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var rol = ValidacionRol(correo);

                //Creamos los claims para el token
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
            catch(Exception e)
            {
                return e.Message;
            }
        }

        public string ValidacionRol(string correo)
        {
            try
            {
                var split = correo.Split("@").ToList();
                if (split[1] == "ecomedellin.com")
                {
                    return "Empleado";
                }

                return "Cliente";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string HashearContrasena(string contrasena)
        {
            try
            {
                return BCrypt.Net.BCrypt.HashPassword(contrasena);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool CompararContrasenas(string contrasenaPlana, string hashContrasena)
        {
            try
            {
                bool esValido = BCrypt.Net.BCrypt.Verify(contrasenaPlana, hashContrasena);

                return esValido;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
