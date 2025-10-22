using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioBuscarUltimoSqlServer : IRepositorioBuscarUltimoTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioBuscarUltimoSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<Dictionary<string, object>?> BuscarUltimoAsync(string nombreTabla, string campoOrden)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreTabla))
                    throw new ArgumentException("El nombre de la tabla no puede estar vacío.", nameof(nombreTabla));

                if (string.IsNullOrWhiteSpace(campoOrden))
                    throw new ArgumentException("El campo de orden no puede estar vacío.", nameof(campoOrden));

                var sql = $"SELECT TOP 1 * FROM [{nombreTabla}] ORDER BY [{campoOrden}] DESC;";

                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                using var comando = new SqlCommand(sql, conexion);

                await conexion.OpenAsync();

                using var reader = await comando.ExecuteReaderAsync();
                if (!reader.HasRows)
                    return null;

                await reader.ReadAsync();

                var resultado = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var nombreColumna = reader.GetName(i);
                    var valor = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    resultado[nombreColumna] = valor!;
                }

                return resultado;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                   $"Error de SQL Server al consultar la tabla '{nombreTabla}': {excepcionSql.Message}. " +
                   $"Código de error SQL Server: {excepcionSql.Number}. " +
                   $"Verificar que la tabla existe y se tienen permisos de actualizacion.",
                   excepcionSql
                );
            }
        }
    }
}
