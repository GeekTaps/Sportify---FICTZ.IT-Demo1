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
    }
}
