import { useEffect, useState } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { obtenerInformacionReportePorId } from '../../api/reporte'
import type { ReporteDetalle } from '../../interfaces/reporte'

function DetalleReporte() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)
    const [reporte, setReporte] = useState<ReporteDetalle | null>(null)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        const getReporteDetalle = async () => {
            if (!id) {
                setError('ID de reporte no válido')
                return
            }

            setIsLoading(true)
            setError(null)

            try {
                const data = await obtenerInformacionReportePorId(parseInt(id))
                setReporte(data)
            } catch (error) {
                console.error('Error fetching reporte detalle:', error)
                setError('Error al cargar el detalle del reporte')
            } finally {
                setIsLoading(false)
            }
        }

        getReporteDetalle()
    }, [id])

    if (isLoading) {
        return (
            <div className="text-center py-8">
                <p className="text-gray-600">Cargando detalle del reporte...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="text-center py-8">
                <p className="text-red-600">{error}</p>
                <button
                    onClick={() => navigate('/mis-reportes')}
                    className="mt-4 bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded"
                >
                    Volver a mis reportes
                </button>
            </div>
        )
    }

    if (!reporte) {
        return (
            <div className="text-center py-8">
                <p className="text-gray-600">No se encontró el reporte</p>
            </div>
        )
    }

    return (
        <div className="max-w-7xl mx-auto p-4">
            {/* Header */}
            <h1 className="text-3xl font-bold mb-8">Reporte de entrega.</h1>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                {/* Columna izquierda - Tabla de materiales */}
                <div className="bg-white rounded-lg border-2 border-gray-200 p-6">
                    <table className="w-full">
                        <thead>
                            <tr className="border-b-2 border-gray-300">
                                <th className="text-left py-3 px-2 font-semibold">Material</th>
                                <th className="text-left py-3 px-2 font-semibold">Cantidad</th>
                                <th className="text-left py-3 px-2 font-semibold">Estado</th>
                                <th className="text-left py-3 px-2 font-semibold">Puntos</th>
                            </tr>
                        </thead>
                        <tbody>
                            {reporte.materialesEntrega?.map((material, index) => (
                                <tr key={index} className="border-b border-gray-200">
                                    <td className="py-3 px-2">{material.NombreMaterial}</td>
                                    <td className="py-3 px-2">{material.Peso} Kg</td>
                                    <td className="py-3 px-2">{material.Estado ? 'Aceptado' : 'Rechazado'}</td>
                                    <td className="py-3 px-2">{material.Puntos}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

                {/* Columna derecha - Datos y resumen */}
                <div className="space-y-6">
                    {/* Datos Cliente y Empleado */}
                    <div className="bg-white rounded-lg border-2 border-gray-200 p-6">
                        <div className="grid grid-cols-2 gap-8">
                            {/* Datos Cliente */}
                            <div>
                                <h3 className="font-bold text-lg mb-4">Datos Cliente</h3>
                                <div className="space-y-2 text-sm">
                                    <p>{reporte.cliente.nombre}</p>
                                    <p>{reporte.cliente.correo}</p>
                                    <p>Cel: {reporte.cliente.telefono}</p>
                                    <p>Doc: {reporte.cliente.cedula}</p>
                                </div>
                            </div>

                            {/* Datos Empleado */}
                            <div>
                                <h3 className="font-bold text-lg mb-4">Datos Empleado</h3>
                                <div className="space-y-2 text-sm">
                                    <p>{reporte.trabajador.nombre}</p>
                                    <p>{reporte.trabajador.correo}</p>
                                    <p>Cel: {reporte.trabajador.telefono}</p>
                                    <p>Doc: {reporte.trabajador.cedula}</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Top 3 materiales */}
                    {reporte.top3 && Object.keys(reporte.top3).length > 0 && (
                        <div className="bg-green-50 rounded-lg border-2 border-gray-200 p-6">
                            <h3 className="font-bold text-lg mb-4 text-center">Top 3 materiales entregados</h3>
                            <div className="flex justify-center gap-8 text-sm">
                                {Object.entries(reporte.top3).map(([key, value], index) => (
                                    <div key={key}>
                                        <span className="font-semibold">{index + 1}. {value}</span>
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}

                    {/* Resumen de la entrega */}
                    {reporte.totales && Object.keys(reporte.totales).length > 0 && (
                        <div className="bg-white rounded-lg border-2 border-gray-200 p-6">
                            <h3 className="font-bold text-lg mb-4 text-center">Resumen de la entrega</h3>
                            <div className="grid grid-cols-4 gap-4 text-center">
                                {Object.entries(reporte.totales).map(([key, value]) => (
                                    <div key={key}>
                                        <p className="text-sm mb-1 capitalize">
                                            {key.replace(/([A-Z])/g, ' $1').trim()}
                                        </p>
                                        <p className="text-2xl font-bold">
                                            {key.toLowerCase().includes('puntos') ? value : `${value} Kg`}
                                        </p>
                                    </div>
                                ))}
                            </div>
                            <div className="flex justify-center mt-6">
                                <button className="bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 px-6 rounded">
                                    Imprimir Reporte
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    )
}

export default DetalleReporte