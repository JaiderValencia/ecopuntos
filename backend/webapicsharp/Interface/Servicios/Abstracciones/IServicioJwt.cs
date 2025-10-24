using webapicsharp.Modelos;

namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioJwt
    {
        public string GenerarToken(string correo);
        public string ValidacionCorreoRol(string correo);
        public bool CompararContrasenas(string contrasenaPlana, string hashContrasena);
        public string HashearContrasena(string contrasena);

        //public Task<Usuario> ObtenerUsuarioPorCorreo(string correo);
    }
}
