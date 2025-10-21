using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using webapicsharp.Interface.Servicios.Abstracciones;

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

                //Creamos los claims para el token
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, correo),
                    //new Claim(ClaimTypes.Role, rol),
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

    }
}
