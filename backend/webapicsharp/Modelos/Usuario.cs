using System.Text.Json.Serialization;

namespace webapicsharp.Modelos
{
    public class Usuario
    {

        [JsonIgnore] public int Id { get; set; }
        [JsonInclude] protected string? Nombre { get; set; }
        [JsonInclude] protected string? Cedula { get; set; }
        [JsonInclude] protected string? Correo { get; set; }
        [JsonInclude] protected string? Direccion { get; set; }
        [JsonInclude] protected string? Telefono { get; set; }
        [JsonIgnore] private string? Contraseña { get; set; }

        public Usuario(string nombre, string cedula, string correo, string direccion, string telefono, string contraseña)
        {
            Nombre = nombre;
            Cedula = cedula;
            Correo = correo;
            Direccion = direccion;
            Telefono = telefono;
            Contraseña = contraseña;
        }

        public string? ObtenerNombre() => Nombre;
        public string? ObtenerCedula() => Cedula;
        public string? ObtenerCorreo() => Correo;
        public string? ObtenerDireccion() => Direccion;
        public string? ObtenerTelefono() => Telefono;
        public string? ObtenerContraseña() => Contraseña;

        protected bool LogIn(string cedula, string contraseña)
        {
            return this.Cedula == cedula && this.Contraseña == contraseña;
        }

        public Usuario? Registrarse(Usuario NuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(NuevoUsuario.Nombre) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Cedula) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Correo) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Direccion) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Telefono) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Contraseña))
            {
                return null;
            }
            return NuevoUsuario;
        }
    }

    public class UsuarioDto{
        public string? Nombre { get; set; }
        public string? Cedula { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Contraseña { get; set; }
    }

}
