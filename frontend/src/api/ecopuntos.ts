import type { Ecopunto, EcopuntoRequestGet } from '../interfaces/ecopunto'
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
    console.log(formData)

    const response = await axios.post('/EcoPunto/CrearEcopunto', formData)

    return response.data
}