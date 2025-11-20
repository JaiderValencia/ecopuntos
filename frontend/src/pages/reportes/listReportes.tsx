import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import Table from '../../components/table'
import type { ReporteAllDto } from '../../interfaces/reporte'
import { obtenerReportesPorIdCliente, obtenerReportesPorIdTrabajador } from '../../api/reporte'
import { useUserContext } from '../../contex/user/user'

function ListReportes() {
    const { userStatus } = useUserContext()
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)
    const [reportes, setReportes] = useState<ReporteAllDto[]>([])
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        const obtenerReportesPorRol = {
            'Cliente': obtenerReportesPorIdCliente(userStatus.userId!),
            'Empleado': obtenerReportesPorIdTrabajador(userStatus.userId!)
        }

        const getReportes = async () => {
            if (!userStatus.userId) {
                setError('Usuario no identificado')
                return
            }

            setIsLoading(true)
            setError(null)

            try {
                const data = await obtenerReportesPorRol[userStatus.userRole as 'Cliente' | 'Empleado']
                setReportes(data)
            } catch (error) {
                console.error('Error fetching reportes:', error)
                setError('Error al cargar los reportes')
            } finally {
                setIsLoading(false)
            }
        }

        getReportes()
    }, [userStatus.userId, userStatus.userRole])

    const formatDate = (dateString: string) => {
        const date = new Date(dateString)
        return date.toLocaleDateString('es-ES', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        })
    }

    return (
        <>
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-6">
                <h2 className="text-2xl font-bold text-text-light dark:text-text-dark mb-4 sm:mb-0">
                    Mis Reportes
                </h2>
            </div>

            {isLoading && (
                <div className="text-center py-8">
                    <p className="text-gray-600">Cargando reportes...</p>
                </div>
            )}

            {error && (
                <div className="text-center py-8">
                    <p className="text-red-600">{error}</p>
                </div>
            )}

            {!reportes.length && !isLoading && !error && (
                <div className="text-center py-8">
                    <p className="text-gray-600">No tienes reportes registrados aún.</p>
                </div>
            )}

            {reportes.length > 0 && !isLoading && (
                <section>
                    <Table columns={['ID Reporte', 'Fecha de Creación', 'Ecopunto', 'Responsable']}>
                        {reportes.map((reporte) => (
                            <tr
                                className="border-b cursor-pointer hover:bg-gray-100"
                                key={reporte.idReporte}
                                onClick={() => navigate(`/reportes/mis-reportes/${reporte.idReporte}`)}
                            >
                                <td className="px-4 py-2">{reporte.idReporte}</td>
                                <td className="px-4 py-2">{formatDate(reporte.fechaCreacion)}</td>
                                <td className="px-4 py-2">{reporte.nombreEcopunto}</td>
                                <td className="px-4 py-2">{reporte.responsable || userStatus.userName}</td>
                            </tr>
                        ))}
                    </Table>
                </section>
            )}
        </>
    )
}

export default ListReportes