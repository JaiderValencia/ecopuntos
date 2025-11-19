namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioJoin
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
        public Task<List<Dictionary<string, object?>>> JoinTresTablasFiltradoAsync(
            string tabla1,
            string tabla2,
            string tabla3,
            string campoRelacion12Tabla1,
            string campoRelacion12Tabla2,
            string campoRelacion23Tabla1,
            string campoRelacion23Tabla3,
            string columnasSeleccionadas = "*",
            int? limite = 15,
            string? campoFiltro = null,
            object? valorFiltro = null);
        public Task<List<Dictionary<string, object?>>> JoinCuatroTablasAsync(
            string tabla1,
            string tabla2,
            string tabla3,
            string tabla4,
            string campoRelacion12Tabla1,
            string campoRelacion12Tabla2,
            string campoRelacion23Tabla2,
            string campoRelacion23Tabla3,
            string campoRelacion34Tabla3,
            string campoRelacion34Tabla4,
            string columnasSeleccionadas = "*",
            string tipoJoin = "INNER",
            int? limite = 15,
            string? campoFiltro = null,
            object? valorFiltro = null);
        public Task<List<Dictionary<string, object?>>> JoinCuatroTablasConFiltrosAsync(
            string tabla1,
            string tabla2,
            string tabla3,
            string tabla4,
            string campoRelacion12Tabla1,
            string campoRelacion12Tabla2,
            string campoRelacion23Tabla2,
            string campoRelacion23Tabla3,
            string campoRelacion34Tabla3,
            string campoRelacion34Tabla4,
            Dictionary<string, object?> filtros,
            string columnasSeleccionadas = "*",
            string tipoJoin = "INNER",
            int? limite = 15);
    }
}
