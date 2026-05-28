using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Usuario;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/usuarios")]
public class ModificarUsuarioController : ControllerBase
{
    private readonly modificarUsuarioUseCase actualizarUsuarioUseCase;

    public ModificarUsuarioController(modificarUsuarioUseCase actualizarUsuarioUseCase)
    {
        this.actualizarUsuarioUseCase = actualizarUsuarioUseCase;
    }

 
    [HttpPatch("me")]
    public async Task<IActionResult> UpdateMe([FromBody] Usuario dto)
    {
        try
        {
        
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (identityId == null)
                return Unauthorized(new { message = "No estás autenticado" });

            await actualizarUsuarioUseCase.Ejecutar(identityId, dto);

            return Ok(new
            {
                message = "Usuario actualizado correctamente"
            });
        }
        catch (ValidacionException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}