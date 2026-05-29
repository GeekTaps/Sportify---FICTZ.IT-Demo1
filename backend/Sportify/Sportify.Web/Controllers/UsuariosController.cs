using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Usuario;
using Sportify.Aplicacion.Excepciones;
namespace Sportify.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;
using Sportify.Web.DTOs;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly RegistrarUsuarioUseCase registrarUsuarioUseCase;
    private readonly UserManager<UsuarioIdentity> userManager;

    public UsuariosController(RegistrarUsuarioUseCase registrarUsuarioUseCase, UserManager<UsuarioIdentity> userManager)
    {
        this.registrarUsuarioUseCase = registrarUsuarioUseCase;
        this.userManager = userManager;
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

[HttpGet("info/{email}")]
public async Task<IActionResult> GetUserInfo(string email)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        return NotFound(new { message = "Usuario no encontrado" });
    }

    return Ok(new
    {
        email = user.Email,
        suspendido = user.Suspendido
    });
}

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDTO dto)
{
    var user = await userManager.FindByEmailAsync(dto.Email);
    if (user == null || user.Borrado)
    {
        return BadRequest(new { message = "Usuario o contraseña incorrectos" });
    }

    var isPasswordValid = await userManager.CheckPasswordAsync(user, dto.Password);
    if (!isPasswordValid)
    {
        return BadRequest(new { message = "Usuario o contraseña incorrectos" });
    }

    return Ok(new
    {
        id = user.Id,
        email = user.Email,
        nombreCompleto = user.NombreCompleto,
        suspendido = user.Suspendido,
        esAdmin = user.EsAdmin
    });
}
}