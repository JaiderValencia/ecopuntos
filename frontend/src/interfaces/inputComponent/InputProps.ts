import type { UseFormRegisterReturn } from 'react-hook-form'

export interface InputProps {
    label: string
    inputType: string
    inputPlaceholder?: string
    inputId: string
    inputName: string,
    divClassName?: string,
    inputClassName?: string,
    register?: UseFormRegisterReturn,
    spanAlert?: string,
    classNameSpanAlert?: string
}