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

        public async Task<bool> InsertarAsync(string nombreTabla, Dictionary<string, object?> valores)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreTabla))
                    throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

                if (valores == null || valores.Count == 0)
                    throw new ArgumentException("Debe proporcionar al menos una columna con valor.");

                var columnas = string.Join(", ", valores.Keys);
                var parametros = string.Join(", ", valores.Keys.Select(k => $"@{k}"));
                var sql = $"INSERT INTO [{nombreTabla}] ({columnas}) VALUES ({parametros});";

                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                using var comando = new SqlCommand(sql, conexion);

                foreach (var kvp in valores)
                    comando.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);

                await conexion.OpenAsync();
                var filas = await comando.ExecuteNonQueryAsync();
                return filas > 0;
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