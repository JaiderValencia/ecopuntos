import { useEffect, useState } from 'react'
import Table from '../../components/table'
import type { Ecopunto } from '../../interfaces/ecopunto'
import { getEcopuntos } from '../../api/ecopuntos'
import { materialsAcceptedJoined } from '../../utils/ecopunto'
import { NavLink } from 'react-router-dom'

function ListEcopuntos() {
    const [isLoading, setIsLoading] = useState(false)
    const [sucursales, setSucursales] = useState<Ecopunto[]>([])

    const getSucursales = async () => {
        setIsLoading(true)
        try {
            setIsLoading(true)
            const data = await getEcopuntos({ limite: 50 })
            setSucursales(data.ecopuntos)
        } catch (error) {
            console.error('Error fetching sucursales:', error)
        } finally {
            setIsLoading(false)
        }
    }

    useEffect(() => {
        getSucursales()
    }, [])

    return (
        <>

            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-6">
                <h2 className="text-2xl font-bold text-text-light dark:text-text-dark mb-4 sm:mb-0">Ecopuntos Registrados
                </h2>
                <div className="flex space-x-2">
                    <NavLink to={`/ecopuntos/editar/`} className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded">
                        Actualizar Ecopunto
                    </NavLink>
                    <NavLink to="/ecopuntos/registrar" className="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded">
                        Registrar Ecopunto
                    </NavLink>
                </div>
            </div>

            {isLoading && 'Cargando sucursales...'}

            {!sucursales.length && !isLoading && 'No hay sucursales disponibles, vuelve más tarde.'}

            {sucursales.length > 0 && (
                <section>
                    <Table columns={['ID', 'Direccion', 'Responsable', 'Materiales']} >
                        {sucursales.map((sucursal, index) => (
                            <tr className={`border-b cursor-pointer hover:bg-gray-100`} key={index}>
                                <td className='px-4 py-2'>{`${sucursal.id}`}</td>
                                <td className='px-4 py-2'>{sucursal.ubicacion.direccion}</td>
                                <td className='px-4 py-2'>{sucursal.trabajador.nombre}</td>
                                <td className='px-4 py-2'>{materialsAcceptedJoined(sucursal.materialesAceptados)}</td>
                            </tr>
                        ))}
                    </Table>
                </section>
            )}
        </>
    )
}

export default ListEcopuntos