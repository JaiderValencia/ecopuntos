import SelectComponent from '../../components/input/select'
import InputComponent from '../../components/input/texts'
import Table from '../../components/table'
import Button from '../../components/button'
import { useEffect, useState } from 'react'
import { getMateriales } from '../../api/materiales'
import { getEcopuntos } from '../../api/ecopuntos'
import type { Material, MaterialEntrega } from '../../interfaces/materiales'
import type { Ecopunto } from '../../interfaces/ecopunto'
import { useForm } from 'react-hook-form'
import type { ReporteForm } from '../../interfaces/reporte'
import { registrarReporte } from '../../api/reporte'
import { useUserContext } from '../../contex/user/user'
import { useNavigate } from 'react-router-dom'
import { validarReporteForm } from '../../utils/reporte'

function RegistrarReporte() {
    const { userStatus } = useUserContext()
    const { register, handleSubmit, watch, setValue, formState: { errors }, setError } = useForm<ReporteForm>()
    const navigate = useNavigate()

    const [ecoPuntos, setEcoPuntos] = useState<Ecopunto[]>([])
    const [materiales, setMateriales] = useState<Material[]>([])
    const [aceptados, setAceptados] = useState<MaterialEntrega[]>([])
    const [rechazados, setRechazados] = useState<MaterialEntrega[]>([])

    const [totalEntrega, setTotalEntrega] = useState<number>(0)
    const [totalAceptado, setTotalAceptado] = useState<number>(0)
    const [totalRechazado, setTotalRechazado] = useState<number>(0)

    const fetchMateriales = async () => {
        try {
            const { materiales } = await getMateriales({ limite: 100 })
            setMateriales(materiales)
        } catch (error) {
            console.error('Error fetching materiales:', error)
        }
    }

    const getSucursales = async () => {
        try {
            const data = await getEcopuntos({ limite: 50 })
            setEcoPuntos(data.ecopuntos)
        } catch (error) {
            console.error('Error fetching sucursales:', error)
        }
    }

    useEffect(() => {
        fetchMateriales()
        getSucursales()
    }, [])

    useEffect(() => {
        setValue('materialesEntrega', 0)
    }, [setValue])

    const handleAgregarMaterial = () => {
        const materialId = watch('materialesEntrega')

        if (!materialId) return

        const nombre = materiales.find((mat) => mat.Id == materialId)?.Nombre || 'N/A'
        const cantidad = parseInt(watch('cantidad').toString()) || 0
        const estado = watch('estado')
        const puntos = estado ? 100 * cantidad : 0

        setValue('materialesEntrega', 0)
        setValue('cantidad', 0)
        setValue('estado', false)

        setTotalEntrega(totalEntrega + cantidad)

        if (!estado) {
            setRechazados([...rechazados, { IdMaterial: materialId, nombre, peso: cantidad, puntos, estado }])
            setTotalRechazado(totalRechazado + cantidad)
            return
        }

        setAceptados([...aceptados, { IdMaterial: materialId, nombre, peso: cantidad, puntos, estado }])
        setTotalAceptado(totalAceptado + cantidad)
    }

    const handleMaterialesEntregaError = (message: string) => {
        setError('materialesEntrega', { type: 'required', message })
    }

    const onSubmit = async (data: ReporteForm) => {
        if (aceptados.length + rechazados.length === 0) {
            handleMaterialesEntregaError('Debe agregar al menos un material a la entrega.')

            return
        }

        try {
            await registrarReporte({
                cedulaCliente: data.cedulaCliente,
                idTrabajador: userStatus.userId,
                idEcoPunto: data.idEcoPunto,
                materialesEntrega: [...aceptados, ...rechazados]
            })
        } catch (error) {
            alert(`Error al registrar el reporte: ${error}`)
        } finally {
            alert('Reporte registrado con exito')
            navigate('/')
        }
    }

    const handleCancelar = () => {
        navigate('/')
    }

    return (
        <>
            <div className='flex gap-2'>
                <svg width="44" height="44" viewBox="0 0 88 88" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M40.8255 17.5979L41.2212 18.2837L38.2261 23.4716L31.9385 34.3614L21.0479 28.0739L27.3354 17.184L33.6229 6.29317L33.9038 5.80591L40.1693 16.4598L40.8255 17.5979Z" fill="#94A580" />
                    <path d="M65.616 15.7594L65.6035 15.8589L63.7721 37.1745L44.3004 28.0677L47.9258 25.9873L42.5564 16.6813L41.896 15.5349L38.5574 9.85411L32.752 0H51.6756L52.3234 1.12131L54.3166 4.57222L61.9906 17.8523L65.5412 15.7965L65.616 15.7594Z" fill="#7FA661" />
                    <path d="M68.7225 57.4857L67.9309 57.4856L64.9356 52.2977L58.6484 41.4077L69.539 35.1196L75.8261 46.0098L82.114 56.9003L82.3957 57.3872L70.0364 57.4864L68.7225 57.4857Z" fill="#94A580" />
                    <path d="M57.6165 76.4296L76.944 76.4289L78.2398 76.4298L87.7027 60.0394L69.6783 60.1841L68.3551 60.1833L57.6155 60.1831L57.616 56L40 68.3064L57.6174 80.6135L57.6165 76.4296Z" fill="#7FA661" />
                    <path d="M20.6733 62.7672L21.0693 62.0817L27.0597 62.0815L39.6342 62.0819L39.6345 74.6573L27.0598 74.6571L14.4844 74.6573L13.9219 74.6578L20.0157 63.9046L20.6733 62.7672Z" fill="#94A580" />
                    <path d="M28.0055 54.5158L24.3801 52.4229L19.0107 61.7289L18.3506 62.875L10.2279 77.2265L9.46808 78.5596L6.60275 73.6014L4.17347 69.4033L0 62.1651L0.647797 61.0438L10.3152 44.3005L6.68989 42.2201L24.7414 33.7735L26.1616 33.1133L28.0055 54.5158Z" fill="#7FA661" />
                </svg>

                <h1 className="text-4xl font-bold text-text-light dark:text-text-dark">Registrar entrega.</h1>
            </div>

            <div className='grid grid-cols-1 lg:grid-cols-2 gap-8'>

                <section className='bg-card-light dark:bg-card-dark rounded-lg shadow-sm p-6 lg:p-8 shadow-lg'>
                    <Table columns={['Material', 'Cantidad', 'Estado', 'Puntos']}>
                        {aceptados.map((material, index) => (
                            <tr key={index} className='border-b border-border-light dark:border-border-dark'>
                                <td className='px-4 py-2'>{material.nombre}</td>
                                <td className='px-4 py-2'>{material.peso}</td>
                                <td className='px-4 py-2'>{material.estado ? 'Aceptado' : 'Rechazado'}</td>
                                <td className='px-4 py-2'>{material.puntos}</td>
                            </tr>
                        ))}
                        {rechazados.map((material, index) => (
                            <tr key={index} className='border-b border-border-light dark:border-border-dark'>
                                <td className='px-4 py-2'>{material.nombre}</td>
                                <td className='px-4 py-2'>{material.peso}</td>
                                <td className='px-4 py-2'>{material.estado ? 'Aceptado' : 'Rechazado'}</td>
                                <td className='px-4 py-2'>{material.puntos}</td>
                            </tr>
                        ))}
                    </Table>
                </section>

                <form onSubmit={handleSubmit(onSubmit)} className='bg-card-light dark:bg-card-dark rounded-lg shadow-sm p-6 lg:p-8 flex flex-col space-y-8 shadow-lg'>
                    <div>
                        <SelectComponent
                            label='Ecopunto'
                            inputId='idEcoPunto'
                            inputName='idEcoPunto'
                            register={register('idEcoPunto', { ...validarReporteForm.idEcoPunto })}
                            spanAlert={errors['idEcoPunto']?.message ?? ''}
                            classNameSpanAlert='text-red-500 text-sm'
                        >

                            {ecoPuntos.map((eco) => (
                                <option key={eco.id} value={eco.id}>{eco.nombre}</option>
                            ))}

                        </SelectComponent>
                    </div>

                    <div>
                        <InputComponent
                            label='Identificacion del cliente'
                            inputId='cedulaCliente'
                            inputName='cedulaCliente'
                            inputType='text'
                            register={register('cedulaCliente', { ...validarReporteForm.cedulaCliente })}
                            spanAlert={errors['cedulaCliente']?.message ?? ''}
                            classNameSpanAlert='text-red-500 text-sm'
                        />
                    </div>

                    <div>
                        <h3 className="text-lg font-semibold mb-4">Clasificación de material</h3>
                        <div className='grid grid-cols-1 sm:grid-cols-[1fr_1fr_auto] gap-4 items-end'>
                            <div>
                                <SelectComponent
                                    label='Tipo de material'
                                    inputId='materialesEntrega'
                                    inputName='materialesEntrega'
                                    register={register('materialesEntrega')}
                                >

                                    {materiales.map((material) => (
                                        <option key={material.Id} value={material.Id}>{material.Nombre}</option>
                                    ))}

                                </SelectComponent>
                            </div>

                            <div>
                                <InputComponent
                                    label='Cantidad (Kg)'
                                    inputId='cantidad'
                                    inputName='cantidad'
                                    inputType='number'
                                    register={register('cantidad')}
                                />
                            </div>

                            <div className='relative inline-block w-12 mr-2 align-middle select-none transition duration-200 ease-in'>
                                <InputComponent
                                    label='Aceptado'
                                    inputId='estado'
                                    inputName='estado'
                                    inputType='checkbox'
                                    register={register('estado')}
                                />
                            </div>

                            <Button type='button' onClick={handleAgregarMaterial} className='w-full bg-blue-500 text-white hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary dark:focus:ring-offset-background-dark'>Agregar material</Button>


                            {errors['materialesEntrega'] && (
                                <span className='text-red-500 text-sm col-span-3'>{errors['materialesEntrega']?.message}</span>
                            )}
                        </div>
                    </div>

                    <div>
                        <h3 className="text-lg font-semibold mb-4">Resumen de entrega</h3>
                        <div className='flex flex-col gap-2'>
                            <span className="text-subtle-light dark:text-subtle-dark">Total entregado: {totalEntrega}kg</span>
                            <span className="text-subtle-light dark:text-subtle-dark">Aceptado: {totalAceptado}kg</span>
                            <span className="text-subtle-light dark:text-subtle-dark">Rechazado: {totalRechazado}kg</span>
                        </div>
                    </div>

                    <div className='flex items-center justify-between gap-4'>
                        <Button type='submit' className='bg-blue-500 text-white hover:bg-blue-600'>Registrar entrega</Button>
                        <Button type='button' onClick={handleCancelar} className='bg-red-500 text-white hover:bg-red-600'>Cancelar</Button>
                    </div>
                </form>
            </div>
        </>
    )
}

export default RegistrarReporte