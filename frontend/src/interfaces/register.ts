import type { InputProps } from './Input'

export interface registerForm {
    correo: string
    nombre: string
    apellidos: string
    direccion: string
    telefono: string
    documento: string
    contraseña: string
}

export interface InputsSteps {
    stepOne: InputProps[]
    stepTwo: InputProps[]
}