import type { EmpleadoResponse } from '../interfaces/empleados'
import axios from './axios'

export const getEmpleados = async ({ limit }: { limit: number }): Promise<EmpleadoResponse> => {
    const response = await axios.get('/Trabajador/ObtenerTrabajadores', {
        params: {
            limit
        }
    })

    return response.data
}