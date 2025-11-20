import { NavLink } from 'react-router-dom'
import { useUserContext } from '../../contex/user/user'
import { useState } from 'react'

function Header() {
    const { userStatus } = useUserContext()
    const [isDropdownOpen, setIsDropdownOpen] = useState(false)

    const opcionesTrabajador = () => {
        if (userStatus.userRole !== 'Empleado') return null

        return (
            <div className="relative mx-4">
                <button
                    onClick={() => setIsDropdownOpen(!isDropdownOpen)}
                    className="text-white flex items-center focus:outline-none"
                >
                    Entregas
                    <span className="material-icons ml-1 text-sm">
                        {isDropdownOpen ? 'expand_less' : 'expand_more'}
                    </span>
                </button>
                {isDropdownOpen && (
                    <div className="absolute top-full left-0 mt-1 bg-white rounded-md shadow-lg min-w-[200px]">
                        <NavLink
                            to="/reportes/registrar"
                            className="block px-4 py-2 text-gray-800 hover:bg-green-100 rounded-md"
                            onClick={() => setIsDropdownOpen(false)}
                        >
                            Registrar entrega
                        </NavLink>

                        <NavLink
                            to="/reportes/mis-reportes"
                            className="block px-4 py-2 text-gray-800 hover:bg-green-100 rounded-md"
                            onClick={() => setIsDropdownOpen(false)}
                        >
                            Lista de entregas
                        </NavLink>
                    </div>
                )}
            </div>
        )
    }

    const opcionesCliente = () => {
        if (userStatus.userRole !== 'Cliente') return null

        return (<>
            <NavLink className='text-white mx-4' to="/reportes/mis-reportes">Mis reportes</NavLink>
        </>)
    }

    const opcionesAdministrador = () => {
        if (userStatus.userRole !== 'Admin') return null

        return (<>
            <NavLink className="text-white mx-4" to="/ecopuntos">ECO puntos</NavLink>
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
                            <NavLink className="text-white mx-4" to="/">Mapa</NavLink>
                            {opcionesAdministrador()}
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
