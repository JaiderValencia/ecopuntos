import { useUserContext, defaultUserStatus } from '../contex/user/user'
import { useNavigate } from 'react-router-dom'

export const useLogout = () => {
    const { setUserStatus } = useUserContext()
    const navigate = useNavigate()

    const logout = async () => {
        sessionStorage.removeItem('userSession')
        setUserStatus(defaultUserStatus)
        await navigate('/login')
    }

    return logout
}