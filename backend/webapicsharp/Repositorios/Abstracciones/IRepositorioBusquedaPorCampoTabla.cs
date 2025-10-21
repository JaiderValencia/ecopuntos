namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioBusquedaPorCampoTabla
    {
        Task<Dictionary<string, object?>?> BuscarPorCampoAsync(string tabla, string campo, object valor);
    }
}
