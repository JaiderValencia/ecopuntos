export interface ClienteDetallado {
    Id: number
    Nombre: string
    Cedula: string
    Correo: string
    Direccion: string
    Telefono: string
    Contrasena: string
    EcoPuntos: number
    TotalEntregas: number
    PesoTotalEntregado: number
    UltimoEcopunto: string | null
}
