namespace webapicsharp.Modelos
{
    public class Trabajador : Empleado
    {
        public string? Horario { get; set; }

        public Trabajador(int id, string nombre, string cedula, string correo, string direccion, string telefono, string contrasena, string codigoDeEmpleado, string horario)
                : base(id, nombre, cedula, correo, direccion, telefono, contrasena, codigoDeEmpleado)
        {
            Horario = horario;
        }
    }

    public class TrabajadorDto : EmpleadoDto
    {
        public string? Horario { get; set; }
    }
}
