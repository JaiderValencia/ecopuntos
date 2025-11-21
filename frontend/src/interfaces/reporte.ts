import type { MaterialEntrega } from './materiales'

export interface Reporte {
    cedulaCliente: string
    idTrabajador: number
    idEcoPunto: number
    materialesEntrega: MaterialEntrega[]
}

export interface ReporteForm extends Pick<Reporte, 'idEcoPunto' | 'cedulaCliente'> {
    materialesEntrega: number
    cantidad: number
    estado: boolean    
}

export interface ReporteAllDto {
    idReporte: number
    fechaCreacion: string
    nombreEcopunto: string
    responsable: string
}

export interface ClienteReporte {
    id: number
    nombre: string
    cedula: string
    correo: string
    direccion: string
    telefono: string
    ecoPuntos: number
}

export interface TrabajadorReporte {
    id: number
    nombre: string
    cedula: string
    correo: string
    direccion: string
    telefono: string
    codigoDeEmpleado: string
    horario: string
}

export interface ReporteDetalle {
    id: number
    cliente: ClienteReporte
    trabajador: TrabajadorReporte
    materialesEntrega: Array<{
        [key: string]: string | number | boolean
    }>
    top3: {
        [key: string]: string
    }
    totales: {
        [key: string]: number
    }
}