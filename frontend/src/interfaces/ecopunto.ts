export interface Ecopunto {
    id: number
    nombre: string
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
    nombre: string
    direccion: string
    latitud: number | string
    longitud: number | string
    materiales: string[]
    codigoDeEmpleado: string
}

export interface formDataUpdateForm extends formDataInsert {
    id: number
}

export interface formDataUpdateRequest {
    id: number
    nombre: string
    direccion: string
    latitud: string
    longitud: string
    materiales: MaterialesAceptado[]
    horario: string
    codigoDeEmpleado: string
}

export interface searchEcopuntoByIdResponse {
    mensaje: string
    ecoPunto: Ecopunto
}