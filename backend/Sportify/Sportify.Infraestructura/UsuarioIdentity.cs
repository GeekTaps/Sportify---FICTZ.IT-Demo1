using Microsoft.AspNetCore.Identity;

namespace Sportify.Infraestructura.Identity;

public class UsuarioIdentity : IdentityUser
{
    public string NombreCompleto { get; set; } = "";
}