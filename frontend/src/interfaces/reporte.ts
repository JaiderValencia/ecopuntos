import type { MaterialEntrega } from './materiales'

export interface Reporte {
    idCliente: number
    idTrabajador: number
    idEcoPunto: number
    materialesEntrega: MaterialEntrega[]
}

export interface ReporteForm extends Pick<Reporte, 'idEcoPunto' | 'idCliente'> {
    materialesEntrega: number
    cantidad: number
    estado: boolean    
}