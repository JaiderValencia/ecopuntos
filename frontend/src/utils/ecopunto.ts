import type { MaterialesAceptado } from '../interfaces/ecopunto'

export const isOpen = (horario: string): boolean => {
  // Extraer el rango horario del texto (ejemplo: "8:00-17:00")
  const match = horario.match(/(\d{1,2}:\d{2})-(\d{1,2}:\d{2})/)
  if (!match) return false

  const [, aperturaStr, cierreStr] = match

  // Convertir la hora de apertura y cierre a minutos desde medianoche
  function aMinutos(hora: string): number {
    const [h, m] = hora.split(':').map(Number)
    return h * 60 + m
  }

  const apertura = aMinutos(aperturaStr)
  const cierre = aMinutos(cierreStr)

  // Obtener la hora actual en minutos desde medianoche
  const ahora = new Date()
  const minutosAhora = ahora.getHours() * 60 + ahora.getMinutes()

  // Verificar si la hora actual está dentro del rango
  return minutosAhora >= apertura && minutosAhora <= cierre
}

export const materialsAcceptedJoined = (materialsAccepted: MaterialesAceptado[]): string => {
  let materials: string = ''

  for (const material of materialsAccepted) {
    materials += material.nombre + ', '
  }

  return materials.slice(0, -2) // Remove trailing comma and space  
}