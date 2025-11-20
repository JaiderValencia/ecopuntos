import type { Reporte, ReporteAllDto, ReporteDetalle } from '../interfaces/reporte'
import axios from './axios'

export const registrarReporte = async (data: Reporte): Promise<void> => {
    const response = await axios.post('/Entrega/CrearEntrega', data)

    return response.data
}

export const obtenerReportesPorIdCliente = async (idCliente: number): Promise<ReporteAllDto[]> => {
    const response = await axios.get('/Reporte/ObtenerReportesPorIdCliente', {
        params: { idCliente }
    })

    return response.data.reportes
}

export const obtenerReportesPorIdTrabajador = async (idTrabajador: number): Promise<ReporteAllDto[]> => {
    const response = await axios.get('/Reporte/ObtenerReportesPorIdTrabajador', {
        params: { idTrabajador }
    })
    
    return response.data.reportes
}

export const obtenerInformacionReportePorId = async (idReporte: number): Promise<ReporteDetalle> => {
    const response = await axios.get('/Reporte/ObtenerInformacionReportePorId', {
        params: { idReporte }
    })

    return response.data.reporte
}