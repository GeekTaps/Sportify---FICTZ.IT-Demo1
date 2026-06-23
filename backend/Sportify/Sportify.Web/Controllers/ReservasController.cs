using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;
using Sportify.Aplicacion.Excepciones;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Web.DTOs;

namespace Sportify.Web.Controllers
{
    // [ApiController] indica que esta clase responderá a peticiones HTTP y formateará las respuestas en formato JSON
    [ApiController] 
    // [Route] define la URL base para todos los endpoints de este controlador (ej: /api/Reservas)
    [Route("api/[controller]")] 
    public class ReservasController : ControllerBase
    {
        // Declaramos los casos de uso (Use Cases) como dependencias de solo lectura.
        // Esto sigue el principio de Inyección de Dependencias, aislando la lógica de negocio del controlador.
        private readonly ReservaAltaUseCase _reservaAltaUseCase;
        private readonly ReservaBajaUseCase _reservaBajaUseCase;
        private readonly ReservaBusquedaUseCase _reservaBusquedaUseCase;
        private readonly ReservaListadoUseCase _reservaListadoUseCase;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IRepositorioTurno _repositorioTurno;
        private readonly IRepositorioDeporte _repositorioDeporte;
        private readonly IRepositorioReserva _repositorioReserva;

        // El constructor recibe los casos de uso inyectados automáticamente por el contenedor de dependencias de ASP.NET (configurado en Program.cs)
        public ReservasController(
            ReservaAltaUseCase reservaAltaUseCase,
            ReservaBajaUseCase reservaBajaUseCase,
            ReservaBusquedaUseCase reservaBusquedaUseCase,
            ReservaListadoUseCase reservaListadoUseCase,
            UserManager<UsuarioIdentity> userManager,
            IRepositorioTurno repositorioTurno,
            IRepositorioDeporte repositorioDeporte,
            IRepositorioReserva repositorioReserva)
        {
            _reservaAltaUseCase = reservaAltaUseCase;
            _reservaBajaUseCase = reservaBajaUseCase;
            _reservaBusquedaUseCase = reservaBusquedaUseCase;
            _reservaListadoUseCase = reservaListadoUseCase;
            _userManager = userManager;
            _repositorioTurno = repositorioTurno;
            _repositorioDeporte = repositorioDeporte;
            _repositorioReserva = repositorioReserva;
        }

