import { createContext, useContext } from 'react'
import hasSession from '../../utils/hasSession'

export type UserContextValue = {
    userStatus: {
        isLogged: boolean,
        userId: number
        userName: string
        userEmail: string
        userPhone: string,
        userRole: string
    },
    setUserStatus: (userStatus: UserContextValue['userStatus']) => void
}

export const defaultUserStatus: UserContextValue['userStatus'] = {
    ...hasSession() || {
        isLogged: false,
        userId: 0,
        userName: '',
        userEmail: '',
        userPhone: '',
        userRole: ''
    }
}

export const UserContext = createContext<UserContextValue>({ userStatus: defaultUserStatus, setUserStatus: () => { } })

export const useUserContext = () => (useContext(UserContext))