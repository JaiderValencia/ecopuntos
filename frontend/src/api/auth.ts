import type { LoginResponse } from '../interfaces/login/loginForm'
import axios from './axios'

export const login = async (email: string, password: string): Promise<LoginResponse> => {
    const response = await axios.post('/Jwt/Login', { email, password })

    sessionStorage.setItem('userSession', JSON.stringify({
        bearer: response.data.token,
        datosUsuario: response.data.datosUsuario
    }))

    return response.data
}
