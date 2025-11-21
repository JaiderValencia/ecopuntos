import type { ClienteDetallado } from '../interfaces/cliente'
import axios from './axios'

export const obtenerClientePorCorreo = async (correo: string): Promise<ClienteDetallado> => {
    const response = await axios.get(`/Clientes/BuscarClientePorCorreo?correo=${correo}`)
    return response.data.cliente
}
