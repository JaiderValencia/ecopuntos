export interface Trabajador {
    id: number
    nombre: string
    cedula: string
    correo: string
    direccion: string
    telefono: string
    codigoDeEmpleado: string
    horario: string
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
