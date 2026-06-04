using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionPagos;
using Sportify.Dominio.Pagos;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositorioTurno _repositorioTurno;
        private readonly IRepositorioReserva _repositorioReserva;
        private readonly ReservaAltaUseCase _reservaAltaUseCase;
        private readonly RegistrarPagoUseCase _registrarPagoUseCase;
        private readonly UserManager<UsuarioIdentity> _userManager;

        public PagosController(
            IConfiguration configuration,
            IRepositorioTurno repositorioTurno,
            IRepositorioReserva repositorioReserva,
            ReservaAltaUseCase reservaAltaUseCase,
            RegistrarPagoUseCase registrarPagoUseCase,
            UserManager<UsuarioIdentity> userManager)
        {
            _configuration = configuration;
            _repositorioTurno = repositorioTurno;
            _repositorioReserva = repositorioReserva;
            _reservaAltaUseCase = reservaAltaUseCase;
            _registrarPagoUseCase = registrarPagoUseCase;
            _userManager = userManager;
            
            // Inicializar MercadoPago
            MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
        }

        [HttpPost("crear-preferencia")]
        public async Task<IActionResult> CrearPreferencia([FromBody] PagoRequest request)
        {
            if (string.IsNullOrEmpty(MercadoPagoConfig.AccessToken) || MercadoPagoConfig.AccessToken == "PEGÁ_ACÁ_TU_ACCESS_TOKEN")
            {
                return BadRequest(new { message = "Las credenciales de Mercado Pago no están configuradas." });
            }

            var turnoExiste = await _repositorioTurno.BuscarTurnoPorId(request.IdTurno);
            if (!turnoExiste) return NotFound(new { message = "Turno no encontrado." });
            
            var listTurnos = await _repositorioTurno.ListarTurnos();
            var turno = listTurnos.Find(t => t.Id == request.IdTurno);

            // Calcular el 50% de seña
            decimal montoSeña = (decimal)(turno.Precio * 0.5);

            var requestPref = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = $"Seña: {turno.nombreTurno}",
                        Quantity = 1,
                        CurrencyId = "ARS",
                        UnitPrice = montoSeña,
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = $"https://redirectmeto.com/http://localhost:5266/api/pagos/retorno?status=approved&idTurno={request.IdTurno}&email={request.Email}", 
                    Failure = $"https://redirectmeto.com/http://localhost:5266/api/pagos/retorno?status=failure&idTurno={request.IdTurno}&email={request.Email}",
                    Pending = $"https://redirectmeto.com/http://localhost:5266/api/pagos/retorno?status=pending&idTurno={request.IdTurno}&email={request.Email}"
                },
                AutoReturn = "approved", 
                ExternalReference = $"{request.IdTurno}|{request.Email}"
            };

            try
            {
                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(requestPref);
                return Ok(new { preferenceId = preference.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al comunicarse con Mercado Pago", error = ex.Message });
            }
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPago([FromBody] RegistrarPagoRequest request)
        {
            try
            {
                if (request.IdUsuario == Guid.Empty || request.IdReserva == Guid.Empty)
                {
                    return BadRequest(new { message = "Debe seleccionar un usuario y una reserva válidos." });
                }

                var reserva = await _repositorioReserva.buscarReserva(request.IdReserva);
                if (reserva == null)
                {
                    return NotFound(new { message = "Reserva no encontrada." });
                }

                if (reserva.idUsuario != request.IdUsuario)
                {
                    return BadRequest(new { message = "El usuario seleccionado no posee esa reserva." });
                }

                if (reserva.paga)
                {
                    return BadRequest(new { message = "esta reserva ya fue pagada" });
                }

                var pago = new Pago(reserva.id, request.IdUsuario, (decimal)reserva.monto);
                await _registrarPagoUseCase.Ejecutar(pago);

                return Ok(new { message = "Pago registrado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al registrar el pago.", detail = ex.Message });
            }
        }

        [HttpGet("retorno")]
        public async Task<IActionResult> Retorno(string status, Guid idTurno, string email)
        {
            if (status == "approved")
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user != null)
                    {
                        var listTurnos = await _repositorioTurno.ListarTurnos();
                        var turno = listTurnos.Find(t => t.Id == idTurno);
                        if (turno != null && turno.cupo > 0)
                        {
                            // Crear reserva
                            double montoFijo = turno.Precio;
                            var nuevaReserva = new Sportify.Dominio.Reservas.Reserva(Guid.Parse(user.Id), turno.Id, false, montoFijo, turno.nombreTurno);
                            await _reservaAltaUseCase.Ejecutar(nuevaReserva);

                            // Descontar cupo
                            turno.cupo--;
                            await _repositorioTurno.ModificarTurno(turno, turno.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al crear la reserva post-pago: " + ex.Message);
                    return Redirect("http://localhost:7001/reservas?pago=error_interno");
                }
                return Redirect("http://localhost:7001/reservas?pago=exitoso");
            }
            else
            {
                return Redirect("http://localhost:7001/reservas?pago=rechazado");
            }
        }
    }

    public class PagoRequest
    {
        public Guid IdTurno { get; set; }
        public string Email { get; set; }
    }

    public class RegistrarPagoRequest
    {
        public Guid IdUsuario { get; set; }
        public Guid IdReserva { get; set; }
    }
}
