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
<<<<<<< HEAD
    public string NombreCompleto { get; set; } = "";
    public string Edad { get; set; } = "";
    public string Dni { get; set; } = "";
    public bool Borrado { get; set; } = false;

    public UsuarioIdentity()
    {
=======
    // Parameterless constructor required by EF Core for materialization
    protected UsuarioIdentity() { }

    public string NombreCompleto { get; private set; } = "";
    public string Contraseña { get; private set; } = "";
    public string Edad { get; private set; } = "";
    public string Dni { get; private set; } = "";
    public bool Borrado { get; private set; }

    public UsuarioIdentity(string nombre, string contraseña, string mail, string edad, string dni)
    {
        this.NombreCompleto = nombre;
        this.Contraseña = contraseña;
        this.Email = mail;
        this.Edad = edad;
        this.Dni = dni;
        this.Borrado = false;
>>>>>>> origin/development
    }
}