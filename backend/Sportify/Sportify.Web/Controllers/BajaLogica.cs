using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.Excepciones;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/usuarios")]
public class BajaLogicaUsuariosController : ControllerBase
{
    private readonly BajaLogicaUsuarioUseCase bajaLogicaUsuarioUseCase;

    public BajaLogicaUsuariosController(BajaLogicaUsuarioUseCase bajaLogicaUsuarioUseCase)
    {
        this.bajaLogicaUsuarioUseCase = bajaLogicaUsuarioUseCase;
    }

    [HttpPost("{id}/baja")]
    public async Task<IActionResult> BajaLogica(string id)
    {
        try
        {
            await bajaLogicaUsuarioUseCase.Ejecutar(id);

            return Ok(new { message = "Usuario dado de baja correctamente" });
        }
        catch (ValidacionException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
        }
    }
}