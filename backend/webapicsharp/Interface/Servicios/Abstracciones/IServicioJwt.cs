namespace webapicsharp.Interface.Servicios.Abstracciones
{
    public interface IServicioJwt
    {
        public string GenerarToken(string correo);
    }
}
