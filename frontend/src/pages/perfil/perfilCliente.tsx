import { useEffect, useState } from 'react'
import { useUserContext } from '../../contex/user/user'
import { obtenerClientePorCorreo } from '../../api/clientes'
import type { ClienteDetallado } from '../../interfaces/cliente'
import Button from '../../components/button'
import { useLogout } from '../../utils/usuario'

const PerfilCliente = () => {
    const { userStatus } = useUserContext()
    const [cliente, setCliente] = useState<ClienteDetallado | null>(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)    

    useEffect(() => {
        const cargarDatosCliente = async () => {
            try {
                setLoading(true)
                const datos = await obtenerClientePorCorreo(userStatus.userEmail)                
                
                setCliente(datos)
            } catch (err) {
                setError('Error al cargar los datos del cliente')
                console.error(err)
            } finally {
                setLoading(false)
            }
        }

        if (userStatus.userEmail) {
            cargarDatosCliente()
        }
    }, [userStatus.userEmail])

    const cerrarSesion = useLogout()

    if (loading) {
        return (
            <div className="flex justify-center items-center min-h-[70vh]">
                <p className="text-xl text-gray-600">Cargando...</p>
            </div>
        )
    }

    if (error || !cliente) {
        return (
            <div className="flex justify-center items-center min-h-[70vh]">
                <p className="text-xl text-red-600">{error || 'No se encontraron datos'}</p>
            </div>
        )
    }

    return (
        <div className="max-w-7xl mx-auto py-8 px-4">
            <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
                <div className="grid md:grid-cols-[400px,1fr] gap-0">
                    {/* Columna izquierda - Información del usuario */}
                    <div className="bg-gray-50 p-8 flex flex-col items-center justify-center border-r border-gray-200">
                        <div className="w-64 h-64 rounded-full bg-gradient-to-br from-green-400 to-green-600 flex items-center justify-center mb-6 shadow-xl">
                            <div className="w-56 h-56 rounded-full bg-white flex items-center justify-center">
                                <svg className="w-32 h-32 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
                                </svg>
                            </div>
                        </div>

                        <h1 className="text-3xl font-bold text-gray-800 mb-2 text-center">
                            {cliente.Nombre}
                        </h1>
                        
                        <p className="text-gray-600 text-center mb-8">
                            Cliente de ECO Medellin
                        </p>

                        <Button 
                            className="bg-red-500 hover:bg-red-600 text-white w-full max-w-xs"
                            onClick={cerrarSesion}
                        >
                            Cerrar sesión
                        </Button>
                    </div>

                    {/* Columna derecha - Información detallada */}
                    <div className="p-8 lg:p-12">
                        {/* Información Personal */}
                        <div className="mb-10">
                            <h2 className="text-2xl font-bold text-gray-800 mb-6">
                                Informacion Personal
                            </h2>
                            <div className="grid gap-4">
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Documento:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.Cedula}</span>
                                </div>
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Correo electronico:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.Correo}</span>
                                </div>
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Telefono:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.Telefono}</span>
                                </div>
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Direccion:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.Direccion}</span>
                                </div>
                            </div>
                        </div>

                        {/* Información de Participación */}
                        <div>
                            <h2 className="text-2xl font-bold text-gray-800 mb-6">
                                Informacion de Participacion
                            </h2>
                            <div className="grid gap-4">
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Total entregas realizadas:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.TotalEntregas}</span>
                                </div>
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Total material entregado:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.PesoTotalEntregado} Kg</span>
                                </div>
                                <div className="flex justify-between items-center py-3 border-b border-gray-200">
                                    <span className="text-gray-600 font-medium">Puntaje acumulado:</span>
                                    <span className="text-gray-800 font-semibold">{cliente.EcoPuntos}</span>
                                </div>
                                <div className="flex justify-between items-center py-3">
                                    <span className="text-gray-600 font-medium">Ultima entrega:</span>
                                    <span className="text-gray-800 font-semibold">
                                        {cliente.UltimoEcopunto || 'Sin entregas'}
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default PerfilCliente
