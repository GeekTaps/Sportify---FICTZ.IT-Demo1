
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Sportify.Aplicacion.Mails;
//dotnet add package MailKit //EJECUTAR ESOS COMANDOS
//dotnet add package MimeKit
public class ServicioEmail : IServicioEmail
{
        //Conozcan a ServicioEmail, este es mi amigucho que se encarga de enviar mails
        //Se estarán preguntando ¿Cómo envio un mail?  la cuestion es sencilla de cojones
        //si ven en la interfaz de servicio mail, servicio mail tiene UN SOLO Y PUTO METODO (mandar mail)
        //a ese metodo le pasas como parametro el destinatario (un mail, en lo posible que exista, nosotros no verificamos que exista,
        //                      ustedes veran como hacen para que el destinatario esté todo ok,
        //          en mi caso para la recuperacion de contraseña usé DTOs, no se bien por qué cloude me dijo que lo haga yo le hago caso)
        //cualquier cosa me llaman al wasap o ya que estan me envian un mail jaja entieden? sos re chistoso zega por eso sos nuestro amigo

        
    private readonly ModeloMail _settings;
    private readonly ILogger<ServicioEmail> _logger;

    public ServicioEmail(IOptions<ModeloMail> settings, ILogger<ServicioEmail> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task MandarMail(string toEmail, string subject, string htmlBody)
    {
        try
        {
            var message = BuildMessage(toEmail, subject, htmlBody);
            await SendAsync(message);

            _logger.LogInformation("Email enviado correctamente a {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar email a {Email}", toEmail);
            throw; // que el caller decida cómo manejarlo
        }
    }

    

    private MimeMessage BuildMessage(string toEmail, string subject, string htmlBody)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        return message;
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        await client.ConnectAsync(
            _settings.SmtpHost,
            _settings.SmtpPort,
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(quit: true);
    }
}