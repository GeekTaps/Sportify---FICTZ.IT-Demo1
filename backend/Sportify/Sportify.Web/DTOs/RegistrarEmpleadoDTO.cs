namespace Sportify.Web.DTOs;

public class RegistrarEmpleadoDTO
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
}