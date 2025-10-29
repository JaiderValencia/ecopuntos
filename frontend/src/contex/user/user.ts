import { createContext, useContext } from 'react'

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

export const defaultUserStatus: UserContextValue['userStatus'] = {
    isLogged: false,
    userId: '',
    userName: '',
    userEmail: '',
    userPhone: '',
    userRole: ''
}

export const UserContext = createContext<UserContextValue>({ userStatus: defaultUserStatus, setUserStatus: () => { } })

export const useUserContext = () => (useContext(UserContext))