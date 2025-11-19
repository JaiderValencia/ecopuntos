import type { Reporte } from '../interfaces/reporte'
import axios from './axios'

export const registrarReporte = async (data: Reporte): Promise<void> => {
    const response = await axios.post('/Entrega/CrearEntrega', data)

    return response.data
}