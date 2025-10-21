using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioActualizarSqlServer : IRepositorioActualizarTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioActualizarSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<bool> ActualizarPorCampoAsync(string nombreTabla, string campoFiltro, object valorFiltro, Dictionary<string, object?> nuevosValores)
        {
            if (string.IsNullOrWhiteSpace(nombreTabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

            if (nuevosValores == null || nuevosValores.Count == 0)
                throw new ArgumentException("Debe proporcionar al menos una columna con valor.");

            try
            {
                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                //Construye el SET dinamicamente con partes
                var setPartes = new List<string>();
                var comando = new SqlCommand { Connection = conexion };

                int contador = 0;
                foreach (var keyValueParameter in nuevosValores)
                {
                    string nombreParametro = $"@valor{contador}";
                    setPartes.Add($"[{keyValueParameter.Key}] = {nombreParametro}");
                    comando.Parameters.AddWithValue(nombreParametro, keyValueParameter.Value ?? DBNull.Value);
                    contador++;
                }

                // Filtro (WHERE)
                comando.CommandText = $"UPDATE [{nombreTabla}] SET {string.Join(", ", setPartes)} WHERE [{campoFiltro}] = @filtro";
                comando.Parameters.AddWithValue("@filtro", valorFiltro ?? DBNull.Value);

                //Ejecutamos la consulta de actualizacion
                int filasAfectadas = await comando.ExecuteNonQueryAsync();
                return filasAfectadas > 0;

            } catch(SqlException excepcionSql)
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
