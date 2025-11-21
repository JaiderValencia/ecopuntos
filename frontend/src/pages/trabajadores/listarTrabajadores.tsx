import { useEffect, useState } from 'react'
import { NavLink, useNavigate } from 'react-router-dom'
import Table from '../../components/table'
import type { Trabajador } from '../../interfaces/trabajador'
import { obtenerTrabajadores } from '../../api/trabajador'

function ListarTrabajadores() {
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)
    const [trabajadores, setTrabajadores] = useState<Trabajador[]>([])
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        const getTrabajadores = async () => {
            setIsLoading(true)
            setError(null)

            try {
                const data = await obtenerTrabajadores(50)
                setTrabajadores(data)
            } catch (error) {
                console.error('Error fetching trabajadores:', error)
                setError('Error al cargar los trabajadores')
            } finally {
                setIsLoading(false)
            }
        }

        getTrabajadores()
    }, [])

    return (
        <>
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-6">
                <h2 className="text-2xl font-bold text-text-light dark:text-text-dark mb-4 sm:mb-0">
                    Trabajadores Registrados
                </h2>
                <NavLink 
                    to="/trabajadores/crear" 
                    className="bg-green-600 hover:bg-green-700 text-white font-bold py-2 px-4 rounded"
                >
                    Registrar Trabajador
                </NavLink>
            </div>

            {isLoading && (
                <div className="text-center py-8">
                    <p className="text-gray-600">Cargando trabajadores...</p>
                </div>
            )}

            {error && (
                <div className="text-center py-8">
                    <p className="text-red-600">{error}</p>
                </div>
            )}

            {!trabajadores.length && !isLoading && !error && (
                <div className="text-center py-8">
                    <p className="text-gray-600">No hay trabajadores registrados.</p>
                </div>
            )}

            {trabajadores.length > 0 && !isLoading && (
                <section>
                    <Table columns={['Código de Empleado', 'Nombre', 'Cédula', 'Teléfono']}>
                        {trabajadores.map((trabajador) => (
                            <tr 
                                className="border-b cursor-pointer hover:bg-gray-100" 
                                key={trabajador.Id}
                                onClick={() => navigate(`/trabajadores/editar/${trabajador.Id}`)}
                            >
                                <td className="px-4 py-2">{trabajador.CodigoDeEmpleado}</td>
                                <td className="px-4 py-2">{trabajador.Nombre}</td>
                                <td className="px-4 py-2">{trabajador.Cedula}</td>
                                <td className="px-4 py-2">{trabajador.Telefono}</td>
                            </tr>
                        ))}
                    </Table>
                </section>
            )}
        </>
    )
}

export default ListarTrabajadores
