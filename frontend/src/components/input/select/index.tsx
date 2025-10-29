import type { SelectProps } from '../../../interfaces/Input'

function SelectComponent({ label, inputId: id, inputName, divClassName, selectClassName, register, spanAlert, classNameSpanAlert, children, multiple }: SelectProps) {
    return (
        <div className={`mb-4 ${divClassName}`}>
            <label className="block text-gray-700 text-sm font-medium mb-2" htmlFor={id}>{label}</label>
            <select multiple={multiple} className={`w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 ${selectClassName}`}
                id={id}
                name={inputName}
                {...register}                
            >
                <option value="" disabled>Seleccione una opción</option>
                {children}
            </select>

            {spanAlert && <span className={`${classNameSpanAlert}`}>{spanAlert}</span>}
        </div>
    )
}

export default SelectComponent