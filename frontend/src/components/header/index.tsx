import { NavLink } from 'react-router-dom'

function Header() {
    return (
        <header className="bg-green-600 shadow-md fixed top-0 left-0 right-0 z-10">
            <div className="container mx-auto px-6 py-3 flex justify-between items-center">
                <div className="flex items-center">
                    <img src="logo.svg" alt="logo" className='h-8 w-8'/>
                    <h1 className="text-xl font-bold text-white ml-2 hidden md:block">ECO Medellin</h1>
                </div>
                <nav className="flex items-center">
                    <a className="text-white mx-4" href="#">Mapa</a>
                    <NavLink className="text-white mx-4 flex items-center" to="/login">
                        Iniciar sesion
                        <span className="material-icons ml-2">account_circle</span>
                    </NavLink>
                </nav>
            </div>
        </header>
    )
}

export default Header
