namespace Application.Interfaces;

public interface IServicioEmail

{
    Task MandarMail(string toEmail, string subject, string htmlBody);
}