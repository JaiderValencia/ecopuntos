import { createContext, useContext } from 'react'

export type MapContextValue = {
    coordinates: {
        lat: number,
        lng: number
    }
    setCoordinates: (coords: MapContextValue['coordinates']) => void
}

export const defaultCoordinates = {
    lat: 0,
    lng: 0
}

export const MapContext = createContext<MapContextValue>({ coordinates: defaultCoordinates, setCoordinates: () => { } })

export const useMapContext = () => (useContext(MapContext))
