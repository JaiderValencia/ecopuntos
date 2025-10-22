import { Outlet, Navigate } from 'react-router-dom'
import { useUserContext } from '../../contex/user'

function PublicRoute() {
    const { userStatus: { isLogged } } = useUserContext()

    if (!isLogged) {
        return <>{<Outlet />}</>
    }

    return <Navigate to="/" />
}

export default PublicRoute