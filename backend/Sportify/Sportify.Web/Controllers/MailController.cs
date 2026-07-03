using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.Mails;
using System.Text.Json;
using Sportify.Aplicacion.AplicacionUsuarios;

[ApiController]
[Route("api/mails")]
public class MailController : ControllerBase
{
    private readonly IServicioEmail servicioEmail;
    private readonly ListarMailsDeUsuariosConPagosPendientesUseCase listarMailsUseCase;

    public MailController(IServicioEmail servicioEmail, ListarMailsDeUsuariosConPagosPendientesUseCase listarMailsUseCase)
    {
        this.servicioEmail = servicioEmail;
        this.listarMailsUseCase = listarMailsUseCase;
    }

    [HttpPost("enviar")]
    public async Task<IActionResult> Enviar([FromBody] JsonElement datos)
    {
        string asunto = datos.GetProperty("asunto").GetString()!;
        string cuerpo = datos.GetProperty("cuerpo").GetString()!;

        foreach (var mail in datos.GetProperty("mails").EnumerateArray())
        {
            await servicioEmail.MandarMail(
                mail.GetString()!,
                asunto,
                cuerpo);
        }

        return Ok();
    }

    [HttpPost("recordatorio-pagos")]
public async Task<IActionResult> EnviarRecordatorioPagos()
{
    var mails = await listarMailsUseCase.Ejecutar();

    var body = """
        <h2>Recordatorio de pago - Sportify</h2>
        <p>Te recordamos que tenés pagos pendientes.</p>
        <p>Por favor acordate de abonarlos.</p>
        <p>Gracias por elegir Sportify.</p>
        """;

    foreach (var mail in mails)
    {
        await servicioEmail.MandarMail(
            mail,
            "Recordatorio de pago",
            body);
    }

    return Ok();
}
}