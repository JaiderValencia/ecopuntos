import { useForm } from 'react-hook-form'
import { useNavigate, NavLink } from 'react-router-dom'
import FlexCenter from '../../components/flexCenter'
import InputComponent from '../../components/input/texts'
import type { LoginFormData } from '../../interfaces/login/loginForm'
import { inputsLogin as inputs } from '../../utils/login'
import { login } from '../../api/auth'
import { useUserContext } from '../../contex/user'


function Login() {
    const { register, handleSubmit, formState: { errors } } = useForm<LoginFormData>()

    const inputsLogin = inputs(register)

    const navigate = useNavigate()

    const { setUserStatus } = useUserContext();

    const onSubmit = async (data: LoginFormData) => {
        try {
            const dataResponse = await login(data.correo, data.contraseña)

            setUserStatus({
                isLogged: true,
                userId: dataResponse.datosUsuario.id,
                userName: dataResponse.datosUsuario.nombre,
                userEmail: dataResponse.datosUsuario.correo,
                userPhone: dataResponse.datosUsuario.telefono
            })

            navigate('/')
        } catch (error) {
            console.error("Error al iniciar sesión:", error)
        }
    }

    return (
        <FlexCenter>
            <div className='bg-white p-8 rounded-lg shadow-lg w-full max-w-sm'>
                <h2 className="text-2xl font-bold text-center mb-6">Iniciar Sesion</h2>
                <form onSubmit={handleSubmit(onSubmit)}>

                    {inputsLogin.map(input => (
                        <InputComponent key={input.inputId} {...input} classNameSpanAlert={input.classNameSpanAlert} spanAlert={errors[input.inputName as keyof LoginFormData]?.message ?? ''} />
                    ))}

                    <button className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-300"
                        type="submit">Iniciar sesion</button>
                </form>
                <div className="text-center mt-4">
                    <NavLink className="text-gray-500 hover:text-blue-600 text-sm" to="/registro">Registrarse</NavLink>
                </div>
            </div>
        </FlexCenter>
    )
}

export default Login