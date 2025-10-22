using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;
using System.Data;

namespace webapicsharp.Repositorios
{
    public class RepositorioActualizarSqlServer : IRepositorioActualizarTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioActualizarSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<Dictionary<string, object?>> ActualizarPorCampoAsync(
            string nombreTabla,
            string campoFiltro,
            object valorFiltro,
            Dictionary<string, object?> nuevosValores)
        {
            if (string.IsNullOrWhiteSpace(nombreTabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.", nameof(nombreTabla));

            if (nuevosValores == null || nuevosValores.Count == 0)
                throw new ArgumentException("Debe proporcionar al menos una columna con valor.", nameof(nuevosValores));

            try
            {
                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                // Construcción del SET dinámicamente
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

                // Parámetro de filtro
                comando.Parameters.AddWithValue("@filtro", valorFiltro ?? DBNull.Value);

                // Construcción de la consulta con OUTPUT para devolver los valores actualizados
                string setClause = string.Join(", ", setPartes);
                comando.CommandText = $@"
                    UPDATE [{nombreTabla}]
                    SET {setClause}
                    OUTPUT inserted.*
                    WHERE [{campoFiltro}] = @filtro;";

                var filaActualizada = new Dictionary<string, object?>();

                using var lector = await comando.ExecuteReaderAsync(CommandBehavior.SingleRow);

                if (await lector.ReadAsync())
                {
                    for (int i = 0; i < lector.FieldCount; i++)
                    {
                        string nombreColumna = lector.GetName(i);
                        object? valor = lector.IsDBNull(i) ? null : lector.GetValue(i);
                        filaActualizada[nombreColumna] = valor;
                    }
                }

                return filaActualizada;
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                    $"Error de SQL Server al actualizar la tabla '{nombreTabla}': {excepcionSql.Message}. " +
                    $"Código de error SQL Server: {excepcionSql.Number}. " +
                    $"Verificar que la tabla existe y se tienen permisos de actualización.",
                    excepcionSql
                );
            }
        }
    }
}
