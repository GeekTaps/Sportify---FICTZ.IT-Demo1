using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using Sportify.Aplicacion.AplicacionAbonos;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/abonos")]
public class AbonosController : ControllerBase
{
    private readonly ObtenerInfoAbonoUseCase _obtenerInfoAbonoUseCase;
    private readonly AbonarUseCase _abonarUseCase;
    private readonly IConfiguration _configuration;

    public AbonosController(
        ObtenerInfoAbonoUseCase obtenerInfoAbonoUseCase, 
        AbonarUseCase abonarUseCase,
        IConfiguration configuration)
    {
        _obtenerInfoAbonoUseCase = obtenerInfoAbonoUseCase;
        _abonarUseCase = abonarUseCase;
        _configuration = configuration;
        MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetInfoAbono([FromQuery] Guid idTurno, [FromQuery] string email)
    {
        try
        {
            var response = await _obtenerInfoAbonoUseCase.Ejecutar(idTurno, email);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // HARDCODEO DE PAGOS: este endpoint registra el abono localmente sin salir a Mercado Pago.
    [HttpPost("procesar-pago-local")]
    public async Task<IActionResult> ProcesarPagoLocal([FromBody] AbonoPreferenceRequest request)
    {
        try
        {
            var info = await _obtenerInfoAbonoUseCase.Ejecutar(request.IdTurno, request.Email);

            if (info.IsAlreadySubscribed || info.HasConflict || info.NoCupo)
            {
                return BadRequest(new { message = "No es posible procesar el pago debido al estado del abono." });
            }

            decimal monto = (decimal)info.PrecioTotal;
            await _abonarUseCase.Ejecutar(request.Email, request.IdTurno, monto);

            return Ok(new { mensaje = "Pago registrado correctamente.", monto });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al procesar el pago local", error = ex.Message });
        }
    }

    [HttpPost("pagar-cuota-local")]
    public async Task<IActionResult> PagarCuotaLocal([FromBody] AbonoPreferenceRequest request, [FromServices] PagarCuotaAbonoUseCase pagarCuotaAbonoUseCase)
    {
        try
        {
            // Opcionalmente podemos pasar el monto si lo recibiéramos, pero el caso de uso lo calcula si le pasamos null
            await pagarCuotaAbonoUseCase.Ejecutar(request.Email, request.IdTurno, null);
            return Ok(new { mensaje = "Abono confirmado correctamente." });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en PagarCuotaLocal: " + ex.ToString());
            return StatusCode(500, new { message = "Error al procesar la cuota del abono", error = ex.Message });
        }
    }

    /*
    // HARDCODEO DE PAGOS: bloque original de Mercado Pago preservado como referencia.
    [HttpPost("crear-preferencia")]
    public async Task<IActionResult> CrearPreferencia([FromBody] AbonoPreferenceRequest request)
    {
        if (string.IsNullOrEmpty(MercadoPagoConfig.AccessToken) || MercadoPagoConfig.AccessToken == "PEGÁ_ACÁ_TU_ACCESS_TOKEN")
        {
            return BadRequest(new { message = "Las credenciales de Mercado Pago no están configuradas." });
        }

        try
        {
            var info = await _obtenerInfoAbonoUseCase.Ejecutar(request.IdTurno, request.Email);

            if (info.IsAlreadySubscribed || info.HasConflict || info.NoCupo)
            {
                return BadRequest(new { message = "No es posible crear la preferencia debido al estado del abono." });
            }

            // Si el precio es cero (por uso completo de créditos), no hace falta mercado pago, pero el FE debería atajarlo.
            if (info.PrecioTotal <= 0)
            {
                return Ok(new { preferenceId = "precio_cero" });
            }

            var requestPref = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = $"Abono Actividad: {info.Actividad}",
                        Quantity = 1,
                        CurrencyId = "ARS",
                        UnitPrice = (decimal)info.PrecioTotal,
                    }
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = $"https://redirectmeto.com/http://localhost:5266/api/abonos/retorno?status=approved&idTurno={request.IdTurno}&email={request.Email}", 
                    Failure = $"https://redirectmeto.com/http://localhost:5266/api/abonos/retorno?status=failure&idTurno={request.IdTurno}&email={request.Email}",
                    Pending = $"https://redirectmeto.com/http://localhost:5266/api/abonos/retorno?status=pending&idTurno={request.IdTurno}&email={request.Email}"
                },
                AutoReturn = "approved", 
                ExternalReference = $"{request.IdTurno}|{request.Email}"
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(requestPref);
            return Ok(new { preferenceId = preference.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al comunicarse con Mercado Pago", error = ex.Message });
        }
    }
    */

    [HttpGet("retorno")]
    public async Task<IActionResult> Retorno(string status, Guid idTurno, string email)
    {
        if (status == "approved")
        {
            try
            {
                await _abonarUseCase.Ejecutar(email, idTurno);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al crear el abono post-pago: " + ex.Message);
                return Redirect("http://localhost:7001/turnos?abono=error_interno");
            }
            return Redirect("http://localhost:7001/turnos?abono=exitoso");
        }
        else
        {
            return Redirect("http://localhost:7001/turnos?abono=rechazado");
        }
    }
}

public class AbonoPreferenceRequest
{
    public Guid IdTurno { get; set; }
    public string Email { get; set; }
}
