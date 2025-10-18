import InputComponent from '../../components/input'
import type { InputProps } from '../../interfaces/login/InputProps'

const inputsLogin: InputProps[] = [{
    label: 'Correo Electronico',
    inputType: 'email',
    inputId: 'email',
    inputName: 'email',
    inputPlaceholder: 'Correo Electronico'
},
{
    label: 'Contraseña',
    inputType: 'password',
    inputId: 'password',
    inputName: 'password',
    inputPlaceholder: 'Contraseña'
}
]

function Login() {
    return (
        <div className='bg-white p-8 rounded-lg shadow-lg w-full max-w-sm'>
            <h2 className="text-2xl font-bold text-center mb-6">Iniciar Sesion</h2>
            <form>

                {inputsLogin.map(input => (
                    <InputComponent key={input.inputId} {...input} />
                ))}

                <button className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-300"
                    type="button">Iniciar sesion</button>
            </form>
            <div className="text-center mt-4">
                <a className="text-gray-500 hover:text-blue-600 text-sm" href="#">Registrarse</a>
            </div>
        </div>
    )
}

export default Login