import type { UserContextValue } from '../contex/user'

function hasSession(): UserContextValue['userStatus'] | undefined {
    const storedSession = sessionStorage.getItem('userSession')

    if (storedSession) {
        return {
            isLogged: true,
            userId: JSON.parse(storedSession).datosUsuario.id,
            userName: JSON.parse(storedSession).datosUsuario.nombre,
            userEmail: JSON.parse(storedSession).datosUsuario.correo,
            userPhone: JSON.parse(storedSession).datosUsuario.telefono,
            userRole: JSON.parse(storedSession).datosUsuario.rol
        }
    }

}

export default hasSession