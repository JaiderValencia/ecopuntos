export interface Trabajador {
    Id: number
    Nombre: string
    Cedula: string
    Correo: string
    Direccion: string
    Telefono: string
    CodigoDeEmpleado: string
    Horario: string
}

export interface TrabajadorFormData {
    nombre: string
    cedula: string
    correo: string
    direccion: string
    telefono: string
    contrasena: string
    codigoDeEmpleado: string
    horario: string
}
