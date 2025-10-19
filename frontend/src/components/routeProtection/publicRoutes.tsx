import { Outlet, Navigate } from 'react-router-dom'
import { useUserContext } from '../../contex/user'

function PublicRoute() {
    const { isLogged } = useUserContext()

    if (!isLogged) {
        return <>{<Outlet />}</>
    }

    return <Navigate to="/" />
}

export default PublicRoute