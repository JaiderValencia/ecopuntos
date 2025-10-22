namespace webapicsharp.Repositorios.Abstracciones
{
    public interface IRepositorioJoinTresTablas
    {
        Task<List<Dictionary<string, object?>>> JoinTresTablasAsync(
            string tabla1,
            string tabla2,
            string tabla3,
            string columnaJoin1_2_T1,
            string columnaJoin1_2_T2,
            string columnaJoin2_3_T2,
            string columnaJoin2_3_T3,
            string columnasSeleccionadas = "*",
            string tipoJoin = "INNER",
            int? limite = null);
    }
}