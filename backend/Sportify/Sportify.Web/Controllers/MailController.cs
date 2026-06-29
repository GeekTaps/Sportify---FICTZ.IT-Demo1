using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.Mails;
using System.Text.Json;

[ApiController]
[Route("api/mails")]
public class MailController : ControllerBase
{
    private readonly IServicioEmail servicioEmail;

    public MailController(IServicioEmail servicioEmail)
    {
        this.servicioEmail = servicioEmail;
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
}