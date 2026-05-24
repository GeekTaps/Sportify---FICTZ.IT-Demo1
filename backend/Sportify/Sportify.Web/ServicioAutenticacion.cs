using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Infraestructura.Identity;
//OK OK OK ACEPTO PREGUNTAS esto fue hecho entre chatgpt y un fokin tutorial de yt (adjunto link): https://www.youtube.com/watch?v=GF7jm9EQQ8Y&t=311s
//en teoria este madafaka tiene operaciones como sign in (tomi te interesa esa) y sign ou (usada aca)
//aparte de toooda una lista enorme encontrada aca en esta pagina (adjunto link) https://learn.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1?view=aspnetcore-9.0
//cuestion, no lo probé, hasta que  no haya inicio de sesion claro... pero dejemos esto aca como esqueleto del sign out confio en esto del identity recemos

public class ServicioAutenticacion : IServicioAutenticacion
{
    private readonly SignInManager<UsuarioIdentity> signInManager;

    public ServicioAutenticacion(SignInManager<UsuarioIdentity> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }
}