import { useForm } from 'react-hook-form'
import InputComponent from '../../components/input/texts'
import DraggableMarker from '../../components/map/DraggableMarker'
import type { Ecopunto, formDataUpdateForm, formDataUpdateRequest } from '../../interfaces/ecopunto'
import { useEffect, useState } from 'react'
import type { Material } from '../../interfaces/materiales'
import SelectComponent from '../../components/input/select'
import ButtonForm from '../../components/ecopuntos/buttonForm'
import { diasAtencion, formatearHorariosAtencion, horasAtencion, parsearHorarioEcopunto } from '../../utils/ecopunto'
import type { Empleado } from '../../interfaces/empleados'
import { useNavigate } from 'react-router-dom'
import Button from '../../components/button'
import { deleteEcopunto, searchEcopuntoById, updateEcopunto } from '../../api/ecopuntos'
import { useMapContext } from '../../contex/map/map'
import { getMateriales } from '../../api/materiales'
import { getEmpleados } from '../../api/empleados'


function EditEcopuntos() {
    const { register, handleSubmit, watch, setValue } = useForm<formDataUpdateForm>()
    const Navigate = useNavigate()

    const { coordinates, setCoordinates } = useMapContext()
    const [ecopunto, setEcopunto] = useState<Ecopunto | null>(null)
    const [materiales, setMateriales] = useState<Material[]>([])
    const [selectedMateriales, setSelectedMateriales] = useState<Material[]>([])
    const [empleados, setEmpleados] = useState<Empleado[]>([])
    const [diasState, setDiasState] = useState<string[]>([])
    const [horasState, setHorasState] = useState<string>('')
    const materialesInput = watch('materiales')


    const deleteEcopuntoHandle = async () => {
        try {
            if (!ecopunto) return

            await deleteEcopunto(ecopunto.id)
            return Navigate('/ecopuntos')
        } catch (error) {
            console.log(error)
        }
    }

    const fetchMateriales = async () => {
        const { materiales } = await getMateriales({ limite: 100 })
        setMateriales(materiales)
    }

    useEffect(() => {
        if (materialesInput) {
            const materialesSeleccionados = materiales.filter(material =>
                materialesInput.includes(material.Id.toString())
            )
            setSelectedMateriales(materialesSeleccionados)
        }

    }, [materialesInput, materiales])

    const fetchEmpleados = async () => {
        const { empleados } = await getEmpleados({ limit: 100 })
        setEmpleados(empleados)
    }

    //cargar datos iniciales
    useEffect(() => {
        fetchMateriales()
        fetchEmpleados()
    }, [])

    const idEcopunto = watch('id')
    const fetchEcopuntoById = async () => {
        const response = await searchEcopuntoById(idEcopunto)
        setEcopunto(response.ecoPunto)

        setCoordinates({
            lat: response.ecoPunto.ubicacion.latitud as number,
            lng: response.ecoPunto.ubicacion.longitud as number
        })
    }

    useEffect(() => {
        if (ecopunto) {
            setValue('nombre', ecopunto.nombre)
            setValue('direccion', ecopunto.ubicacion.direccion)
            setValue('latitud', ecopunto.ubicacion.latitud as number)
            setValue('longitud', ecopunto.ubicacion.longitud as number)
            setValue('codigoDeEmpleado', ecopunto.trabajador.codigoDeEmpleado.toString())
            setValue('materiales', ecopunto.materialesAceptados.map(material => material.id.toString()))

            const { diasAtencionEcopunto, horasAtencionEcopunto } = parsearHorarioEcopunto(ecopunto.horario)

            setDiasState(diasAtencionEcopunto)
            setHorasState(horasAtencionEcopunto)

        }
    }, [ecopunto, setValue])

    // Actualizar latitud y longitud cuando cambian las coordenadas
    useEffect(() => {
        setValue('latitud', coordinates.lat)
        setValue('longitud', coordinates.lng)
    }, [coordinates, setValue])

    const handleDias = (dia: string): void => {
        if (!diasState.includes(dia)) {
            setDiasState([...diasState, dia])
            return
        }

        setDiasState(diasState.filter(d => d !== dia))
    }

    const handleHoras = (horas: string): void => {
        if (horasState !== horas) {
            setHorasState(horas)
            return
        }

        setHorasState('')
    }

    const onCancel = () => {
        return Navigate('/ecopuntos')
    }

    const onSubmit = async (data: formDataUpdateForm) => {
        const formData: formDataUpdateRequest = {
            id: ecopunto?.id || 0,
            nombre: data.nombre,
            horario: formatearHorariosAtencion(diasState, horasState),
            direccion: data.direccion,
            latitud: `${data.latitud}`,
            longitud: `${data.longitud}`,
            materiales: selectedMateriales.map(material => ({
                id: material.Id,
                nombre: '',
                peso: 0
            })),
            codigoDeEmpleado: data.codigoDeEmpleado,
        }

        try {
            await updateEcopunto(formData)

            return Navigate('/ecopuntos')

        } catch (error) {
            console.log(error)
        }
    }

    return (
        <>
            <h2 className="text-3xl font-bold mb-8 text-text-light dark:text-text-dark">Actualizar Ecopuntos</h2>
            <form onSubmit={handleSubmit(onSubmit)} className='grid grid-cols-1 lg:grid-cols-2 gap-8'>
                <div className="space-y-8">
                    <div>
                        <InputComponent label="Buscar ecopunto (ID):" register={register('id')} inputName='id' inputId='id' inputType="text" inputPlaceholder="Ingrese el ID del ecopunto" />
                        <div className="flex gap-4">
                            <Button type="button" className='bg-blue-600 text-white hover:bg-blue-700' onClick={() => fetchEcopuntoById()}>Buscar Ecopunto</Button>
                            <Button type="button" className='bg-rose-600 text-white hover:bg-rose-700' onClick={() => deleteEcopuntoHandle()}>Eliminar Ecopunto</Button>
                        </div>
                    </div>

                    <DraggableMarker className="h-96 w-full rounded-lg shadow-md" />

                    <div className="bg-card-light dark:bg-card-dark p-4 rounded-lg shadow-md">
                        <table className="w-full border-collapse">
                            <thead>
                                <tr>
                                    <th
                                        className="border border-border-light dark:border-border-dark p-3 text-left font-semibold text-text-light dark:text-text-dark">
                                        Materiales</th>
                                </tr>
                            </thead>
                            <tbody>
                                {selectedMateriales.length != 0 && selectedMateriales.map((material, index) => (
                                    <tr key={index}>
                                        <td key={material.Id} className="border border-border-light dark:border-border-dark p-3  align-top">
                                            {material.Nombre}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className='bg-background-light dark:bg-card-dark p-6 rounded-lg shadow-lg'>
                    <div className="space-y-6">
                        <div>
                            <InputComponent
                                label="Nombre del Ecopunto"
                                inputType="text"
                                inputId="nombre-ecopunto"
                                inputName='nombre'
                                register={register('nombre')}
                            />
                        </div>
                        <div>
                            <InputComponent
                                label="Direccion del Ecopunto"
                                inputType="text"
                                inputId="direccion-ecopunto"
                                inputName='direccion'
                                register={register('direccion')}
                            />
                        </div>
                        <div className='grid grid-cols-2 gap-8'>
                            <InputComponent
                                label="Latitud"
                                inputType="text"
                                inputId="latitud-ecopunto"
                                inputName='latitud'
                                register={register('latitud')}
                            />
                            <InputComponent
                                label="Longitud"
                                inputType="text"
                                inputId="longitud-ecopunto"
                                inputName='longitud'
                                register={register('longitud')}
                            />
                        </div>
                        <div>
                            <SelectComponent multiple={true} register={register('materiales')} inputId='materiales-aceptados' inputName='materiales' label='Materiales aceptados' >
                                {materiales.map((material) => (
                                    <option key={material.Id} value={material.Id}>{material.Nombre}</option>
                                ))}
                            </SelectComponent>
                        </div>
                        <div>
                            <SelectComponent register={register('codigoDeEmpleado')} inputId='codigoEmpleado' inputName='codigoDeEmpleado' label='Responsable encargado' >
                                {empleados.map((empleado) => (
                                    <option key={empleado.Id} value={empleado.CodigoDeEmpleado}>{empleado.Nombre}</option>
                                ))}
                            </SelectComponent>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-text-light dark:text-text-dark">Días de
                                atención</label>
                            <div className="mt-2 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-2">
                                {diasAtencion.map((dia) => (
                                    <ButtonForm key={dia} isActive={diasState.includes(dia)} onClick={handleDias}>{dia}</ButtonForm>
                                ))}
                            </div>
                        </div>
                        <div>
                            <label className="block text-sm font-medium text-text-light dark:text-text-dark">Horario de
                                atención</label>
                            <div className="mt-2 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-2">
                                {horasAtencion.map((hora) => (
                                    <ButtonForm key={hora} isActive={horasState === hora} onClick={handleHoras}>{hora}</ButtonForm>
                                ))}
                            </div>
                        </div>
                        <div className="flex justify-between space-x-4 pt-4">
                            <Button type="submit" className="bg-blue-600 text-white hover:bg-blue-700">Actualizar Ecopunto</Button>
                            <Button type="button" onClick={onCancel} className="bg-red-600 text-white hover:bg-red-700">Cancelar</Button>
                        </div>
                    </div>
                </div>
            </form>
        </>
    )
}

export default EditEcopuntos