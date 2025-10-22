using Microsoft.Data.SqlClient;
using System.Data;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioSubconsultaSqlServer : IRepositorioSubconsulta
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioSubconsultaSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<List<Dictionary<string, object?>>?> EjecutarSubconsultaAsync(
            string tablaExterna,
            string tablaInterna,
            string campoRelacionExterna,
            string campoRelacionInterna,
            string campoFiltro,
            object valorFiltro
        )
        {
            try
            {
                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                string consulta = $@"
                    SELECT 
                        u.*, 
                        c.*
                    FROM [{tablaExterna}] AS c
                    INNER JOIN [{tablaInterna}] AS u
                        ON c.[{campoRelacionExterna}] = u.[{campoRelacionInterna}]
                    WHERE u.[{campoFiltro}] = @valor;
                ";

                using var comando = new SqlCommand(consulta, conexion);
                comando.Parameters.AddWithValue("@valor", valorFiltro ?? DBNull.Value);

                using var lector = await comando.ExecuteReaderAsync(); // <-- todos los registros

                var resultados = new List<Dictionary<string, object?>>();

                while (await lector.ReadAsync())
                {
                    var registro = new Dictionary<string, object?>();
                    for (int i = 0; i < lector.FieldCount; i++)
                        registro[lector.GetName(i)] = await lector.IsDBNullAsync(i) ? null : lector.GetValue(i);

                    resultados.Add(registro);
                }

                return resultados.Count > 0 ? resultados : null;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                   $"Error de SQL Server al consultar la subconsulta: {excepcionSql.Message}. " +
                   $"Código de error SQL Server: {excepcionSql.Number}. " +
                   $"Verificar que la tabla existe y se tienen permisos de actualización.",
                   excepcionSql
                );
            }

        }
    }
}