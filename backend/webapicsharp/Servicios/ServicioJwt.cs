using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
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
            _repoBusqueda =repoBusqueda;
        }

        public string GenerarToken(string correo)
        {
            try
            {
                //clave secreta del appsettings.json
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var rol = ValidacionCorreoRol(correo);

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

        public string ValidacionCorreoRol(string correo)
        {
            try
            {
                if (!correo.Contains("@"))
                {
                    return null!;
                }

                var split = correo.Split("@").ToList();
                if (split[1].ToUpper() == "ECOMEDELLIN.COM")
                {
                    return "Empleado";
                }

                if (split[1].ToUpper() == "ADMIN.COM".ToUpper())
                {
                    return "Admin";
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

        public bool CompararContrasenas(string hashContrasena, string contrasenaPlana)
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
