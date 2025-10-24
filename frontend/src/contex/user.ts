import { createContext, createElement, useContext, useEffect, useState } from 'react'
import hasSession from '../utils/hasSession'

export type UserContextValue = {
    userStatus: {
        isLogged: boolean,
        userId: string
        userName: string
        userEmail: string
        userPhone: string,
        userRole: string
    },
    setUserStatus: (userStatus: UserContextValue['userStatus']) => void
}

const defaultUserStatus: UserContextValue['userStatus'] = {
    isLogged: false,
    userId: '',
    userName: '',
    userEmail: '',
    userPhone: '',
    userRole: ''
}

export const UserContext = createContext<UserContextValue>({ userStatus: defaultUserStatus, setUserStatus: () => { } })

export function UserProvider({ children }: { children: React.ReactNode }) {
    const [userStatus, setUserStatus] = useState(defaultUserStatus)

    useEffect(() => {
        setUserStatus(hasSession() || defaultUserStatus)
    }, [])

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