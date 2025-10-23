using System.Collections.Generic;
using System.ComponentModel;
using webapicsharp.Interface.Servicios.Abstracciones;
using webapicsharp.Modelos;
using webapicsharp.Repositorios.Abstracciones;

namespace webapicsharp.Servicios
{
    public class ServicioEcoPunto : IServicioEcoPunto
    {

        private readonly IRepositorioEscrituraTabla _repoEscritura;
        private readonly IRepositorioActualizarTabla _repoActualizar;
        private readonly IRepositorioEliminarTabla _repoEliminar;
        private readonly IRepositorioJoinTresTablasFiltrado _repoJoinTresFiltrado;
        private readonly IRepositorioSubconsulta _repoSubconsulta;
        private readonly IRepositorioBusquedaPorCampoTabla _repoBuqueda;

        public ServicioEcoPunto(
            IRepositorioEscrituraTabla repoEscritura,
            IRepositorioActualizarTabla repoActualizar,
            IRepositorioEliminarTabla repoEliminar,
            IRepositorioJoinTresTablasFiltrado repoJoinTresFiltrado,
            IRepositorioSubconsulta repoSubconsulta,
            IRepositorioBusquedaPorCampoTabla repoBuqueda)
        {
            _repoEscritura = repoEscritura;
            _repoActualizar = repoActualizar;
            _repoEliminar = repoEliminar;
            _repoJoinTresFiltrado = repoJoinTresFiltrado;
            _repoSubconsulta = repoSubconsulta;
            _repoBuqueda = repoBuqueda;
        }