        // POST: api/Reservas
        // Endpoint que atiende las solicitudes POST para crear (dar de alta) una nueva reserva.
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaRequest request)
        {
            try
            {
                // Instanciamos la entidad de dominio Reserva con los datos recibidos del cliente en el DTO
                var nuevaReserva = new Reserva(request.IdUsuario, request.IdTurno, request.Paga, request.Monto, request.Titulo);
                
                // Ejecutamos el caso de uso correspondiente para persistir la reserva en la base de datos
                await _reservaAltaUseCase.Ejecutar(nuevaReserva);
                
                // Retornamos HTTP 201 Created, indicando que el recurso fue creado exitosamente. 
                // Se incluye la ruta (BuscarReserva) para poder acceder a la nueva reserva creada.
                return CreatedAtAction(nameof(BuscarReserva), new { id = nuevaReserva.id }, nuevaReserva);
            }
            catch (EntidadNotFoundException ex)
            {
                // Si la reserva ya existe o hay un error de validación de entidad, devolvemos HTTP 400 Bad Request
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                // Cualquier otra excepción no controlada genera un HTTP 500 Internal Server Error
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // GET: api/Reservas/usuario/email/{email}
        // Endpoint que devuelve la lista de reservas hechas por un usuario en particular, usando su Email.
        [HttpGet("usuario/email/{email}")]
        public async Task<IActionResult> ListarReservasUsuario(string email)
        {
            try
            {
                // Buscamos al usuario por su email
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return NotFound(new { mensaje = "No se encontró ningún usuario registrado con ese email." });
                }

                // Convertimos el ID del usuario (string en Identity) a Guid
                Guid idUsuario = Guid.Parse(user.Id);

                // Llamamos al caso de uso de listado de reservas para este ID de usuario específico
                var reservas = await _reservaListadoUseCase.Ejecutar(idUsuario);
                
                // Si fue exitoso, devolvemos HTTP 200 OK junto con el arreglo JSON de las reservas
                return Ok(reservas); 
            }
            catch (ListadoVacioException ex)
            {
                // La excepción indica que no hay reservas. Retornamos HTTP 404 Not Found.
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // GET: api/Reservas/usuario/{id}
        // Endpoint que devuelve la lista de reservas de un usuario por su ID de Identity.
        [HttpGet("usuario/{id:guid}")]
        public async Task<IActionResult> ListarReservasUsuarioPorId(Guid id)
        {
            try
            {
                var reservas = await _reservaListadoUseCase.Ejecutar(id);
                if (reservas == null || reservas.Count == 0) {
                    throw new ListadoVacioException("No poseés reservas");
                }
                return Ok(reservas);
            }
            catch (ListadoVacioException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "No poseés reservas", detalle = ex.Message });
            }
        }

        // GET: api/Reservas/{id}
        // Endpoint que busca y devuelve una única reserva buscando por su ID de reserva.
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> BuscarReserva(Guid id)
        {
            try
            {
                // Ejecutamos el caso de uso de búsqueda utilizando el identificador
                var reserva = await _reservaBusquedaUseCase.Ejecutar(id);
                
                // Retornamos HTTP 200 OK con el objeto reserva encontrado
                return Ok(reserva); 
            }
            catch (EntidadNotFoundException ex)
            {
                // Si el ID de reserva no se encuentra en base de datos, retornamos HTTP 404 Not Found
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("{id:guid}/detalles")]
        public async Task<IActionResult> BuscarDetalleReserva(Guid id)
        {
            try
            {
                var reserva = await _reservaBusquedaUseCase.Ejecutar(id);
                // Obtener Turno
                var turnoList = await _repositorioTurno.ListarTurnos();
                var turno = turnoList.FirstOrDefault(t => t.Id == reserva.idTurno);
                if (turno == null) return NotFound(new { mensaje = "Turno no encontrado" });

                // Obtener Deporte
                var deportesList = await _repositorioDeporte.ListarDeportes();
                var deporte = deportesList.FirstOrDefault(d => d.id == turno.IdDeporte);
                var nombreActividad = deporte?.nombre ?? "Desconocida";

                // Obtener usuario para saber las cancelaciones y suspensión
                var user = await _userManager.FindByIdAsync(reserva.idUsuario.ToString());
                int cancelaciones = user?.CancelacionesMes ?? 0;
                bool suspendido = user?.Suspendido ?? false;

                // Calcular horas de antelación
                var fechaTurno = turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan());
                var horasAnticipacion = (fechaTurno - DateTime.Now).TotalHours;

                var dto = new ReservaDetalleDTO
                {
                    IdReserva = reserva.id,
                    Actividad = nombreActividad,
                    Fecha = turno.Fecha.ToString("dd/MM/yy"),
                    Horario = turno.horaInicio.ToString("HH:mm") + "hs",
                    Profesor = turno.nommbreProfesor ?? "Sin profesor",
                    HorasAnticipacion = horasAnticipacion,
                    CancelacionesMes = cancelaciones,
                    Suspendido = suspendido
                };

                return Ok(dto);
            }
            catch (EntidadNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // GET: api/Reservas/turno/{id}/inscriptos
        [HttpGet("turno/{id:guid}/inscriptos")]
        public async Task<IActionResult> ObtenerInscriptosPorTurno(Guid id)
        {
            try
            {
                int count = await _repositorioReserva.ContarReservasPorTurno(id);
                return Ok(new { count = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPost("reservar-turno")]
        public async Task<IActionResult> ReservarTurno([FromBody] ReservarTurnoRequest request)
        {
            try
            {
                // Buscar usuario
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado." });
                }

                if (user.Suspendido)
                {
                    return BadRequest(new { mensaje = "Tu cuenta está suspendida. Ya no es posible reservar más clases hasta el mes siguiente y no se devolverá el valor de las señas depositadas en caso de cancelar." });
                }

                // Buscar turno
                var turnoList = await _repositorioTurno.ListarTurnos();
                var turno = turnoList.FirstOrDefault(t => t.Id == request.IdTurno);
                if (turno == null)
                {
                    return NotFound(new { mensaje = "Turno no encontrado." });
                }

                // No permitir reservar turnos que ya pasaron
                var fechaTurno = turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan());
                if (fechaTurno <= DateTime.Now)
                {
                    return BadRequest(new { mensaje = "No se puede reservar un turno que ya pasó." });
                }

                // Verificar cupo
                if (turno.cupo <= 0)
                {
                    return BadRequest(new { mensaje = "No hay cupo disponible para este turno." });
                }

                // Verificar duplicados
                var reservasUsuario = await _repositorioReserva.listarReservasUsuario(Guid.Parse(user.Id));
                if (reservasUsuario.Any(r => r.idTurno == request.IdTurno))
                {
                    return BadRequest(new { mensaje = "Ya reservaste este turno." });
                }

                // Verificar superposición de fecha y hora con otro turno distinto
                var reservaSuperpuesta = reservasUsuario.FirstOrDefault(r => 
                {
                    var t = turnoList.FirstOrDefault(x => x.Id == r.idTurno);
                    if (t == null) return false;
                    // Mismo día y misma hora de inicio
                    return t.Fecha.Date == turno.Fecha.Date && t.horaInicio == turno.horaInicio;
                });

                if (reservaSuperpuesta != null)
                {
                    return BadRequest(new { mensaje = "Ya tenés un turno reservado en esa fecha y horario" });
                }

                if (turno.Precio > 0 && user.Creditos == 0)
                {
                    return Ok(new { mensaje = "Requiere pago" });
                }

                bool pagoRealizado = false;
                string mensaje = "";

                // Lógica de créditos o gratis
                if (turno.Precio > 0 && user.Creditos > 0)
                {
                    pagoRealizado = true;
                    user.Creditos--;
                    await _userManager.UpdateAsync(user);
                    mensaje = "Reserva lista! Se usó el crédito disponible";
                }
                else
                {
                    pagoRealizado = true; // Es gratis
                    mensaje = "Reserva generada con éxito.";
                }

                // Descontar cupo
                turno.cupo--;
                await _repositorioTurno.ModificarTurno(turno, turno.Id);

                // Crear reserva
                double montoFijo = turno.Precio; // Ya que el turno tiene Precio, mejor usarlo (opcional, pero la HU pedía usar el titulo, usaré el monto del turno también o dejo 2000)
                var tituloReserva = turno.nombreTurno;
                var nuevaReserva = new Reserva(Guid.Parse(user.Id), turno.Id, pagoRealizado, montoFijo, tituloReserva);

                await _reservaAltaUseCase.Ejecutar(nuevaReserva);

                return Ok(new { mensaje = mensaje, reserva = nuevaReserva });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPost("{id:guid}/cancelar")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            try
            {
                var reserva = await _repositorioReserva.buscarReserva(id);
                if (reserva == null) return NotFound(new { mensaje = "Reserva no encontrada." });
                if (reserva.eliminada) return BadRequest(new { mensaje = "La reserva ya ha sido cancelada previamente." });

                var turnoList = await _repositorioTurno.ListarTurnos();
                var turno = turnoList.FirstOrDefault(t => t.Id == reserva.idTurno);
                var user = await _userManager.FindByIdAsync(reserva.idUsuario.ToString());

                if (turno == null || user == null) return NotFound(new { mensaje = "Turno o usuario no encontrado." });

                // Calcular horas de antelación
                var fechaTurno = turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan());
                if (fechaTurno <= DateTime.Now)
                {
                    return BadRequest(new { mensaje = "No se puede cancelar una reserva de un turno que ya pasó." });
                }

                var horasAnticipacion = (fechaTurno - DateTime.Now).TotalHours;

                bool estabaSuspendido = user.Suspendido;
                string mensajeBase = "Reserva cancelada exitosamente.";
                string advertencia = null;
                
                if (!estabaSuspendido)
                {
                    if (horasAnticipacion >= 48)
                    {
                        mensajeBase += " Seña devuelta.";
                    }
                }

                user.CancelacionesMes++;

                if (!estabaSuspendido && user.CancelacionesMes >= 3)
                {
                    user.Suspendido = true;
                    advertencia = "Se cancelaron 3 reservas en un mes, tu cuenta fue suspendida. Ya no es posible reservar más clases hasta el mes siguiente y no se devolverá el valor de las señas depositadas en caso de cancelar.";
                }

                await _userManager.UpdateAsync(user);

                // Devolver cupo
                turno.cupo++;
                await _repositorioTurno.ModificarTurno(turno, turno.Id);

                // Eliminar reserva
                await _reservaBajaUseCase.Ejecutar(id);

                return Ok(new { mensaje = mensajeBase, advertencia = advertencia });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // DELETE: api/Reservas/{id}
        // Endpoint para dar de baja (eliminar) una reserva ya existente a partir de su ID.
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> EliminarReserva(Guid id)
        {
            try
            {
                // Se invoca el caso de uso que elimina la reserva de la base de datos
                await _reservaBajaUseCase.Ejecutar(id);
                
                // Retornamos HTTP 204 No Content para indicar que la operación fue exitosa, pero no hay contenido de respuesta.
                return NoContent(); 
            }
            catch (EntidadNotFoundException ex)
            {
                // Si la reserva no existe y no puede eliminarse, se devuelve HTTP 404 Not Found
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }

    // DTO (Data Transfer Object) para recibir únicamente los datos requeridos para la creación de una reserva.
    // Esto previene un "over-posting" y permite desacoplar los modelos de Dominio de las peticiones HTTP.
    public class CrearReservaRequest
    {
        public Guid IdUsuario { get; set; }
        public Guid IdTurno { get; set; }
        public bool Paga { get; set; }
        public double Monto { get; set; }
        public string Titulo { get; set; }
    }
}
