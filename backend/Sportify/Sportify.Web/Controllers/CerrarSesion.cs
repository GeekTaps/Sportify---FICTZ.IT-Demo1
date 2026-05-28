using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionUsuarios;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly CerrarSesionUsuarioUseCase cerrarSesionUsuarioUseCase;

    public AuthController(CerrarSesionUsuarioUseCase cerrarSesionUsuarioUseCase)
    {
        this.cerrarSesionUsuarioUseCase = cerrarSesionUsuarioUseCase;
    }

   [HttpPost("logout")]
public async Task<IActionResult> Logout()
{
    try
    {
        await cerrarSesionUsuarioUseCase.Ejecutar();
        return Ok();
    }
    catch (Exception ex)
    {
        return StatusCode(500, ex.Message);
    }
}
}
