import { createElement, useState } from 'react'
import { defaultUserStatus, UserContext } from './user'

export function UserProvider({ children }: { children: React.ReactNode }) {
    const [userStatus, setUserStatus] = useState(defaultUserStatus)

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