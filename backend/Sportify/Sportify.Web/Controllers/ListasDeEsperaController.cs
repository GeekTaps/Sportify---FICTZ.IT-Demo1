using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Aplicacion;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionPagos;
using Sportify.Dominio.Reservas;
using Sportify.Dominio.Pagos;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListasDeEsperaController : ControllerBase
    {
        private readonly EntrarListaTurnoUseCase _entrarListaTurnoUseCase;
        private readonly EntrarListaAbonoUseCase _entrarListaAbonoUseCase;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly IRepositorioListaDeEsperaTurno _repositorioListaDeEsperaTurno;
        private readonly IRepositorioTurno _repositorioTurno;
        private readonly IRepositorioReserva _repositorioReserva;
        private readonly ReservaAltaUseCase _reservaAltaUseCase;
        private readonly RegistrarPagoUseCase _registrarPagoUseCase;
        private readonly IConfiguration _configuration;

        public ListasDeEsperaController(
            EntrarListaTurnoUseCase entrarListaTurnoUseCase,
            EntrarListaAbonoUseCase entrarListaAbonoUseCase,
            UserManager<UsuarioIdentity> userManager,
            IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno,
            IRepositorioTurno repositorioTurno,
            IRepositorioReserva repositorioReserva,
            ReservaAltaUseCase reservaAltaUseCase,
            RegistrarPagoUseCase registrarPagoUseCase,
            IConfiguration configuration)
        {
            _entrarListaTurnoUseCase = entrarListaTurnoUseCase;
            _entrarListaAbonoUseCase = entrarListaAbonoUseCase;
            _userManager = userManager;
            _repositorioListaDeEsperaTurno = repositorioListaDeEsperaTurno;
            _repositorioTurno = repositorioTurno;
            _repositorioReserva = repositorioReserva;
            _reservaAltaUseCase = reservaAltaUseCase;
            _registrarPagoUseCase = registrarPagoUseCase;
            _configuration = configuration;
        }

        [HttpPost("entrar")]
        public async Task<IActionResult> Entrar([FromBody] EntrarListaEsperaRequest request)
        {
            try
            {
                // Buscamos al usuario por su email
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado." });
                }

                Guid idUsuario = Guid.Parse(user.Id);

                // Llamamos al UseCase
                await _entrarListaTurnoUseCase.Ejecutar(idUsuario, request.IdTurno);

                return Ok(new { mensaje = "Te uniste a la lista de espera. Cuando sea tu turno, vas a tener 2 horas para confirmar tu asistencia." });
            }
            catch (EntidadNotFoundException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPost("abono")]
        public async Task<IActionResult> EntrarAbono([FromBody] EntrarListaEsperaRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return NotFound(new { mensaje = "Usuario no encontrado." });

                Guid idUsuario = Guid.Parse(user.Id);

                await _entrarListaAbonoUseCase.Ejecutar(idUsuario, request.IdTurno, request.Email);

                return Ok(new { mensaje = "Te uniste a la lista de espera. Cuando haya cupo en esta actividad, serás notificado." });
            }
            catch (EntidadNotFoundException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpPost("confirmar-reserva")]
        public async Task<IActionResult> ConfirmarReserva([FromBody] ConfirmarReservaEsperaRequest request)
        {
            Console.WriteLine("======== CONFIRMAR RESERVA ========"); //para probar cod, si funca borrar
            Console.WriteLine($"Token: {request.Token}");
            Console.WriteLine($"IdTurno: {request.IdTurno}");
            try
            {
                if (string.IsNullOrWhiteSpace(request.Token) || request.IdTurno == Guid.Empty)
                {
                    return BadRequest(new { mensaje = "Datos inválidos para confirmar la reserva." });
                }

                var datos = LeerToken(request.Token);
                if (!datos.Valido || datos.IdTurno != request.IdTurno || string.IsNullOrWhiteSpace(datos.Email) || string.IsNullOrWhiteSpace(datos.IdUsuario))
                {
                    return BadRequest(new { mensaje = "El enlace para confirmar la reserva ya no es válido." });
                }

                var user = await _userManager.FindByEmailAsync(datos.Email);
                if (user == null) return NotFound(new { mensaje = "Usuario no encontrado." });

                Guid idUsuario = Guid.Parse(user.Id);
                if (datos.IdUsuario != user.Id)
                {
                    return BadRequest(new { mensaje = "El enlace no corresponde a este usuario." });
                }

                var turno = await _repositorioTurno.ObtenerTurnoPorId(request.IdTurno);
                if (turno == null) return NotFound(new { mensaje = "Turno no encontrado." });

                if (turno.cupo <= 0)
                {
                    return BadRequest(new { mensaje = "No hay cupo disponible para confirmar esta reserva." });
                }

                var reservasUsuario = await _repositorioReserva.listarReservasUsuario(idUsuario);
                if (reservasUsuario.Any(r => r.idTurno == request.IdTurno && !r.eliminada))
                {
                    return BadRequest(new { mensaje = "Ya tenés una reserva para este turno." });
                }

                await _repositorioListaDeEsperaTurno.eliminarEspera(idUsuario, request.IdTurno);

                turno.cupo--;
                await _repositorioTurno.ModificarTurno(turno, turno.Id);

                decimal montoSeña = turno.Precio > 0 ? Math.Round((decimal)(turno.Precio * 0.5), 2) : 0;
                var nuevaReserva = new Reserva(idUsuario, turno.Id, montoSeña > 0, (double)montoSeña, turno.nombreTurno);
                await _reservaAltaUseCase.Ejecutar(nuevaReserva);

                if (montoSeña > 0)
                {
                    var pago = new Pago(nuevaReserva.id, idUsuario, montoSeña);
                    await _registrarPagoUseCase.Ejecutar(pago);
                }

                return Ok(new { mensaje = "Reserva confirmada correctamente.", monto = montoSeña, reservaId = nuevaReserva.id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        private (bool Valido, string? IdUsuario, Guid IdTurno, string? Email) LeerToken(string token)
        {
            try
            {
                var decoded = Encoding.UTF8.GetString(Base64UrlDecode(token));

                Console.WriteLine("TOKEN DECODIFICADO:");//para probar cod, si funca borrar
                Console.WriteLine(decoded);

                var ultimoPunto = decoded.LastIndexOf('.');

                if (ultimoPunto < 0)
                {
                    Console.WriteLine("ERROR: No se encontró el separador del token");
                    return (false, null, Guid.Empty, null);
                }

                var payloadJson = decoded.Substring(0, ultimoPunto);
                var hashRecibido = decoded.Substring(ultimoPunto + 1);
                var secret = _configuration["ListaEspera:Secret"] ?? "SportifyListaEsperaSecret";
                using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
                var expectedHash = Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(payloadJson)));

                if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expectedHash), Encoding.UTF8.GetBytes(hashRecibido)))
                {
          
                    Console.WriteLine("ERROR 2: Hash distinto");
                    return (false, null, Guid.Empty, null);
                }

                var payload = JsonSerializer.Deserialize<PayloadConfirmacion>(payloadJson);
                if (payload == null || string.IsNullOrWhiteSpace(payload.userId) || string.IsNullOrWhiteSpace(payload.email) || !Guid.TryParse(payload.turnoId, out var turnoId))
                {
                    Console.WriteLine("ERROR 3: Payload inválido");
                    return (false, null, Guid.Empty, null);
                }

                Console.WriteLine($"expiresAt recibido: '{payload.expiresAt}'"); //para probar cod, si funca borrar

                if (!DateTime.TryParseExact(payload.expiresAt, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresAt))
                {
                    Console.WriteLine($"ERROR 4: Fecha inválida -> {payload.expiresAt}");
                    return (false, null, Guid.Empty, null);
                }

                return (true, payload.userId, turnoId, payload.email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (false, null, Guid.Empty, null);
            }
        }
        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }

        private static byte[] Base64UrlDecode(string value)
        {
            var padding = value.Length % 4;
            if (padding > 0)
            {
                value += new string('=', 4 - padding);
            }

            return Convert.FromBase64String(value.Replace("-", "+").Replace("_", "/"));
        }
    }

    public class PayloadConfirmacion
    {
        public string? userId { get; set; }
        public string? turnoId { get; set; }
        public string? email { get; set; }
        public string? expiresAt { get; set; }
    }

    public class EntrarListaEsperaRequest
    {
        public string Email { get; set; }
        public Guid IdTurno { get; set; }
    }

    public class ConfirmarReservaEsperaRequest
    {
        public string Token { get; set; }
        public Guid IdTurno { get; set; }
    }
}
