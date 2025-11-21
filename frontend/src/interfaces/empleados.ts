export interface Empleado {
    Id: number
    Nombre: string
    Cedula: string
    Correo: string
    Direccion: string
    Telefono: string
    Contrasena: string
    CodigoDeEmpleado: string
    Horario: string
}

export interface EmpleadoResponse {
    limit: number
    total: number
    mensaje: string
    empleados: Empleado[]
}