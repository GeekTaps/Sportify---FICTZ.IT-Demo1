using Microsoft.AspNetCore.Identity;

namespace Sportify.Infraestructura.Identity;

public class UsuarioIdentity : IdentityUser
{
    public Guid ID { get; private set; }
    public string NombreCompleto { get; private set; } = "";
    public string Contraseña {get; private set;}
    public string Mail { get; private set; }
    public string Edad { get; private set; } 
    public string Dni { get; private set; } 
    public bool Borrado { get; private set; } 

    public UsuarioIdentity (string nombre, string contraseña, string mail, string edad, string dni)
    {
        this.ID = Guid.NewGuid();
        this.NombreCompleto=nombre;
        this.Contraseña=contraseña;
        this.Mail=mail;
        this.Edad=edad;
        this.Dni=dni;
        this.Borrado=false;

    }
}