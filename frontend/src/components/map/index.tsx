import { memo, useMemo } from 'react'
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet"
import { Icon } from 'leaflet'
import 'leaflet/dist/leaflet.css'
import type { Ecopunto } from '../../interfaces/ecopunto'

// Fix del icono por defecto de Leaflet
const defaultIcon = new Icon({
    iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
    iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
    shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41]
})

interface MapProps {
    className?: string
    ecopuntos?: Ecopunto[]
}

const Map = memo(({ className, ecopuntos = [] }: MapProps) => {
    const center: [number, number] = useMemo(() => [6.2710241, -75.55652], [])
    
    const markers = useMemo(() => 
        ecopuntos.map((ecopunto) => (
            <Marker 
                key={ecopunto.id} 
                position={[Number(ecopunto.ubicacion.latitud), Number(ecopunto.ubicacion.longitud)]}
                icon={defaultIcon}
            >
                <Popup>
                    <strong>{ecopunto.ubicacion.direccion || 'Ecopunto'}</strong>
                    <br />
                    {ecopunto.ubicacion.direccion}
                </Popup>
            </Marker>
        )), 
    [ecopuntos])

    return (
        <div className={className}>
            <MapContainer 
                center={center} 
                zoom={13} 
                style={{ width: '100%', height: '100%', zIndex: 0 }}
                scrollWheelZoom={true}
                preferCanvas={true}
            >
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                    maxZoom={19}
                    updateWhenIdle={true}
                    keepBuffer={2}
                />
                {markers}
            </MapContainer>
        </div>
    )
})

Map.displayName = 'Map'

export default Map