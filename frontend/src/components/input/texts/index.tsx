import type { InputProps } from '../../../interfaces/login/InputProps'

function InputComponent({ label, inputType: type, inputPlaceholder: placeholder, inputId: id, inputName: name }: InputProps) {
    return (
        <div className="mb-4">
            <label className="block text-gray-700 text-sm font-medium mb-2 sr-only" htmlFor={id}>{label}</label>
            <input
                className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                id={id} name={name} placeholder={placeholder} type={type} />
        </div>
    )
}

export default InputComponent