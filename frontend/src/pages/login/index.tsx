import { useForm } from 'react-hook-form'
import { useNavigate, NavLink } from 'react-router-dom'
import { useState } from 'react'
import FlexCenter from '../../components/flexCenter'
import InputComponent from '../../components/input/texts'
import type { LoginFormData } from '../../interfaces/login'
import { inputsLogin as inputs } from '../../utils/login'
import { login } from '../../api/auth'
import { useUserContext } from '../../contex/user/user'
import Card from '../../components/card'


function Login() {
    const { register, handleSubmit, formState: { errors } } = useForm<LoginFormData>()
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const inputsLogin = inputs(register)

    const navigate = useNavigate()

    const { setUserStatus } = useUserContext();

    const onSubmit = async (data: LoginFormData) => {
        try {
            setLoading(true)
            setError(null)
            
            const dataResponse = await login(data.correo, data.contraseña)

            setUserStatus({
                isLogged: true,
                userId: Number(dataResponse.datosUsuario.id),
                userName: dataResponse.datosUsuario.nombre,
                userEmail: dataResponse.datosUsuario.correo,
                userPhone: dataResponse.datosUsuario.telefono,
                userRole: dataResponse.datosUsuario.rol
            })

            navigate('/')
        } catch (err: unknown) {
            const errorMessage = (err as { response?: { data?: { mensaje?: string } } }).response?.data?.mensaje || 'Credenciales incorrectas. Por favor verifica tu correo y contraseña.'
            setError(errorMessage)
            console.error("Error al iniciar sesión:", err)
        } finally {
            setLoading(false)
        }
    }

    return (
        <FlexCenter>
            <Card className='bg-white p-8 rounded-lg shadow-lg w-full max-w-sm'>
                <h2 className="text-2xl font-bold text-center mb-6">Iniciar Sesion</h2>
                
                {error && (
                    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                        {error}
                    </div>
                )}
                
                <form onSubmit={handleSubmit(onSubmit)}>

                    {inputsLogin.map(input => (
                        <InputComponent key={input.inputId} {...input} classNameSpanAlert={input.classNameSpanAlert} spanAlert={errors[input.inputName as keyof LoginFormData]?.message ?? ''} />
                    ))}

                    <button className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-300 disabled:bg-blue-400 disabled:cursor-not-allowed"
                        type="submit" disabled={loading}>
                        {loading ? 'Iniciando sesión...' : 'Iniciar sesión'}
                    </button>
                </form>
                <div className="text-center mt-4">
                    <NavLink className="text-gray-500 hover:text-blue-600 text-sm" to="/registro">Registrarse</NavLink>
                </div>
            </Card>
        </FlexCenter>
    )
}

export default Login