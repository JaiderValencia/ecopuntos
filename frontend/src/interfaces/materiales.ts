export interface Material {
    Id: number
    Nombre: string
    Peso: number
}

export interface MaterialResponse {
    mensaje: string
    limite: number
    total: number
    materiales: Material[]
}

export interface MaterialEntrega {
    IdMaterial: number
    nombre?: string
    peso: number
    puntos: number
    estado: boolean
}