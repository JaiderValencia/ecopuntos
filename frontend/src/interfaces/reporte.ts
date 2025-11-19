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