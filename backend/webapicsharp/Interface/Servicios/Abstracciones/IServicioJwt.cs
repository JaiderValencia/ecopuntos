using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioJwt
    {
        string GenerarToken(string correo);
        string ValidacionCorreoRol(string correo);
        string HashearContrasena(string contrasena);
        bool CompararContrasenas(string hashContrasena, string contrasenaPlana);
    }
}
