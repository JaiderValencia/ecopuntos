export interface Ecopunto {
    id: number
    horario: string
    trabajador: Trabajador
    materialesAceptados: MaterialesAceptado[]
    ubicacion: Ubicacion
}

interface Trabajador {
    horario: string
    codigoDeEmpleado: string
    id: number
    nombre: string
    cedula: string
    correo: string
    direccion: string
    telefono: string
}

export interface MaterialesAceptado {
    id: number
    nombre: string
    peso: number
}

interface Ubicacion {
    latitud: number
    longitud: number
    direccion: string
}

export interface EcopuntoRequest {
    limite: number
}