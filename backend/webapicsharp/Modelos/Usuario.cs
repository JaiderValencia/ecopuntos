using System.Text.Json.Serialization;

namespace webapicsharp.Modelos
{
    public abstract class Usuario
    {

        [JsonIgnore] public int Id { get; set; }
        [JsonInclude] protected string? Nombre { get; set; }
        [JsonInclude] protected string? Cedula { get; set; }
        [JsonInclude] protected string? Correo { get; set; }
        [JsonInclude] protected string? Direccion { get; set; }
        [JsonInclude] protected string? Telefono { get; set; }
        [JsonIgnore] protected string? Contrasena { get; set; }

        public Usuario( string nombre, string cedula, string correo, string direccion, string telefono, string contrasena)
        {
            Nombre = nombre;
            Cedula = cedula;
            Correo = correo;
            Direccion = direccion;
            Telefono = telefono;
            Contrasena = contrasena;
        }

        public Usuario(int id, string nombre, string cedula, string correo, string direccion, string telefono, string contrasena)
        {
            Id = id;
            Nombre = nombre;
            Cedula = cedula;
            Correo = correo;
            Direccion = direccion;
            Telefono = telefono;
            Contrasena = contrasena;
        }

        public string? ObtenerNombre() => Nombre;
        public string? ObtenerCedula() => Cedula;
        public string? ObtenerCorreo() => Correo;
        public string? ObtenerDireccion() => Direccion;
        public string? ObtenerTelefono() => Telefono;
        public string? ObtenerContrasena() => Contrasena;

        public bool LogIn(string correo, string contrasena)
        {
            return this.Correo == correo && this.Contrasena == contrasena;
        }

        public Usuario? Registrarse(Usuario NuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(NuevoUsuario.Nombre) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Cedula) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Correo) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Direccion) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Telefono) ||
                string.IsNullOrWhiteSpace(NuevoUsuario.Contrasena))
            {
                return null;
            }
            return NuevoUsuario;
        }
    }

    public abstract class UsuarioDto{
        public string? Nombre { get; set; }
        public string? Cedula { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Contrasena { get; set; }
    }

}
