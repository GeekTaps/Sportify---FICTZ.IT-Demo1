using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Sportify.Aplicacion.AplicacionAsistencias;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Asistencias;
using Sportify.Infraestructura.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Asistencias;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/Asistencias
    public class AsistenciasController : ControllerBase
    {
        private readonly AsistenciaPasarPresente asistenciaPasarPresente;
        private readonly AsistenciaAlta asistenciaAlta;
        private readonly IRepositorioAsistencias _repositorioAsistencias;
        private readonly IRepositorioReserva _repositorioReserva;
        private readonly UserManager<UsuarioIdentity> _userManager;

        public AsistenciasController(AsistenciaPasarPresente asistenciaPasarPresente, AsistenciaAlta asistenciaAlta, IRepositorioAsistencias repositorioAsistencias, IRepositorioReserva repositorioReserva, UserManager<UsuarioIdentity> userManager)
        private readonly AsistenciaListarAsistenciasDeUsuarioUseCase _listarAsistenciasUseCase;

        public AsistenciasController(AsistenciaPasarPresente asistenciaPasarPresente, AsistenciaAlta asistenciaAlta, AsistenciaListarAsistenciasDeUsuarioUseCase listarAsistenciasUseCase)
        {
            this.asistenciaPasarPresente = asistenciaPasarPresente;
            this.asistenciaAlta = asistenciaAlta;
            this._repositorioAsistencias = repositorioAsistencias;
            this._repositorioReserva = repositorioReserva;
            this._userManager = userManager;
        }

        [HttpGet("clase/{idTurno:guid}")] // api/Asistencias/clase/{idTurno}
        public async Task<IActionResult> ListarAsistenciasPorClase(Guid idTurno)
        {
            try
            {
                var reservas = await _repositorioReserva.ListarReservasPorTurno(idTurno);
                var resultado = new List<object>();

                foreach (var reserva in reservas)
                {
                    var usuario = await _userManager.FindByIdAsync(reserva.idUsuario.ToString());
                    var nombreUsuario = usuario != null
                        ? (!string.IsNullOrWhiteSpace(usuario.NombreCompleto) ? usuario.NombreCompleto : (!string.IsNullOrWhiteSpace(usuario.Email) ? usuario.Email : usuario.UserName))
                        : "Desconocido";

                    var presente = await _repositorioAsistencias.AsistioATurno(reserva.idUsuario, idTurno);

                    resultado.Add(new {
                        IdUsuario = reserva.idUsuario,
                        Usuario = nombreUsuario,
                        Presente = presente
                    });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
            this._listarAsistenciasUseCase = listarAsistenciasUseCase; 
        }

        [HttpPut("confirmar-presente")] // api/Asistencias/confirmar-presente
        public async Task<IActionResult> ConfirmarPresente([FromBody] ConfirmarAsistenciaRequest request)
        {
            try
            {
                // Ejecutamos el caso de uso con los IDs que viajan desde el QR de React
                bool resultado = await asistenciaPasarPresente.Ejecutar(request.UsuarioId, request.TurnoId);

                if (resultado)
                {
                    return Ok(new { mensaje = "¡Asistencia registrada con éxito!" });
                }

                return BadRequest(new { mensaje = "No se pudo actualizar el estado de la asistencia." });
            }
            catch (Exception ex) when (ex.Message.Contains("No se encontró la asistencia"))
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", error = ex.Message });
            }
        }
       [HttpGet("usuario/{idUsuario}")]
       public async Task<ActionResult<List<Asistencia>>> ObtenerAsistenciasPorUsuario(Guid idUsuario)
       {
            try
            {
                var asistencias = await _listarAsistenciasUseCase.Ejecutar(idUsuario);
                return Ok(asistencias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
       }
    }

    public class ConfirmarAsistenciaRequest
    {
        public Guid UsuarioId { get; set; }
        public Guid TurnoId { get; set; }
    }
}