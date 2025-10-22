namespace webapicsharp.Modelos
{
    public abstract class Empleado : Usuario
    {
        public string? CodigoDeEmpleado { get; set; }

        public Empleado(int id, string nombre, string cedula, string correo, string direccion, string telefono, string contrasena, string codigoDeEmpleado)
            : base(id, nombre, cedula, correo, direccion, telefono, contrasena)

        {
            CodigoDeEmpleado = codigoDeEmpleado;
        }
    }

    public class EmpleadoDto : UsuarioDto
    {
        public string? CodigoDeEmpleado { get; set; }
    }
}
