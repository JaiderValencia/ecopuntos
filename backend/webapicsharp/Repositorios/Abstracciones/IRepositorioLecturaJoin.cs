namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioLecturaJoin
    {
        public Task<IReadOnlyList<Dictionary<string, object?>>> ListarTrabajadoresAsync(int? limite = null);
    }
}
