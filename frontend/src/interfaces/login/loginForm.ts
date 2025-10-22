export interface LoginFormData {
    correo: string
    contraseña: string
}

export interface LoginResponse {
    bearer: string
    datosUsuario: {
        id: string
        nombre: string
        correo: string
        telefono: string
    },
    mensaje: string
}