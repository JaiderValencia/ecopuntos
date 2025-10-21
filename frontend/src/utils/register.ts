import type { UseFormRegister } from 'react-hook-form'
import type { InputsSteps } from '../interfaces/register/inputsSteps'
import type { registerForm } from '../interfaces/register/registerForm'

export const inputsRegister = (register: UseFormRegister<registerForm>): InputsSteps => ({
    stepOne: [{
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
        inputId: 'nombre',
        inputName: 'nombre',
        inputType: 'text',
        label: 'Nombre:',
        classNameSpanAlert: 'text-rose-500',
        register: register('nombre', {
            required: {
                value: true,
                message: 'El nombre es obligatorio'
            }
        })
    }],
    stepTwo: [{
        inputId: 'apellidos',
        inputName: 'apellidos',
        inputType: 'text',
        label: 'Apellidos:',
        classNameSpanAlert: 'text-rose-500',
        register: register('apellidos', {
            required: {
                value: true,
                message: 'Los apellidos son obligatorios'
            }
        })
    },
    {
        inputId: 'direccion',
        inputName: 'direccion',
        inputType: 'text',
        label: 'Direccion:',
        classNameSpanAlert: 'text-rose-500',
        register: register('direccion', {
            required: {
                value: true,
                message: 'La direccion es obligatoria'
            }
        })
    }, {
        inputId: 'telefono',
        inputName: 'telefono',
        inputType: 'tel',
        label: 'Telefono celular:',
        classNameSpanAlert: 'text-rose-500',
        register: register('telefono', {
            required: {
                value: true,
                message: 'El telefono es obligatorio'
            }
        })
    },
    {
        inputId: 'documento',
        inputName: 'documento',
        inputType: 'text',
        label: 'identificacion:',
        classNameSpanAlert: 'text-rose-500',
        register: register('documento', {
            required: {
                value: true,
                message: 'La identificacion es obligatoria'
            }
        })
    }, {
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
    }]
})