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
    latitud: number | string
    longitud: number | string
    direccion: string
}

export interface EcopuntoRequestGet {
    limite: number
}

export interface formDataInsert {
    direccion: string
    latitud: number
    longitud: number
    materiales: string[]
    encargado: string
}

export interface formDataUpdate extends formDataInsert {
    id: number
}

export interface searchEcopuntoByIdResponse {
    mensaje: string
    ecoPunto: Ecopunto
}