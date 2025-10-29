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