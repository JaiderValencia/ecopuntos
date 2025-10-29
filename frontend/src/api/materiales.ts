import type { MaterialResponse } from '../interfaces/materiales'
import axios from './axios'

export const getMateriales = async ({ limite }: { limite: number }): Promise<MaterialResponse> => {
    const response = await axios.get('/Material/ObtenerMateriales', {
        params: { limite }
    })

    return response.data
}