namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioJwt
    {
        public string GenerarToken(string correo);
        public string ValidacionRol(string correo);
        public bool CompararContrasenas(string contrasenaPlana, string hashContrasena);
        public string HashearContrasena(string contrasena);
    }
}
