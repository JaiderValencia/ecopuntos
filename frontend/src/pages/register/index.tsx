import { useState } from 'react'
import Card from '../../components/card'
import FlexCenter from '../../components/flexCenter'
import InputComponent from '../../components/input/texts'
import type { InputsSteps } from '../../interfaces/register/inputsSteps'
import { NavLink } from 'react-router-dom'

const inputsRegister: InputsSteps = {
    stepOne: [{
        inputId: 'correo',
        inputName: 'correo',
        inputType: 'email',
        label: 'Correo:',
    },
    {
        inputId: 'nombre',
        inputName: 'nombre',
        inputType: 'text',
        label: 'Nombre:',
    }],
    stepTwo: [{
        inputId: 'apellido',
        inputName: 'apellido',
        inputType: 'text',
        label: 'Apellido:',
    },
    {
        inputId: 'direccion',
        inputName: 'direccion',
        inputType: 'text',
        label: 'Direccion:',
    }, {
        inputId: 'telefono',
        inputName: 'telefono',
        inputType: 'tel',
        label: 'Telefono celular:',
    },
    {
        inputId: 'documento',
        inputName: 'documento',
        inputType: 'text',
        label: 'identificacion:',
    }, {
        inputId: 'password',
        inputName: 'password',
        inputType: 'password',
        label: 'Contraseña:',
    }]
}

function RegisterPage() {
    const [step, setStep] = useState(1)

    const nextStep = () => {
        setStep(prevStep => prevStep + 1)
    }

    const prevStep = () => {
        setStep(prevStep => prevStep - 1)
    }

    return (
        <FlexCenter>
            <Card className='bg-white p-8 rounded-lg shadow-lg w-full max-w-sm'>
                <h2 className="text-2xl font-bold text-center mb-6">Registrarse</h2>
                <form>

                    {/* primera etapa */}
                    <div className={`${step === 1 ? 'block' : 'hidden'}`}>

                        {inputsRegister.stepOne.map(input => (
                            <InputComponent key={input.inputId} {...input} />
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
                            <InputComponent divClassName={index == inputsRegister.stepTwo.length - 1 ? 'col-span-2' : ''} key={input.inputId} {...input} />
                        ))}
                    </div>

                    <div className={`${step === 2 ? 'block' : 'hidden'} flex gap-3`}>
                        <button className="w-full bg-rose-600 text-white py-2 rounded-md hover:bg-rose-700 transition duration-300"
                            type="button" onClick={prevStep}>Anterior</button>

                        <button className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition duration-300"
                            type="button">Registrarse</button>
                    </div>
                </form>
            </Card>
        </FlexCenter>
    )
}

export default RegisterPage