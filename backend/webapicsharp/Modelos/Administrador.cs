namespace webapicsharp.Modelos
{
    public class Administrador : Empleado
    {
        public int NivelDeAcceso { get; set; }

        public Administrador(int id, string nombre, string cedula, string correo, string direccion, string telefono, string contrasena, string codigoDeEmpleado, int nivelDeAcceso)
                : base(id, nombre, cedula, correo, direccion, telefono, contrasena, codigoDeEmpleado)
        {
            NivelDeAcceso = nivelDeAcceso;
        }
    }

    public class AdministradorDto : EmpleadoDto
    {
        public int NivelDeAcceso { get; set; }
    }
}
