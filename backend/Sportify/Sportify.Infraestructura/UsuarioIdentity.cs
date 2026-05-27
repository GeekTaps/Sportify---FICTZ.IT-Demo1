using Microsoft.AspNetCore.Identity;

namespace Sportify.Infraestructura.Identity;
// hola amigos debido a los multiples cambios que hay que hacerle a identity user (propiedad de entity framework)
// no se puede correr de forma normal el proyecto, o sea salta error por todos lados
//busque al respecto y tipo como que hay que poner unos comandos para que el entity framework se actualice con los cambios que le hiciste a identity user
//  si no les anda nada usen estos comandos:
// dotnet tool install --global dotnet-ef 
// dotnet ef migrations add UpdateUsuarioIdentity --project Sportify.Infraestructura --startup-project Sportify.Web
// dotnet ef database update --project Sportify.Infraestructura --startup-project Sportify.Web
//una paja pero si queremos usar entity framework hay que hacerlo
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