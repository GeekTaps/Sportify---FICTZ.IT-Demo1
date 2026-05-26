using Microsoft.AspNetCore.Identity;

namespace Sportify.Infraestructura.Identity;

public class UsuarioIdentity : IdentityUser
{
    public string NombreCompleto { get; set; } = "";
    public string Edad { get; set; } = "";
    public string Dni { get; set; } = "";
    public bool Borrado { get; set; } = false;

    public UsuarioIdentity()
    {
    }
}