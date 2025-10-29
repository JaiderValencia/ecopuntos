import type { UseFormRegisterReturn } from 'react-hook-form'

export interface SelectProps {
    label: string,
    inputId: string,
    inputName: string,
    divClassName?: string,
    selectClassName?: string,
    register?: UseFormRegisterReturn,
    spanAlert?: string,
    classNameSpanAlert?: string,
    children?: React.ReactNode,
    multiple?: boolean,
    ref?: React.Ref<HTMLSelectElement>
}

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
    classNameSpanAlert?: string,
    value?: string | number
}