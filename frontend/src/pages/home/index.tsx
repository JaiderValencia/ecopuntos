import { useEffect, useState } from 'react'
import Card from '../../components/card'
import Table from '../../components/table'
import Map from '../../components/map'
import { getEcopuntos } from '../../api/ecopuntos'
import type { Ecopunto } from '../../interfaces/ecopunto'
import { isOpen, materialsAcceptedJoined } from '../../utils/ecopunto'
import SpanState from '../../components/spanState'

function Inicio() {
    const [isLoading, setIsLoading] = useState(false)
    const [sucursales, setSucursales] = useState<Ecopunto[]>([])

    const getSucursales = async () => {
        setIsLoading(true)
        try {
            setIsLoading(true)
            const data = await getEcopuntos({ limite: 50 })
            setSucursales(data.ecopuntos)
        } catch (error) {
            console.error('Error al obtener sucursales:', error)
        } finally {
            setIsLoading(false)
        }
    }

    useEffect(() => {
        getSucursales()
    }, [])

    return (
        <div className='p-7 flex flex-col'>
            <section className='flex gap-10 flex-col lg:flex-row mb-8'>
                <Card className='h-[416px] lg:w-[557%]'>
                    {!isLoading && <Map className='h-full w-full' ecopuntos={sucursales} />}
                </Card>
                <Card className='flex flex-col'>
                    <h2 className="text-2xl md:text-3xl font-semibold tracking-tight mb-3">Sobre nosotros:</h2>
                    <p className="text-lg md:text-xl text-left my-auto leading-relaxed">
                        Somos una empresa comprometida con la sostenibilidad y el cuidado del medio ambiente, que busca transformar los hábitos de la comunidad mediante la correcta disposición de residuos reciclables.
                    </p>
                </Card>
            </section>

            <section>
                <h2 className='text-4xl font-bold mb-4'>Ecopuntos</h2>

                {isLoading && 'Cargando sucursales...'}
                {!sucursales.length && !isLoading && 'No hay sucursales disponibles, vuelve más tarde.'}

                {sucursales.length > 0 && (
                    <Table columns={['Nombre', 'Dirección', 'Materiales', 'Responsable', 'Estado']}>
                        {sucursales.map((sucursal, index) => (
                            <tr key={index} className="border-b">
                                <td className="px-4 py-2">{sucursal.nombre}</td>
                                <td className="px-4 py-2">{sucursal.ubicacion.direccion}</td>
                                <td className="px-4 py-2">{materialsAcceptedJoined(sucursal.materialesAceptados)}</td>
                                <td className="px-4 py-2">{sucursal.trabajador.nombre}</td>
                                <td className="px-4 py-2"><SpanState isOpen={isOpen(sucursal.horario)} /></td>
                            </tr>
                        ))}
                    </Table>
                )}
            </section>
        </div>
    )
}
export default Inicio