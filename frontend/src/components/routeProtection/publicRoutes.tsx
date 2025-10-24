import { Outlet, Navigate } from 'react-router-dom'
import { useUserContext } from '../../contex/user'

function PublicRoute() {
    const { userStatus: { isLogged } } = useUserContext()

    // Si el usuario no está autenticado, permite el acceso
    if (!isLogged) {
        return <>{<Outlet />}</>
    }

    return <Navigate to="/" />
}

export default PublicRoute