        public async Task<EcoPunto> BuscarEcoPuntoPorIDAsync(int id)
        {
            try
            {
                var dictEcoPuntoFiltrado = await _repoJoinTresFiltrado.JoinTresTablasAsync(
                   "EcoPunto",
                   "MaterialEcoPunto",
                   "Material",
                   "Id",
                   "IdEcoPunto",
                   "IdMaterial",
                   "Id",
                   columnasSeleccionadas: "*",
                   tipoJoin: "INNER",
                   limite: null,
                   campoFiltro: "Id",
                   valorFiltro: id
                   );

                var trabajadorDatos = await _repoJoinTresFiltrado.JoinTresTablasAsync(
                   "Usuario",
                   "Empleado",
                   "Trabajador",
                   "Id",
                   "Id",
                   "Id",
                   "Id",
                   columnasSeleccionadas: "*",
                   tipoJoin: "INNER",
                   limite: null,
                   campoFiltro: "Id",
                   valorFiltro: dictEcoPuntoFiltrado[0]!["IdTrabajador"]!
                   );

                List<Material> materialesAceptados = new List<Material>();

                try
                {
                    for (var i = 0; i < dictEcoPuntoFiltrado!.Count; i++)
                    {
                        var materialDict = dictEcoPuntoFiltrado[i];

                        int idMaterial = Convert.ToInt32(materialDict["Id"]);
                        string nombre = materialDict?["Nombre"]!.ToString() ?? "";
                        double peso = Convert.ToDouble(materialDict!["Peso"] ?? 0);

                        materialesAceptados.Add(new Material(
                            id: idMaterial,
                            nombre: nombre,
                            peso: peso
                            ));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Ocurrio un error al listar los materiales aceptados: {e.Message}");
                }

                var respuesta = new EcoPunto
                {
                    Id = id,
                    Horario = dictEcoPuntoFiltrado[0]?["Horario"]!.ToString() ?? "",
                    Ubicacion = new Ubicacion
                    {
                        Latitud = dictEcoPuntoFiltrado[0]?["Latitud"]!.ToString() ?? "",
                        Longitud = dictEcoPuntoFiltrado[0]?["Longitud"]!.ToString() ?? "",
                        Direccion = dictEcoPuntoFiltrado[0]?["Direccion"]!.ToString() ?? "",
                    },
                    Trabajador = new Trabajador(
                        Convert.ToInt32(trabajadorDatos[0]["Id"]),
                        trabajadorDatos[0]["Nombre"]?.ToString() ?? "",
                        trabajadorDatos[0]["Cedula"]?.ToString() ?? "",
                        trabajadorDatos[0]["Correo"]?.ToString() ?? "",
                        trabajadorDatos[0]["Direccion"]?.ToString() ?? "",
                        trabajadorDatos[0]["Telefono"]?.ToString() ?? "",
                        trabajadorDatos[0]["Contrasena"]?.ToString() ?? "",
                        trabajadorDatos[0]["CodigoDeEmpleado"]?.ToString() ?? "",
                        trabajadorDatos[0]["Horario"]?.ToString() ?? ""
                    ){},
                    MaterialesAceptados = materialesAceptados,
                };

                return respuesta;
            }
            catch(Exception e){
                throw new Exception($"Ocurrio un error al buscar el EcoPunto: ${e.Message}");
            }
        }

        public async Task<EcoPunto> CrearEcoPuntoAsync(
            string codigoDeEmpelado,
            string latitud,
            string longitud,
            string direccion,
            string horario,
            List<Material> materiales)
        {
            try
            {
                var trabajadorDatos = await _repoBuqueda.BuscarPorCampoAsync(
                    "Empleado",
                    campo: "CodigoDeEmpleado",
                    valor: codigoDeEmpelado
                );

                var idTrabajador = int.TryParse(trabajadorDatos![0]["Id"]?.ToString(), out var id) ? id : 0;

                var datosEcoPunto = new Dictionary<string, object?>
                {
                    { "IdTrabajador", idTrabajador},
                    { "Latitud", latitud },
                    { "Longitud", longitud },
                    { "Direccion", direccion },
                    { "Horario", horario }
                };

                var ecoPuntoCreado = await _repoEscritura.InsertarAsync("EcoPunto", datosEcoPunto);
                int idEcoPunto = Convert.ToInt32(ecoPuntoCreado["Id"]);

                foreach (var material in materiales)
                {
                    var datosRelacion = new Dictionary<string, object?>
                    {
                        { "IdEcoPunto", idEcoPunto },
                        { "IdMaterial", Convert.ToInt32(material.Id.ToString()) }
                    };
                    await _repoEscritura.InsertarAsync("MaterialEcoPunto", datosRelacion);
                }

                var materialesDatos = await _repoSubconsulta.EjecutarSubconsultaAsync(
                    "Material",
                    "MaterialEcoPunto",
                    "Id",
                    "IdMaterial",
                    "IdEcoPunto",
                    idEcoPunto
                    );

                List<Material> materialesAceptados = new List<Material>();

                try
                {
                    for (var i = 0; i < materialesDatos!.Count; i++)
                    {
                        var materialDict = materialesDatos[i];

                        int idMaterial = Convert.ToInt32(materialDict["Id"]);
                        string nombre = materialDict?["Nombre"]!.ToString() ?? "";
                        double peso = Convert.ToDouble(materialDict!["Peso"] ?? 0);

                        materialesAceptados.Add(new Material(
                            id: idMaterial,
                            nombre: nombre,
                            peso: peso
                            ));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Ocurrio un error al listar los materiales aceptados: {e.Message}");
                }

                var respuesta = await BuscarEcoPuntoPorIDAsync(idEcoPunto);

                return respuesta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el EcoPunto: {ex.Message}");
            }
        }

        public async Task<EcoPunto> ActualizarEcoPuntoPorIDAsync(
            int idEcoPunto,
            string codigoDeEmpleado,
            string latitud,
            string longitud,
            string direccion,
            string horario,
            List<Material> materiales)
        {
            try
            {
                var trabajadorDatos = await _repoJoinTresFiltrado.JoinTresTablasAsync(
                    "Empleado",
                    "Trabajador",
                    "Usuario",
                    "Id",
                    "Id",
                    "Id",
                    "Id",
                    columnasSeleccionadas: "*",
                    tipoJoin: "INNER",
                    limite: null,
                    campoFiltro: "CodigoDeEmpleado",
                    valorFiltro: codigoDeEmpleado
                );

                if (trabajadorDatos == null || trabajadorDatos.Count == 0)
                    throw new Exception("No se encontró ningún trabajador con el código proporcionado.");

                var trabajadorDict = trabajadorDatos[0];
                int idTrabajador = trabajadorDict["Id"] != null ? Convert.ToInt32(trabajadorDict["Id"]) : 0;

                var datosEcoPunto = new Dictionary<string, object?>
                    {
                        { "IdTrabajador", idTrabajador },
                        { "Latitud", latitud },
                        { "Longitud", longitud },
                        { "Direccion", direccion },
                        { "Horario", horario }
                    };

                await _repoActualizar.ActualizarPorCampoAsync(
                    "EcoPunto",
                    campoFiltro: "Id",
                    valorFiltro: idEcoPunto,
                    nuevosValores: datosEcoPunto
                );

                await ActualizarRelacionesMaterialEcoPunto(idEcoPunto, materiales);

                var respuesta = await BuscarEcoPuntoPorIDAsync(idEcoPunto);

                return respuesta;
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrió un error al actualizar el EcoPunto: {e.Message}");
            }
        }

        public async Task ActualizarRelacionesMaterialEcoPunto( int idEcoPunto, List<Material> materiales)
        {
            try
            {
                var materialesActuales = await _repoSubconsulta.EjecutarSubconsultaAsync(
                    "Material",
                    "MaterialEcoPunto",
                    "Id",
                    "IdMaterial",
                    "IdEcoPunto",
                    idEcoPunto
                );

                var idsMaterialExistentes = materialesActuales?.Select(m => Convert.ToInt32(m["IdMaterial"])).ToList() ?? new List<int>();
                var idsMaterialNuevos = materiales.Select(m => Convert.ToInt32(m.Id)).ToList();

                var idsAEliminar = idsMaterialExistentes.Except(idsMaterialNuevos).ToList();
                foreach (var idMat in idsAEliminar)
                {
                    await _repoEliminar.EliminarPorCampoAsync(
                        "MaterialEcoPunto",
                        "IdMaterial",
                        idMat
                    );
                }

                //Inserta los materiales nuevos si existen
                foreach (var material in materiales)
                {
                    int idMat = material.Id;

                    if (!idsMaterialExistentes.Contains(idMat))
                    {
                        var nuevasRelaciones = new Dictionary<string, object?>
                        {
                            { "IdEcoPunto", idEcoPunto },
                            { "IdMaterial", idMat }
                        };

                        await _repoEscritura.InsertarAsync("MaterialEcoPunto", nuevasRelaciones);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrio un error al eliminar las relaciones entre materiales y ecopunto: {e.Message}");
            }
        }

    }
}
