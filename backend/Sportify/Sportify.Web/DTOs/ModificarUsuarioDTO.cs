namespace Sportify.Web.DTOs
{
    public class ModificarUsuarioDTO
    {
        public string? NombreCompleto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Dni { get; set; }
        public string? Email { get; set; }
            public string? PasswordActual { get; set; }
    public string? PasswordNueva { get; set; }
    public string? ConfirmarPassword { get; set; }
    }
}
