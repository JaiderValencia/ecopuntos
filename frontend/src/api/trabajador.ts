import type { TrabajadorFormData, Trabajador } from '../interfaces/trabajador'
import axios from './axios'

export const crearTrabajador = async (data: TrabajadorFormData): Promise<Trabajador> => {
    const response = await axios.post('/Trabajador/CrearTrabajador', {
        nombre: data.nombre,
        cedula: data.cedula,
        correo: data.correo,
        direccion: data.direccion,
        telefono: data.telefono,
        contrasena: data.contrasena,
        codigoDeEmpleado: data.codigoDeEmpleado,
        horario: data.horario
    })

    return response.data.trabajador
}

export const obtenerTrabajadores = async (limit: number = 50): Promise<Trabajador[]> => {
    const response = await axios.get('/Trabajador/ObtenerTrabajadores', {
        params: { limit }
    })

    return response.data.empleados
}
