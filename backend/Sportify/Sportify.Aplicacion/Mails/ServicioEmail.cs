
using Sportify.Aplicacion;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Sportify.Aplicacion;

public class ServicioEmail : IServicioEmail
{
    
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