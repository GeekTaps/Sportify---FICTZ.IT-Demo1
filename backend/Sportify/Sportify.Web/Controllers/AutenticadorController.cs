using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion;
using Sportify.Aplicacion.Excepciones;
using Microsoft.AspNetCore.Identity;
using Sportify.Web;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IServicioEmail _servicioEmail;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<IdentityUser> userManager,
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
}