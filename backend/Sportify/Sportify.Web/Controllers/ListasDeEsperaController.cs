using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Aplicacion;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListasDeEsperaController : ControllerBase
    {
        private readonly EntrarListaTurnoUseCase _entrarListaTurnoUseCase;
        private readonly UserManager<UsuarioIdentity> _userManager;

        public ListasDeEsperaController(
            EntrarListaTurnoUseCase entrarListaTurnoUseCase,
            UserManager<UsuarioIdentity> userManager)
        {
            _entrarListaTurnoUseCase = entrarListaTurnoUseCase;
            _userManager = userManager;
        }

        [HttpPost("entrar")]
        public async Task<IActionResult> Entrar([FromBody] EntrarListaEsperaRequest request)
        {
            try
            {
                // Buscamos al usuario por su email
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return NotFound(new { mensaje = "Usuario no encontrado." });
                }

                Guid idUsuario = Guid.Parse(user.Id);

                // Llamamos al UseCase
                await _entrarListaTurnoUseCase.Ejecutar(idUsuario, request.IdTurno);

                return Ok(new { mensaje = "Te uniste a la lista de espera. Cuando sea tu turno, vas a tener 2 horas para confirmar tu asistencia." });
            }
            catch (EntidadNotFoundException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }

    public class EntrarListaEsperaRequest
    {
        public string Email { get; set; }
        public Guid IdTurno { get; set; }
    }
}
