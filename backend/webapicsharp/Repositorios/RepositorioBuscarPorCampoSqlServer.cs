using System.Data;
using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioBuscarPorCampoSqlServer : IRepositorioBusquedaPorCampoTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioBuscarPorCampoSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<List<Dictionary<string, object?>>?> BuscarPorCampoAsync(string tabla, string campo, object valor)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tabla))
                    throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

                if (campo == null)
                    throw new ArgumentException("Debe proporcionar un campo donde buscar.");

                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                var consulta = new SqlCommand($"SELECT * FROM [{tabla}] WHERE [{campo}] = @valor", conexion);
                consulta.Parameters.AddWithValue("@valor", valor ?? DBNull.Value);

                await conexion.OpenAsync();
                using var lector = await consulta.ExecuteReaderAsync();

                var resultados = new List<Dictionary<string, object?>>();

                while (await lector.ReadAsync())
                {
                    var registro = new Dictionary<string, object?>();
                    for (int i = 0; i < lector.FieldCount; i++)
                        registro[lector.GetName(i)] = await lector.IsDBNullAsync(i) ? null : lector.GetValue(i);

                    resultados.Add(registro);
                }

                if (resultados.Count == 0)
                    return null;

                return resultados;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                   $"Error de SQL Server al consultar la subconsulta: {excepcionSql.Message}. " +
                   $"Código de error SQL Server: {excepcionSql.Number}. " +
                   $"Verificar que la tabla existe y se tienen permisos de actualizacion.",
                   excepcionSql
                );
            }
        }
    }
}