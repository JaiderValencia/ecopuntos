import { useState } from 'react'
import Card from '../../components/card'
import FlexCenter from '../../components/flexCenter'
import InputComponent from '../../components/input/texts'
import { inputsRegister as inputs } from '../../utils/register'
import { NavLink, useNavigate } from 'react-router-dom'
import type { registerForm } from '../../interfaces/register'
import { useForm } from 'react-hook-form'
import { registrarCliente } from '../../api/clientes'

function RegisterPage() {
    const { register, handleSubmit, formState: { errors } } = useForm<registerForm>()
    const navigate = useNavigate()
    const [step, setStep] = useState(1)
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const inputsRegister = inputs(register)

    const nextStep = () => {
        setStep(prevStep => prevStep + 1)
    }

    const prevStep = () => {
        setStep(prevStep => prevStep - 1)
    }

    const onSubmit = async (data: registerForm) => {
        try {
            setLoading(true)
            setError(null)
            
            const nombreCompleto = `${data.nombre} ${data.apellidos}`
            
            await registrarCliente({
                nombre: nombreCompleto,
                cedula: data.documento,
                correo: data.correo,
                direccion: data.direccion,
                telefono: data.telefono,
                contrasena: data.contraseña
            })
            
            alert('Cliente registrado exitosamente. Por favor inicia sesión.')
            navigate('/login')
        } catch (err: unknown) {
            const errorMessage = err && typeof err === 'object' && 'response' in err && err.response && typeof err.response === 'object' && 'data' in err.response && err.response.data && typeof err.response.data === 'object' && 'mensaje' in err.response.data && typeof err.response.data.mensaje === 'string' ? err.response.data.mensaje : 'Error al registrar el cliente'
            setError(errorMessage)
            console.error(err)
        } finally {
            setLoading(false)
        }
    }

    return (
        <FlexCenter>
            <Card className='bg-white p-8 rounded-lg shadow-lg w-full max-w-sm'>
                <h2 className="text-2xl font-bold text-center mb-6">Registrarse</h2>
                
                {error && (
                    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                        {error}
                    </div>
                )}
                
                <form onSubmit={handleSubmit(onSubmit)}>

                    {/* primera etapa */}
                    <div className={`${step === 1 ? 'block' : 'hidden'}`}>

                        {inputsRegister.stepOne.map(input => (
                            <InputComponent
                                key={input.inputId}
                                {...input}
                                classNameSpanAlert={input.classNameSpanAlert}
                                spanAlert={errors[input.inputName as keyof registerForm]?.message ?? ''}

                            />
                        ))}

                        <button className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-300"
                            type="button" onClick={nextStep}>Siguiente</button>

                        <div className="text-center mt-4">
                            <NavLink className="text-gray-500 hover:text-blue-600 text-sm" to="/login">Ya tengo cuenta</NavLink>
                        </div>
                    </div>

                    {/* segunda etapa */}
                    <div className={`${step === 2 ? 'block' : 'hidden'} grid grid-cols-2 grid-rows-3 gap-[8px]`}>
                        {inputsRegister.stepTwo.map((input, index) => (
                            <InputComponent divClassName={index == inputsRegister.stepTwo.length - 1 ? 'col-span-2' : ''}
                                key={input.inputId}
                                {...input}
                                classNameSpanAlert={input.classNameSpanAlert}
                                spanAlert={errors[input.inputName as keyof registerForm]?.message ?? ''}

                            />
                        ))}
                    </div>

                    <div className={`${step === 2 ? 'block' : 'hidden'} flex gap-3`}>
                        <button className="w-full bg-rose-600 text-white py-2 rounded-md hover:bg-rose-700 transition duration-300"
                            type="button" onClick={prevStep} disabled={loading}>Anterior</button>

                        <button className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-300 disabled:bg-blue-400 disabled:cursor-not-allowed"
                            type="submit" disabled={loading}>
                            {loading ? 'Registrando...' : 'Registrarse'}
                        </button>
                    </div>
                </form>
            </Card>
        </FlexCenter>
    )
}

export default RegisterPage