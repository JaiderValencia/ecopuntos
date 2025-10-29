import { createElement, useEffect, useState } from 'react'
import { defaultUserStatus, UserContext } from './user'
import hasSession from '../../utils/hasSession'

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