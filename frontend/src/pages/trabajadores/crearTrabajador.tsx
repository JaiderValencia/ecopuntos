import { useState } from 'react'
import { useForm } from 'react-hook-form'
import InputComponent from '../../components/input/texts'
import type { TrabajadorFormData } from '../../interfaces/trabajador'
import { crearTrabajador } from '../../api/trabajador'

function CrearTrabajador() {
    const { register, handleSubmit, formState: { errors }, reset } = useForm<TrabajadorFormData>()    
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState(false)

    const onSubmit = async (data: TrabajadorFormData) => {
        setIsLoading(true)
        setError(null)
        setSuccess(false)

        try {
            await crearTrabajador(data)
            setSuccess(true)
            reset()
            setTimeout(() => {
                setSuccess(false)
            }, 3000)
        } catch (error: unknown) {
            console.error('Error al crear trabajador:', error)
            const err = error as { response?: { data?: { mensaje?: string } } }
            setError(err.response?.data?.mensaje || 'Error al crear el trabajador')
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <div className="max-w-4xl mx-auto p-4">
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-text-light dark:text-text-dark">
                    Crear Trabajador
                </h2>                
            </div>

            {error && (
                <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                    {error}
                </div>
            )}

            {success && (
                <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded mb-4">
                    Trabajador creado exitosamente
                </div>
            )}

            <div className="bg-white rounded-lg shadow-md p-6">
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <InputComponent
                            label="Nombre completo"
                            inputType="text"
                            inputPlaceholder="Ingrese el nombre completo"
                            inputId="nombre"
                            inputName="nombre"
                            register={register('nombre', {
                                required: 'El nombre es requerido',
                                minLength: { value: 3, message: 'Mínimo 3 caracteres' }
                            })}
                            spanAlert={errors.nombre?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />

                        <InputComponent
                            label="Cédula"
                            inputType="text"
                            inputPlaceholder="Ingrese la cédula"
                            inputId="cedula"
                            inputName="cedula"
                            register={register('cedula', {
                                required: 'La cédula es requerida',
                                pattern: { value: /^[0-9]+$/, message: 'Solo números' }
                            })}
                            spanAlert={errors.cedula?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />

                        <InputComponent
                            label="Correo electrónico"
                            inputType="email"
                            inputPlaceholder="correo@ejemplo.com"
                            inputId="correo"
                            inputName="correo"
                            register={register('correo', {
                                required: 'El correo es requerido',
                                pattern: {
                                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                    message: 'Correo inválido'
                                }
                            })}
                            spanAlert={errors.correo?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />

                        <InputComponent
                            label="Teléfono"
                            inputType="text"
                            inputPlaceholder="Ingrese el teléfono"
                            inputId="telefono"
                            inputName="telefono"
                            register={register('telefono', {
                                required: 'El teléfono es requerido',
                                pattern: { value: /^[0-9]{10}$/, message: '10 dígitos requeridos' }
                            })}
                            spanAlert={errors.telefono?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />

                        <InputComponent
                            label="Dirección"
                            inputType="text"
                            inputPlaceholder="Ingrese la dirección"
                            inputId="direccion"
                            inputName="direccion"
                            register={register('direccion', {
                                required: 'La dirección es requerida',
                                minLength: { value: 5, message: 'Mínimo 5 caracteres' }
                            })}
                            spanAlert={errors.direccion?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />

                        <InputComponent
                            label="Horario"
                            inputType="text"
                            inputPlaceholder="Ej: 8:00 AM - 5:00 PM"
                            inputId="horario"
                            inputName="horario"
                            register={register('horario', {
                                required: 'El horario es requerido'
                            })}
                            spanAlert={errors.horario?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />

                        <InputComponent
                            label="Contraseña"
                            inputType="password"
                            inputPlaceholder="Ingrese la contraseña"
                            inputId="contrasena"
                            inputName="contrasena"
                            register={register('contrasena', {
                                required: 'La contraseña es requerida',
                                minLength: { value: 6, message: 'Mínimo 6 caracteres' }
                            })}
                            spanAlert={errors.contrasena?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />
                    </div>

                    <div className="flex gap-4 mt-6">
                        <button
                            type="button"
                            onClick={() => reset()}
                            className="flex-1 bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded"
                        >
                            Limpiar
                        </button>
                        <button
                            type="submit"
                            disabled={isLoading}
                            className="flex-1 bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded disabled:bg-gray-400"
                        >
                            {isLoading ? 'Creando...' : 'Crear Trabajador'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default CrearTrabajador
