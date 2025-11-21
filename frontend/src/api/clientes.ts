import type { ClienteDetallado } from '../interfaces/cliente'
import axios from './axios'

export const obtenerClientePorCorreo = async (correo: string): Promise<ClienteDetallado> => {
    const response = await axios.get(`/Clientes/BuscarClientePorCorreo?correo=${correo}`)
    return response.data.cliente
}

export const registrarCliente = async (datos: {
    nombre: string
    cedula: string
    correo: string
    direccion: string
    telefono: string
    contrasena: string
}) => {
    const response = await axios.post('/Clientes/CrearCliente', {
        Nombre: datos.nombre,
        Cedula: datos.cedula,
        Correo: datos.correo,
        Direccion: datos.direccion,
        Telefono: datos.telefono,
        Contrasena: datos.contrasena
    })
    return response.data
}
