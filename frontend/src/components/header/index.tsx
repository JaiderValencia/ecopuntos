import { NavLink } from 'react-router-dom'
import { useUserContext } from '../../contex/user/user'

function Header() {
    const { userStatus } = useUserContext()

    const opcionesTrabajador = () => {
        if (userStatus.userRole !== 'Empleado') return null

        return (<>
            <NavLink className='text-white mx-4' to="/reportes/registrar">Registrar entrega</NavLink>
        </>)
    }

    const opcionesCliente = () => {
        if (userStatus.userRole !== 'Cliente') return null

        return (<>
            <NavLink className='text-white mx-4' to="/mis-reportes">Mis reportes</NavLink>
        </>)
    }

    return (
        <header className="bg-green-600 shadow-md fixed top-0 left-0 right-0 z-10">
            <div className="container mx-auto px-6 py-3 flex justify-between items-center">
                <NavLink to="/" className="flex items-center">
                    <img src="/logo.svg" alt="logo" className='h-8 w-8' />
                    <h1 className="text-xl font-bold text-white ml-2 hidden md:block">ECO Medellin</h1>
                </NavLink>
                <nav className="flex items-center">
                    {userStatus.isLogged && (
                        <>
                            <NavLink className="text-white mx-4" to="/ecopuntos">ECO puntos</NavLink>
                            <NavLink className="text-white mx-4" to="#">Mapa</NavLink>
                            {opcionesTrabajador()}
                            {opcionesCliente()}
                        </>
                    )}
                    <NavLink className="text-white mx-4 flex items-center" to={userStatus.isLogged ? '/perfil' : '/login'}>
                        {userStatus.isLogged ? 'Perfil' : 'Iniciar Sesion'}
                        <span className="material-icons ml-2">account_circle</span>
                    </NavLink>
                </nav>
            </div>
        </header>
    )
}

export default Header
