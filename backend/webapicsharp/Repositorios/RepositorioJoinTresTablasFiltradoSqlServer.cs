using Microsoft.Data.SqlClient;
using System.Data;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioJoinTresTablasFiltradoSqlServer : IRepositorioJoinTresTablasFiltrado
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioJoinTresTablasFiltradoSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<List<Dictionary<string, object?>>> JoinTresTablasAsync(
            string tabla1,
            string tabla2,
            string tabla3,
            string campoRelacion12Tabla1,
            string campoRelacion12Tabla2,
            string campoRelacion23Tabla2,
            string campoRelacion23Tabla3,
            string columnasSeleccionadas = "*",
            string tipoJoin = "INNER",
            int? limite = 15,
            string? campoFiltro = null,
            object? valorFiltro = null)
        {
            try
            {
                var resultados = new List<Dictionary<string, object?>>();
                int limiteFinal = limite ?? 15;


                if (string.IsNullOrWhiteSpace(tabla1) ||
                    string.IsNullOrWhiteSpace(tabla2) ||
                    string.IsNullOrWhiteSpace(tabla3))
                    throw new ArgumentException("Los nombres de las tablas no pueden estar vacíos.");

                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                var topClause = limite.HasValue ? $"TOP {limite.Value}" : string.Empty;

                string whereClause = string.Empty;

                if (!string.IsNullOrWhiteSpace(campoFiltro) && valorFiltro != null)
                {
                    // Subconsulta dinámica usando el campo de filtro
                    whereClause = $@"
                        WHERE t1.[{campoRelacion12Tabla1}] = (
                            SELECT TOP 1 [Id]
                            FROM [{tabla1}]
                            WHERE [{campoFiltro}] = @valorFiltro
                        )";
                }

                string query = $@"
                    SELECT {topClause} {columnasSeleccionadas}
                    FROM [{tabla1}] AS t1
                    {tipoJoin} JOIN [{tabla2}] AS t2 ON t1.[{campoRelacion12Tabla1}] = t2.[{campoRelacion12Tabla2}]
                    {tipoJoin} JOIN [{tabla3}] AS t3 ON t2.[{campoRelacion23Tabla2}] = t3.[{campoRelacion23Tabla3}]
                    {whereClause};
                ";

                using var comando = new SqlCommand(query, conexion);

                if (!string.IsNullOrWhiteSpace(campoFiltro) && valorFiltro != null)
                    comando.Parameters.AddWithValue("@valorFiltro", valorFiltro);

                using var lector = await comando.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                while (await lector.ReadAsync())
                {
                    var fila = new Dictionary<string, object?>();
                    for (int i = 0; i < lector.FieldCount; i++)
                        fila[lector.GetName(i)] = await lector.IsDBNullAsync(i) ? null : lector.GetValue(i);

                    resultados.Add(fila);
                }

                return resultados;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                    $"Error de SQL Server al consultar el join: {excepcionSql.Message}. " +
                    $"Código de error SQL Server: {excepcionSql.Number}. " +
                    $"Verificar que la tabla existe y se tienen permisos de actualización.",
                    excepcionSql
                );
            }
        }
    }
}
