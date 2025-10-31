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

export const diasAtencion = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo']
export const horasAtencion = ['8:00-17:00', '7:00-16:00', '6:00-12:00', '10:00-19:00']

export const formatearHorariosAtencion = (dias: string[], horas: string): string => {
  if (dias.length === 0) return horas
  if (dias.length === 1) return dias[0] + ' ' + horas
  
  // Convertir días a índices
  const indices = dias.map(dia => diasAtencion.indexOf(dia)).sort((a, b) => a - b)

  // Agrupar días consecutivos
  const grupos: string[] = []
  let inicio = indices[0]
  let fin = indices[0]

  for (let i = 1; i <= indices.length; i++) {
    if (i < indices.length && indices[i] === fin + 1) {
      // Día consecutivo
      fin = indices[i]
    } else {
      // Fin de grupo
      if (inicio === fin) {
        grupos.push(diasAtencion[inicio])
      } else if (fin === inicio + 1) {
        // Solo dos días consecutivos, mejor separarlos con coma
        grupos.push(diasAtencion[inicio])
        grupos.push(diasAtencion[fin])
      } else {
        grupos.push(`De ${diasAtencion[inicio]} a ${diasAtencion[fin]}`)
      }

      if (i < indices.length) {
        inicio = indices[i]
        fin = indices[i]
      }
    }
  }

  return grupos.join(', ') + ' ' + horas
}

export const parsearHorarioEcopunto = (horarioTexto: string): { diasAtencionEcopunto: string[], horasAtencionEcopunto: string } => {
  // Extraer las horas (ejemplo: "8:00-17:00")
  const matchHoras = horarioTexto.match(/(\d{1,2}:\d{2}-\d{1,2}:\d{2})/)
  const horasAtencionEcopunto = matchHoras ? matchHoras[1] : ''

  const diasAtencionEcopunto: string[] = []

  // Detectar rangos: "De Lunes a Miércoles"
  const matchRango = horarioTexto.match(/De\s+(\w+)\s+a\s+(\w+)/i)
  if (matchRango) {
    const diaInicio = matchRango[1]
    const diaFin = matchRango[2]
    
    const indiceInicio = diasAtencion.indexOf(diaInicio)
    const indiceFin = diasAtencion.indexOf(diaFin)
    
    if (indiceInicio !== -1 && indiceFin !== -1) {
      for (let i = indiceInicio; i <= indiceFin; i++) {
        diasAtencionEcopunto.push(diasAtencion[i])
      }
    }
  } else {
    // Detectar lista de días separados por comas
    // Primero eliminar las horas del texto para evitar confusión
    const textoSinHoras = horarioTexto.replace(/\d{1,2}:\d{2}-\d{1,2}:\d{2}/, '')
    
    for (const dia of diasAtencion) {
      if (textoSinHoras.includes(dia)) {
        diasAtencionEcopunto.push(dia)
      }
    }
  }

  return { diasAtencionEcopunto, horasAtencionEcopunto }
}