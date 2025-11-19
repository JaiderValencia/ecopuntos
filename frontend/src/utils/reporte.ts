export const validarReporteForm = {
    idEcoPunto: {
        required: 'El ecopunto es obligatorio',
    },
    cedulaCliente: {
        required: 'La cédula del cliente es obligatoria',
    },
    materialesEntrega: {
        required: 'El material es obligatorio',
    },
    cantidad: {
        required: 'La cantidad es obligatoria',
        min: { value: 1, message: 'La cantidad debe ser al menos 1' },
    },        
}