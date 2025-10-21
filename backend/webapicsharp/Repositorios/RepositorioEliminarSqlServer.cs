using Microsoft.Data.SqlClient;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Repositorios
{
    public class RepositorioEliminarSqlServer : IRepositorioEliminarTabla
    {
        private readonly IProveedorConexion _proveedor;

        public RepositorioEliminarSqlServer (IProveedorConexion proveedor)
        {
            _proveedor = proveedor;
        }

        public async Task<bool> EliminarPorCampoAsync(string nombreTabla, string campoFiltro, object valorFiltro)
        {
            if (string.IsNullOrWhiteSpace(nombreTabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.");
            if (string.IsNullOrWhiteSpace(campoFiltro))
                throw new ArgumentException("El campo de filtro no puede estar vacío.");
            
            try
            {
                using var conexion = new SqlConnection(_proveedor.ObtenerCadenaConexion());
                await conexion.OpenAsync();

                using var comando = new SqlCommand($"DELETE FROM [{nombreTabla}] WHERE [{campoFiltro}] = @valor", conexion);
                comando.Parameters.AddWithValue("@valor", valorFiltro ?? DBNull.Value);

                int filasAfectadas = await comando.ExecuteNonQueryAsync();
                return filasAfectadas > 0;
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
