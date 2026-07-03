namespace Sportify.Web.DTOs;

public class AbonoInfoResponse
{
    public bool IsAlreadySubscribed { get; set; }
    public bool HasConflict { get; set; }
    public bool IsPast10thDay { get; set; }
    public bool HasFewClasses { get; set; }
    public bool NoCupo { get; set; }
    
    public string Actividad { get; set; } = string.Empty;
    public string Horario { get; set; } = string.Empty;
    public double PrecioTotal { get; set; }
    public double DescuentoAplicado { get; set; }
}
