using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionAsistencias;
using System;
using System.Threading.Tasks;

namespace Sportify.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // api/Asistencias
    public class AsistenciasController : ControllerBase
    {
        private readonly AsistenciaPasarPresente asistenciaPasarPresente;
        private readonly AsistenciaAlta asistenciaAlta;
        public AsistenciasController(AsistenciaPasarPresente asistenciaPasarPresente, AsistenciaAlta asistenciaAlta)
        {
            this.asistenciaPasarPresente = asistenciaPasarPresente;
            this.asistenciaAlta = asistenciaAlta;
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
    }

    public class ConfirmarAsistenciaRequest
    {
        public Guid UsuarioId { get; set; }
        public Guid TurnoId { get; set; }
    }
}
