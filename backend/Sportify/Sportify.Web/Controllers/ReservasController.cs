using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;

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

        // El constructor recibe los casos de uso inyectados automáticamente por el contenedor de dependencias de ASP.NET (configurado en Program.cs)
        public ReservasController(
            ReservaAltaUseCase reservaAltaUseCase,
            ReservaBajaUseCase reservaBajaUseCase,
            ReservaBusquedaUseCase reservaBusquedaUseCase,
            ReservaListadoUseCase reservaListadoUseCase,
            UserManager<UsuarioIdentity> userManager)
        {
            _reservaAltaUseCase = reservaAltaUseCase;
            _reservaBajaUseCase = reservaBajaUseCase;
            _reservaBusquedaUseCase = reservaBusquedaUseCase;
            _reservaListadoUseCase = reservaListadoUseCase;
            _userManager = userManager;
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
