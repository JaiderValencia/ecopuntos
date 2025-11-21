import type { UseFormRegister } from 'react-hook-form'
import type { InputProps } from '../interfaces/Input'
import type { LoginFormData } from '../interfaces/login'

export const inputsLogin = (register: UseFormRegister<LoginFormData>): InputProps[] => {
    return [{
        label: 'Correo:',
        inputType: 'email',
        inputId: 'email',
        inputName: 'correo',
        classNameSpanAlert: 'text-rose-500',
        register: register('correo', {
            required: {
                value: true,
                message: 'El correo es obligatorio'
            },
            pattern: {
                value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                message: 'El correo no es valido'
            }
        })
    },
    {
        label: 'Contraseña:',
        inputType: 'password',
        inputId: 'password',
        inputName: 'contraseña',
        classNameSpanAlert: 'text-rose-500',
        register: register('contraseña', {
            required: {
                value: true,
                message: 'La contraseña es obligatoria'
            }
        })
    }
    ]
}