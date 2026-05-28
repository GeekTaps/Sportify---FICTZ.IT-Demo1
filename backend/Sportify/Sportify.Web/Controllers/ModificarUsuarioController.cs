using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Usuario;
using Sportify.Web.DTOs;

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
[HttpGet("{id}")]
public async Task<IActionResult> GetUsuario([FromRoute] string id)
{
    try
    {
        Usuario usuario = await actualizarUsuarioUseCase.ObtenerPorId(id);

        return Ok(new
        {
            nombreCompleto = usuario.NombreCompleto,
            email = usuario.Mail,
            dni = usuario.Dni,
            edad = usuario.Edad
        });
    }
    catch (ValidacionException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
 
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ModificarUsuarioDTO dto)
    {
        try
        {
            Usuario usuario = new Usuario(
                dto.NombreCompleto ?? "",
                dto.Email ?? "",
                dto.Dni ?? "",
                dto.Password ?? "",
                dto.Edad ?? ""
            );

            await actualizarUsuarioUseCase.Ejecutar(id, usuario);

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