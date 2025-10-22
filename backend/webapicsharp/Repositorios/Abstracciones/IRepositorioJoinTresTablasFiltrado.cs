namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioJoinTresTablasFiltrado
    {
        public Task<List<Dictionary<string, object?>>> JoinTresTablasAsync(
            string tabla1,
            string tabla2,
            string tabla3,
            string campoRelacion12Tabla1,
            string campoRelacion12Tabla2,
            string campoRelacion23Tabla2,
            string campoRelacion23Tabla3,
            string columnasSeleccionadas = "*",
            string tipoJoin = "INNER",
            int? limite = null,
            string? campoFiltro = null,
            object? valorFiltro = null);
        
    }
}
