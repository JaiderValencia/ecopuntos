import { Outlet, Navigate } from 'react-router-dom'
import { useUserContext } from '../../contex/user'

function PrivateRoute() {
    const { userStatus: { isLogged } } = useUserContext()

    // Si el usuario no está autenticado, redirigir al login
    if (!isLogged) {
        return <Navigate to="/login" />
    }

    return <>{<Outlet />}</>
}

export default PrivateRoute