using Microsoft.Data.SqlClient;
using webapicsharp.Servicios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioAuditoria : IServicioAuditoria
    {
        private readonly IProveedorConexion proveedorConexion;
        public ServicioAuditoria(IProveedorConexion Conexion)
        {
            proveedorConexion = Conexion;
        }
        public async Task RegistrarAccionAsync(string accion, string? usuario = null)
        {
            try
            {
                using var con = new SqlConnection(proveedorConexion.ObtenerCadenaConexion());
                await con.OpenAsync();

                string sql = "INSERT INTO Auditoria (Usuario, Accion) VALUES (@Usuario, @Accion)";
                using var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Usuario", (object?)usuario ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Accion", accion);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUDITORIA ERROR] {ex.Message}");
            }
        }

    }
}
