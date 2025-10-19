import { Outlet, Navigate } from 'react-router-dom'
import { useUserContext } from '../../contex/user'

function PrivateRoute() {
    const { isLogged } = useUserContext()
    
    if (!isLogged) {
        return <Navigate to="/login" />
    }

    return <>{<Outlet />}</>
}

export default PrivateRoute