using Microsoft.AspNetCore.Mvc;
using Sportify.Dominio.Usuario;
using Sportify.Aplicacion.Excepciones;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;
using Sportify.Web.DTOs;
using Sportify.Aplicacion.AplicacionUsuarios;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly RegistrarUsuarioUseCase registrarUsuarioUseCase;
    private readonly UserManager<UsuarioIdentity> userManager;
    private readonly ReactivarAlumnoUseCase reactivarAlumnoUseCase;
    private readonly ListarUsuariosSuspendidosUseCase listarUsuariosSuspendidosUseCase;
    private readonly ListarUsuariosEnListaEsperaAbonoUseCase listarUsuariosEnListaDeEsperaAbonoUseCase;
    private readonly ListarUsuariosEnListaEsperaTurnoUseCase listarUsuariosEnListaDeEsperaTurnoUseCase;
    private readonly SuspenderCuentaUseCase suspenderCuentaUseCase;

    public UsuariosController(
        RegistrarUsuarioUseCase registrarUsuarioUseCase,
        UserManager<UsuarioIdentity> userManager,
        ReactivarAlumnoUseCase reactivarAlumnoUseCase,
        ListarUsuariosSuspendidosUseCase listarUsuariosSuspendidosUseCase,
        ListarUsuariosEnListaEsperaAbonoUseCase listarUsuariosEnListaDeEsperaUseCase,
        ListarUsuariosEnListaEsperaTurnoUseCase listarUsuariosEnListaDeEsperaTurnoUseCase,
        SuspenderCuentaUseCase suspenderCuentaUseCase)
    {
        this.registrarUsuarioUseCase = registrarUsuarioUseCase;
        this.userManager = userManager;
        this.reactivarAlumnoUseCase = reactivarAlumnoUseCase;
        this.listarUsuariosSuspendidosUseCase = listarUsuariosSuspendidosUseCase;
        this.listarUsuariosEnListaDeEsperaAbonoUseCase = listarUsuariosEnListaDeEsperaUseCase;
        this.listarUsuariosEnListaDeEsperaTurnoUseCase = listarUsuariosEnListaDeEsperaTurnoUseCase;
        this.suspenderCuentaUseCase = suspenderCuentaUseCase;
    }

    [HttpGet]
    public IActionResult ListarUsuarios()
    {
        var usuarios = userManager.Users
            .Where(u => !u.EsAdmin)
            .Select(user => new
            {
                id = user.Id,
                email = user.Email,
                nombreCompleto = user.NombreCompleto,
                esAdmin = user.EsAdmin,
                dni = user.Dni,
                fechaNacimiento = user.FechaNacimiento,
                suspendido = user.Suspendido,
                suspendidoPermanente = user.SuspendidoPermanente,
            }).ToList();

        return Ok(usuarios);
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
                "",
                dto.Password,
                dto.FechaNacimiento
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
            suspendido = user.Suspendido,
            suspendidoPermanente = user.SuspendidoPermanente,
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return BadRequest(new { message = "Usuario o contraseña incorrectos" });
        }

        if (user.Borrado)
        {
            return BadRequest(new { message = "Su cuenta ha sido eliminada" });
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
            suspendidoPermanente = user.SuspendidoPermanente,
            esAdmin = user.EsAdmin,
            esEmpleado = user.EsEmpleado
        });
    }

    [HttpGet("suspendidos")]
    public IActionResult ListarSuspendidos()
    {
        var usuarios = userManager.Users
            .Where(u => (u.Suspendido || u.SuspendidoPermanente) && !u.EsAdmin)
            .Select(user => new
            {
                id = user.Id,
                email = user.Email,
                nombreCompleto = user.NombreCompleto,   
                esAdmin = user.EsAdmin,
                dni = user.Dni,
                fechaNacimiento = user.FechaNacimiento,
                suspendido = user.Suspendido,
                suspendidoPermanente = user.SuspendidoPermanente
            }).ToList();

        return Ok(usuarios);
    }

    [HttpPost("reactivar/{email}")]
    public async Task<IActionResult> Reactivar(string email)
    {
        try
        {
            await reactivarAlumnoUseCase.Ejecutar(email);
            return Ok(new { message = "Alumno reactivado correctamente." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("suspender/{email}")]
    public async Task<IActionResult> SuspenderPermanente(string email, [FromQuery] bool cancelarReservas = false)
    {
        try
        {
            await suspenderCuentaUseCase.Ejecutar(email, cancelarReservas);
            
            string mensaje = cancelarReservas 
                ? "Usuario suspendido. Reservas canceladas." 
                : "Usuario suspendido.";
                
            return Ok(new { message = mensaje });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("lista-espera-abono/{idDeporte}")]
    public async Task<IActionResult> ListarListaEsperaAbono(Guid idDeporte)
    {
        var usuarios = await listarUsuariosEnListaDeEsperaAbonoUseCase.Ejecutar(idDeporte);
        return Ok(usuarios);
    }

    [HttpGet("lista-espera-turno/{idTurno}")]
    public async Task<IActionResult> ListarListaEsperaTurno(Guid idTurno)
    {
        var usuarios = await listarUsuariosEnListaDeEsperaTurnoUseCase.Ejecutar(idTurno);
        return Ok(usuarios);
    }

}