namespace Sportify.Aplicacion.Mails;

public interface IServicioEmail

{
    Task MandarMail(string toEmail, string subject, string htmlBody);
}