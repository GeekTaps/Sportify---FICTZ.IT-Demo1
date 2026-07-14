using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionEstadisticas;
using Sportify.Aplicacion.AplicacionEstadisticas.DTOs;
using System.Collections.Generic;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadisticasController : ControllerBase
    {
        private readonly IRepositorioEstadisticas _repositorioEstadisticas;

        public EstadisticasController(IRepositorioEstadisticas repositorioEstadisticas)
        {
            _repositorioEstadisticas = repositorioEstadisticas;
        }

        [HttpGet("deportes")]
        public async Task<IActionResult> ObtenerEstadisticasPorDeporte()
        {
            try
            {
                List<EstadisticaDeporteDto> lista = await _repositorioEstadisticas.ObtenerInscripcionesPorDeporte();
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
        [HttpGet("reservas")]
    public async Task<IActionResult> ObtenerEstadisticasReservas()
    {
    try
    {
        var stats = await _repositorioEstadisticas.ObtenerEstadisticasReservas();
        return Ok(stats);
    }
    catch (System.Exception ex)
    {
        return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
    }
}

        [HttpGet("turnos")]
        public async Task<IActionResult> ObtenerEstadisticasPorTurno()
        {
            try
            {
                List<EstadisticaTurnoDto> lista = await _repositorioEstadisticas.ObtenerInscripcionesPorTurno();
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("asistencias")]
        public async Task<IActionResult> ObtenerEstadisticasPorAsistencia()
        {
            try
            {
                List<EstadisticaAsistenciaDto> lista = await _repositorioEstadisticas.ObtenerAsistenciasPorDeporte();
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("pagos")]
        public async Task<IActionResult> ObtenerEstadisticasDePagos()
        {
            try
            {
                List<EstadisticaPagoDto> lista = await _repositorioEstadisticas.ObtenerEstadisticasDePagos();
                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
