import type { EcopuntoRequest } from '../interfaces/ecopunto'
import axios from './axios'

export const getEcopuntos = async ({ limite }: EcopuntoRequest) => {
    const response = await axios.get('/EcoPunto/ObtenerEcopuntos', {
        params: {
            limite
        }
    })
    return response.data
}