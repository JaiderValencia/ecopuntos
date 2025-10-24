using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioEscrituraSqlServer : IRepositorioEscrituraTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioEscrituraSqlServer(IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<Dictionary<string, object?>> InsertarAsync(string nombreTabla, Dictionary<string, object?> valores)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreTabla))
                    throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

                if (valores == null || valores.Count == 0)
                    throw new ArgumentException("Debe proporcionar al menos una columna con valor.");

                var columnas = string.Join(", ", valores.Keys);
                var parametros = string.Join(", ", valores.Keys.Select(k => $"@{k}"));

                var sql = $"INSERT INTO [{nombreTabla}] ({columnas}) OUTPUT INSERTED.* VALUES ({parametros});";

                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                using var comando = new SqlCommand(sql, conexion);

                foreach (var kvp in valores)
                    comando.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);

                await conexion.OpenAsync();

                using var reader = await comando.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var resultado = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultado[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
                    }
                    return resultado;
                }

                throw new InvalidOperationException("No se pudo insertar el registro o recuperar los datos insertados.");
            }
            catch (SqlException excepcionSql)
            {
                throw new InvalidOperationException(
                    $"Error de SQL Server al insertar en la tabla '{nombreTabla}': {excepcionSql.Message}. " +
                    $"Código SQL: {excepcionSql.Number}. Verifique permisos y existencia de la tabla.",
                    excepcionSql
                );
            }
        }

    }
}