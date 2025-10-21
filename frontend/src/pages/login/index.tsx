import { NavLink } from 'react-router-dom'
import FlexCenter from '../../components/flexCenter'
import InputComponent from '../../components/input/texts'
import type { InputProps } from '../../interfaces/inputComponent/InputProps'

const inputsLogin: InputProps[] = [{
    label: 'Correo:',
    inputType: 'email',
    inputId: 'email',
    inputName: 'email',
},
{
    label: 'Contraseña:',
    inputType: 'password',
    inputId: 'password',
    inputName: 'password',
}
]

function Login() {
    return (
        <FlexCenter>
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
                    <NavLink className="text-gray-500 hover:text-blue-600 text-sm" to="/registro">Registrarse</NavLink>
                </div>
            </div>
        </FlexCenter>
    )
}

export default Login