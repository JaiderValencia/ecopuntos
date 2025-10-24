import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet"
import 'leaflet/dist/leaflet.css'
import type { Ecopunto } from '../../interfaces/ecopunto'

function Map({ className, ecopuntos = [] }: { className?: string, ecopuntos?: Ecopunto[] }) {
    const center: [number, number] = [6.2710241, -75.55652]
    return (
        <div className={className}>
            <MapContainer center={center} zoom={13} style={{ width: '100%', height: '100%' }}>
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                {ecopuntos.length > 0 && ecopuntos.map((ecopunto) => (
                    <Marker key={ecopunto.id} position={[Number(ecopunto.ubicacion.latitud), Number(ecopunto.ubicacion.longitud)]}>
                        <Popup>
                            Ecopunto en: <br />{ecopunto.ubicacion.direccion}.
                        </Popup>
                    </Marker>
                ))}
            </MapContainer>
        </div>
    )
}

export default Map