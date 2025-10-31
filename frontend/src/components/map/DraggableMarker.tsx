import { useCallback, useMemo, useRef, useState } from 'react'
import { MapContainer, Marker, Popup, TileLayer } from 'react-leaflet'
import type { Marker as LeafletMarker } from 'leaflet'
import { useMapContext } from '../../contex/map/map'

function DraggableMarker({ className }: { className?: string }) {
    const { setCoordinates, coordinates: { lat, lng } } = useMapContext()

    const center: [number, number] = [lat || 6.2710241, lng || -75.556521]
    const [draggable, setDraggable] = useState(false)
    const markerRef = useRef<LeafletMarker | null>(null)

    const eventHandlers = useMemo(
        () => ({
            dragend() {
                const marker = markerRef.current
                if (marker != null) {
                    const { lat, lng } = marker.getLatLng()
                    setCoordinates({ lat, lng })
                }
            },
        }),
        [setCoordinates],
    )

    const toggleDraggable = useCallback(() => {
        setDraggable((d) => !d)
    }, [])

    return (
        <div className={className}>
            <MapContainer center={center} zoom={13} style={{ width: '100%', height: '100%', zIndex: 0 }}>
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <Marker
                    draggable={draggable}
                    eventHandlers={eventHandlers}
                    position={center}
                    ref={markerRef}>
                    <Popup minWidth={90}>
                        <span onClick={toggleDraggable}>
                            {draggable
                                ? 'Puedes mover el marcador'
                                : 'Haz clic aquí para hacer que el marcador sea movible'}
                        </span>
                    </Popup>
                </Marker>
            </MapContainer>
        </div>
    )
}


export default DraggableMarker