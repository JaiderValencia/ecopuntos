import type { InputProps } from '../../../interfaces/inputComponent/InputProps'

function InputComponent({ label, inputType: type, inputPlaceholder: placeholder, inputId: id, inputName, divClassName, inputClassName, register, spanAlert, classNameSpanAlert }: InputProps) {
    return (
        <div className={`mb-4 ${divClassName}`}>
            <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor={id}>{label}</label>
            <input
                className={`w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${inputClassName}`}
                id={id} name={inputName} placeholder={placeholder} type={type} {...register} />

            {spanAlert && <span className={`${classNameSpanAlert}`}>{spanAlert}</span>}
        </div>
    )
}

export default InputComponent