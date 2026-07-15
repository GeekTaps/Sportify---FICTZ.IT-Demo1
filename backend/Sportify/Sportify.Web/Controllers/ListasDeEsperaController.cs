using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Aplicacion;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Identity;
using Sportify.Web.DTOs;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListasDeEsperaController : ControllerBase
    {
        private readonly EntrarListaTurnoUseCase _entrarListaTurnoUseCase;
        private readonly EntrarListaAbonoUseCase _entrarListaAbonoUseCase;
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly SalirListaEsperaTurnoUseCase salirListaEsperaTurnoUseCase;
        private readonly EstaEnListaEsperaAbonoUseCase estaEnListaEsperaAbonoUseCase;
        private readonly estaEnListaEsperaTurnoUseCase estaEnListaEsperaTurnoUseCase;
        private readonly SalirListaEsperaAbonoUseCase salirListaEsperaAbonoUseCase;
        

        public ListasDeEsperaController(
            EntrarListaTurnoUseCase entrarListaTurnoUseCase,
            EntrarListaAbonoUseCase entrarListaAbonoUseCase,
            UserManager<UsuarioIdentity> userManager,
            SalirListaEsperaTurnoUseCase salirListaEsperaTurnoUseCase,
            EstaEnListaEsperaAbonoUseCase estaEnListaEsperaAbonoUseCase,
            estaEnListaEsperaTurnoUseCase estaEnListaEsperaTurnoUseCase,
            SalirListaEsperaAbonoUseCase salirListaEsperaAbonoUseCase
                    )
        {
            _entrarListaTurnoUseCase = entrarListaTurnoUseCase;
            _entrarListaAbonoUseCase = entrarListaAbonoUseCase;
            _userManager = userManager;
            this.salirListaEsperaTurnoUseCase = salirListaEsperaTurnoUseCase;
            this.estaEnListaEsperaAbonoUseCase = estaEnListaEsperaAbonoUseCase;
            this.estaEnListaEsperaTurnoUseCase = estaEnListaEsperaTurnoUseCase;
            this.salirListaEsperaAbonoUseCase = salirListaEsperaAbonoUseCase;
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

        [HttpPost("abono")]
        public async Task<IActionResult> EntrarAbono([FromBody] EntrarListaEsperaRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null) return NotFound(new { mensaje = "Usuario no encontrado." });

                Guid idUsuario = Guid.Parse(user.Id);

                await _entrarListaAbonoUseCase.Ejecutar(idUsuario, request.IdTurno, request.Email);

                return Ok(new { mensaje = "Te uniste a la lista de espera. Cuando haya cupo en esta actividad, serás notificado." });
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
    
    [HttpDelete("salir")]
public async Task<IActionResult> SalirListaEspera([FromBody] SalirListaDTO dto)
{
    await salirListaEsperaTurnoUseCase.Ejecutar(dto.Email, dto.IdTurno);

    return Ok(new
    {
        mensaje = "Saliste de la lista de espera."
    });
}
[HttpDelete("salir-abono")]
public async Task<IActionResult> SalirListaEsperaAbono([FromQuery] string email, [FromQuery] Guid idDeporte)
{
    Console.WriteLine($"Intentando salir: email={email}, idDeporte={idDeporte}");
    await salirListaEsperaAbonoUseCase.Ejecutar(email, idDeporte);

    return Ok(new { mensaje = "Saliste de la lista de espera de abonados." });
}
[HttpGet("esta-en-lista-turno")]
public async Task<IActionResult> EstaEnListaTurno(string email, Guid idTurno)
{
    bool esta = await estaEnListaEsperaTurnoUseCase.Ejecutar(email, idTurno);
    return Ok(esta);
}

    public class EntrarListaEsperaRequest
    {
        public string Email { get; set; }
        public Guid IdTurno { get; set; }
    }


[HttpGet("esta-en-lista-abono")]
public async Task<IActionResult> EstaEnListaAbono(
    string email,
    Guid idDeporte,
    Guid idHorario)
{
    bool esta = await estaEnListaEsperaAbonoUseCase.Ejecutar(
        email,
        idDeporte,
        idHorario);

    return Ok(esta);
}


   




}
}