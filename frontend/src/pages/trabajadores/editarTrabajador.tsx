import { useEffect, useState } from 'react'
import { useForm } from 'react-hook-form'
import { useNavigate, useParams } from 'react-router-dom'
import InputComponent from '../../components/input/texts'
import type { TrabajadorUpdateFormData, Trabajador } from '../../interfaces/trabajador'
import { actualizarTrabajador, obtenerTrabajadores } from '../../api/trabajador'

function EditarTrabajador() {
    const { id } = useParams<{ id: string }>()
    const { register, handleSubmit, formState: { errors }, setValue } = useForm<TrabajadorUpdateFormData>()
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)
    const [isFetching, setIsFetching] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState(false)

    useEffect(() => {
        const cargarTrabajador = async () => {
            if (!id) {
                setError('ID de trabajador no válido')
                setIsFetching(false)
                return
            }

            try {
                setIsFetching(true)
                const trabajadores = await obtenerTrabajadores(100)
                const trabajador = trabajadores.find((t: Trabajador) => t.Id === parseInt(id))

                if (!trabajador) {
                    setError('Trabajador no encontrado')
                    return
                }

                // Cargar datos en el formulario
                setValue('id', trabajador.Id)
                setValue('nombre', trabajador.Nombre)
                setValue('cedula', trabajador.Cedula)
                setValue('correo', trabajador.Correo)
                setValue('telefono', trabajador.Telefono)
                setValue('direccion', trabajador.Direccion)
                setValue('horario', trabajador.Horario)
            } catch (error) {
                console.error('Error al cargar trabajador:', error)
                setError('Error al cargar los datos del trabajador')
            } finally {
                setIsFetching(false)
            }
        }

        cargarTrabajador()
    }, [id, setValue])

    const onSubmit = async (data: TrabajadorUpdateFormData) => {
        setIsLoading(true)
        setError(null)
        setSuccess(false)

        try {
            await actualizarTrabajador(data)
            setSuccess(true)
            setTimeout(() => {
                navigate('/trabajadores/lista')
            }, 1500)
        } catch (error: unknown) {
            console.error('Error al actualizar trabajador:', error)
            const err = error as { response?: { data?: { mensaje?: string } } }
            setError(err.response?.data?.mensaje || 'Error al actualizar el trabajador')
        } finally {
            setIsLoading(false)
        }
    }

    if (isFetching) {
        return (
            <div className="text-center py-8">
                <p className="text-gray-600">Cargando datos del trabajador...</p>
            </div>
        )
    }

    if (error && !success) {
        return (
            <div className="max-w-4xl mx-auto p-4">
                <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                    {error}
                </div>                
            </div>
        )
    }

    return (
        <div className="max-w-4xl mx-auto p-4">
            <div className="flex justify-between items-center mb-6">
                <h2 className="text-2xl font-bold text-text-light dark:text-text-dark">
                    Editar Trabajador
                </h2>                
            </div>

            {error && (
                <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                    {error}
                </div>
            )}

            {success && (
                <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded mb-4">
                    Trabajador actualizado exitosamente. Redirigiendo...
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
                            label="Nueva contraseña (opcional)"
                            inputType="password"
                            inputPlaceholder="Dejar vacío para no cambiar"
                            inputId="contrasena"
                            inputName="contrasena"
                            register={register('contrasena', {
                                minLength: { value: 6, message: 'Mínimo 6 caracteres' }
                            })}
                            spanAlert={errors.contrasena?.message}
                            classNameSpanAlert="text-red-500 text-sm"
                        />
                    </div>

                    <div className="flex gap-4 mt-6">
                        <button
                            type="button"
                            onClick={() => navigate('/trabajadores/lista')}
                            className="flex-1 bg-gray-500 hover:bg-gray-600 text-white font-bold py-2 px-4 rounded"
                        >
                            Cancelar
                        </button>
                        <button
                            type="submit"
                            disabled={isLoading}
                            className="flex-1 bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded disabled:bg-gray-400"
                        >
                            {isLoading ? 'Actualizando...' : 'Actualizar Trabajador'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default EditarTrabajador
