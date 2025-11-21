namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioConsultaPersonalizada
    {
        Task<List<Dictionary<string, object?>>> EjecutarConsultaAsync(string consulta, Dictionary<string, object?>? parametros = null);
    }
}
