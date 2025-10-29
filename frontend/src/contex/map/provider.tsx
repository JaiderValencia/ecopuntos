import { createElement, useState } from 'react'
import { defaultCoordinates, MapContext } from './map'

export function MapProvider({ children }: { children: React.ReactNode }) {
    const [coordinates, setCoordinates] = useState(defaultCoordinates)

    return createElement(
        MapContext.Provider,
        {
            value: {
                coordinates,
                setCoordinates
            }
        },
        children
    )
}