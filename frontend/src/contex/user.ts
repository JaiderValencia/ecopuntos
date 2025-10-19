import { createContext, createElement, useContext, useState } from 'react'

type UserContextValue = {
    isLogged: boolean,
    setIsLogged?: (isLogged: boolean) => void
}

export const UserContext = createContext<UserContextValue>({ isLogged: false })

export function UserProvider({ children }: { children: React.ReactNode }) {
    const [isLogged, setIsLogged] = useState(false)

    return createElement(
        UserContext.Provider,
        {
            value: {
                isLogged,
                setIsLogged
            }
        },
        children
    )
}

export const useUserContext = () => (useContext(UserContext))