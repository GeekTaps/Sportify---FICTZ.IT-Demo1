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

 
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] Usuario dto)
    {
        try
        {
            await actualizarUsuarioUseCase.Ejecutar(id, dto);

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