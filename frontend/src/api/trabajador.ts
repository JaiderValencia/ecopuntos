import type { TrabajadorFormData, Trabajador, TrabajadorUpdateFormData } from '../interfaces/trabajador'
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

export const obtenerTrabajadorPorCorreo = async (correo: string): Promise<Trabajador> => {
    const response = await axios.get('/Trabajador/BuscarTrabajadorPorCorreo', {
        params: { correo }
    })

    return response.data.cliente
}

export const actualizarTrabajador = async (data: TrabajadorUpdateFormData): Promise<void> => {
    const payload: Record<string, unknown> = {
        id: data.id,
        nombre: data.nombre,
        cedula: data.cedula,
        correo: data.correo,
        direccion: data.direccion,
        telefono: data.telefono,
        codigoDeEmpleado: data.codigoDeEmpleado,
        horario: data.horario
    }

    // Solo incluir contraseña si se proporcionó
    if (data.contrasena && data.contrasena.trim() !== '') {
        payload.contrasena = data.contrasena
    }

    await axios.put('/Trabajador/ActualizarTrabajador', payload)
}
