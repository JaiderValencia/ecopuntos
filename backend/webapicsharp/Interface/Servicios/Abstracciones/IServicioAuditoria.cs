namespace webapicsharp.Servicios.Abstracciones
{
    public interface IServicioAuditoria
    {
        Task RegistrarAccionAsync(string Accion, string? Usuario = null);
    }
}
