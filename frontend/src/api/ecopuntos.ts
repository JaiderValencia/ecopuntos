import type { Ecopunto, EcopuntoRequestGet, searchEcopuntoByIdResponse } from '../interfaces/ecopunto'
import axios from './axios'

export const getEcopuntos = async ({ limite }: EcopuntoRequestGet) => {
    const response = await axios.get('/EcoPunto/ObtenerEcopuntos', {
        params: {
            limite
        }
    })
    return response.data
}

export const createEcopunto = async (formData: Ecopunto) => {
    const response = await axios.post('/EcoPunto/CrearEcopunto', formData)

    return response.data
}

export const searchEcopuntoById = async (id: number): Promise<searchEcopuntoByIdResponse> => {
    const response = await axios.post(`/EcoPunto/BuscarEcoPuntoPorID?id=${id}`)

    return response.data
}