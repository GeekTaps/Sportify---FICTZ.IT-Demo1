using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion;
using Sportify.Aplicacion.Excepciones;
using Sportify.Aplicacion.Mails;
using Microsoft.AspNetCore.Identity;
using Sportify.Web.DTOs;
using Sportify.Infraestructura.Identity;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UsuarioIdentity> _userManager;
    private readonly IServicioEmail _servicioEmail;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<UsuarioIdentity> userManager,
        IServicioEmail servicioEmail,
        IConfiguration config)
    {
        _userManager = userManager;
        _servicioEmail = servicioEmail;
        _config = config;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> OlvideContrasena([FromBody] OlvideMiContraseñaDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Ok(new { message = "Si el correo existe, recibirás un email." });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Uri.EscapeDataString(token);

        var frontendUrl = _config["AppSettings:FrontendUrl"];
        var link = $"{frontendUrl}/reset-password?email={dto.Email}&token={encodedToken}";

        var body = $"""
            <h2>Recuperación de contraseña - Sportify</h2>
            <p>Hacé click acá para restablecer tu contraseña:</p>
            <a href="{link}">Restablecer contraseña</a>
            <p>Si no fuiste vos, ignorá este mail.</p>
            """;

        await _servicioEmail.MandarMail(dto.Email, "Recuperar contraseña", body);

        return Ok(new { message = "Si el correo existe, recibirás un email." });
    }
    [HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword([FromBody] ResetearContraseñaDTO dto)
{
    var user = await _userManager.FindByEmailAsync(dto.Email);
    if (user == null)
        return BadRequest(new { message = "Solicitud inválida." });

    var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);

  if (!result.Succeeded)
{
    var errores = result.Errors.Select(e => e.Code switch
    {
        "PasswordTooShort" => "La contraseña debe tener al menos 6 caracteres.",
        "InvalidToken" => "El token es inválido o expiró.",
        _ => e.Description
    });
    return BadRequest(new { message = string.Join(" ", errores) });
}

    return Ok(new { message = "Contraseña restablecida correctamente." });
}
}