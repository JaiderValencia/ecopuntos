import { createContext, createElement, useContext, useState } from 'react'

type UserContextValue = {
    userStatus: {
        isLogged: boolean,
        userId: string
        userName: string
        userEmail: string
        userPhone: string
    },
    setUserStatus: (userStatus: UserContextValue['userStatus']) => void
}

export const UserContext = createContext<UserContextValue>({ userStatus: { isLogged: false, userId: '', userName: '', userEmail: '', userPhone: '' }, setUserStatus: () => {} })

export function UserProvider({ children }: { children: React.ReactNode }) {
    const [userStatus, setUserStatus] = useState({ isLogged: false, userId: '', userName: '', userEmail: '', userPhone: '' })

    return createElement(
        UserContext.Provider,
        {
            value: {
                userStatus,
                setUserStatus
            }
        },
        children
    )
}

export const useUserContext = () => (useContext(UserContext))