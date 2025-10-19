import { useState } from 'react'
import Card from '../../components/card'
import Table from '../../components/table'

function Inicio() {
    const [sucursales] = useState([
        {
            "Nombre": "Ecopunto Norte",
            "Dirección": "Cra 7 # 45-89",
            "Materiales": "Calle 99 #75-156",
            "Responsable": "Plástico, Vidrio",
            "Estado": (
                <span className="inline-block bg-green-200 text-green-800 px-2 py-1 rounded text-xs">Abierto</span>
            )
        },
        {
            "Nombre": "Ecopunto Sur",
            "Dirección": "Cra. 55 #65 B 256",
            "Materiales": "Metal, Plástico",
            "Responsable": "Tomas Doe",
            "Estado": (
                <span className="inline-block bg-gray-300 text-gray-800 px-2 py-1 rounded text-xs">Cerrado</span>
            )
        }
    ])

    return (
        <div className='p-7 flex flex-col'>
            <section className='flex gap-10 flex-col lg:flex-row mb-8'>
                <Card className='h-[416px] lg:w-[557%]'>
                    <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2174.726093940677!2d-75.56143847353155!3d6.281689146120537!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x8e4429c63d3c110f%3A0xf09de83123a50796!2sChoco%20Freseo%20-%20Aranjuez!5e0!3m2!1ses!2sco!4v1760833952713!5m2!1ses!2sco" width="100%" height="100%" style={{ border: 0 }} loading="lazy" referrerPolicy="no-referrer-when-downgrade"></iframe>
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
                <Table data={sucursales} />
            </section>
        </div>
    )
}
export default Inicio