using Microsoft.Data.SqlClient;
using System.Data;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioConsultaPersonalizadaSqlServer : IRepositorioConsultaPersonalizada
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioConsultaPersonalizadaSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<List<Dictionary<string, object?>>> EjecutarConsultaAsync(string consulta, Dictionary<string, object?>? parametros = null)
        {
            try
            {
                var resultados = new List<Dictionary<string, object?>>();

                if (string.IsNullOrWhiteSpace(consulta))
                    throw new ArgumentException("La consulta no puede estar vacía.");

                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                using var comando = new SqlCommand(consulta, conexion);

                if (parametros != null && parametros.Count > 0)
                {
                    foreach (var parametro in parametros)
                    {
                        comando.Parameters.AddWithValue(parametro.Key, parametro.Value ?? DBNull.Value);
                    }
                }

                using var lector = await comando.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                while (await lector.ReadAsync())
                {
                    var fila = new Dictionary<string, object?>();
                    for (int i = 0; i < lector.FieldCount; i++)
                    {
                        fila[lector.GetName(i)] = await lector.IsDBNullAsync(i) ? null : lector.GetValue(i);
                    }
                    resultados.Add(fila);
                }

                return resultados;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                    $"Error de SQL Server al ejecutar consulta personalizada: {excepcionSql.Message}. " +
                    $"Código de error SQL Server: {excepcionSql.Number}.",
                    excepcionSql
                );
            }
        }
    }
}
