using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Usuario;
using Sportify.Aplicacion.Excepciones;
namespace Sportify.Web.Controllers;
using Sportify.Web.DTOs;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly RegistrarUsuarioUseCase registrarUsuarioUseCase;

    public UsuariosController(RegistrarUsuarioUseCase registrarUsuarioUseCase)
    {
        this.registrarUsuarioUseCase = registrarUsuarioUseCase;
    }

[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegistrarUsuarioDTO dto)
{
    try
    {
        Usuario usuario = new Usuario(
            dto.NombreCompleto,
            dto.Email,
            dto.Dni,
            dto.Password,
            dto.Edad
        );

        await registrarUsuarioUseCase.Ejecutar(usuario);

        return Ok(new
        {
            message = "Usuario registrado correctamente"
        });
    }
    catch (ValidacionException ex)
    {
        return BadRequest(new
        {
            message = ex.Message
        });
    }
}
}