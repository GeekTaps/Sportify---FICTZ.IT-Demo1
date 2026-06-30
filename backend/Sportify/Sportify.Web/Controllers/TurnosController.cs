using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion;
using Sportify.Aplicacion.Excepciones;
using System;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly TurnoListadoUseCase listadoUseCase;
    private readonly TurnoAltaUseCase altaUseCase;
    private readonly TurnoModificacionUseCase modificacionUseCase;
    private readonly TurnoAltaMensualUseCase altaMensualUseCase;
    private readonly TurnoModificacionMensualUseCase modificacionMensualUseCase;
    private readonly Sportify.Aplicacion.AplicacionDeportes.IRepositorioDeporte repositorioDeporte;
    private readonly IRepositorioTurno repositorioTurno;

    public TurnosController(
        TurnoListadoUseCase listadoUseCase, 
        TurnoAltaUseCase altaUseCase, 
        TurnoModificacionUseCase modificacionUseCase, 
        TurnoAltaMensualUseCase altaMensualUseCase,
        TurnoModificacionMensualUseCase modificacionMensualUseCase,
        Sportify.Aplicacion.AplicacionDeportes.IRepositorioDeporte repositorioDeporte,
        IRepositorioTurno repositorioTurno)
    {
        this.listadoUseCase = listadoUseCase;
        this.altaUseCase = altaUseCase;
        this.modificacionUseCase = modificacionUseCase;
        this.altaMensualUseCase = altaMensualUseCase;
        this.modificacionMensualUseCase = modificacionMensualUseCase;
        this.repositorioDeporte = repositorioDeporte;
        this.repositorioTurno = repositorioTurno;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTurnos()
    {
        try
        {
            await repositorioTurno.actualizarMostrarEnHome();
            var resultado = await listadoUseCase.Ejecutar();

            var ahora = DateTime.Now;
            var filtrados = resultado
                .Where(t => t.mostrarEnHome && (t.Fecha.Date.Add(t.horaInicio.ToTimeSpan()) > ahora))
                .ToList();

            return Ok(filtrados);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerTurnoPorId(Guid id)
    {
        try
        {
            var turnos = await listadoUseCase.Ejecutar();
            var turno = turnos.FirstOrDefault(t => t.Id == id);
            
            if (turno == null)
            {
                return NotFound(new { message = "Turno no encontrado." });
            }
            
            return Ok(turno);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CrearTurno([FromBody] Turno turno)
    {
        if (turno == null)
        {
            return BadRequest(new { message = "El turno no puede estar vacío." });
        }

        var deportes = await repositorioDeporte.ListarDeportes();
        var deporte = deportes.FirstOrDefault(d => d.id == turno.IdDeporte);
        if (deporte != null)
        {
            turno.nombreTurno = $"{deporte.nombre} - {turno.Fecha:dd/MM/yy} - {turno.horaInicio:HH:mm}hs";
        }
        else
        {
            turno.nombreTurno = $"Turno - {turno.Fecha:dd/MM/yy} - {turno.horaInicio:HH:mm}hs";
        }

        try
        {
            await altaUseCase.Ejecutar(turno);
            return Ok(new { message = "Turno creado exitosamente.", id = turno.Id });
        }
        catch (ValidacionException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (EntidadRepetidaException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> ModificarTurno(Guid id, [FromBody] Turno turno)
    {
        if (turno == null)
        {
            return BadRequest(new { message = "El turno no puede estar vacío." });
        }

        var deportes = await repositorioDeporte.ListarDeportes();
        var deporte = deportes.FirstOrDefault(d => d.id == turno.IdDeporte);
        if (deporte != null)
        {
            turno.nombreTurno = $"{deporte.nombre} - {turno.Fecha:dd/MM/yy} - {turno.horaInicio:HH:mm}hs";
        }
        else
        {
            turno.nombreTurno = $"Turno - {turno.Fecha:dd/MM/yy} - {turno.horaInicio:HH:mm}hs";
        }

        try
        {
            turno.Id = id;
            await modificacionUseCase.Ejecutar(turno, id);
            return NoContent();
        }
        catch (ValidacionException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("mensual")]
        public async Task<IActionResult> CrearTurnoMensual([FromBody] Sportify.Web.DTOs.CrearTurnoMensualRequest request)
        {
            try
            {
                await altaMensualUseCase.Ejecutar(
                    request.IdDeporte, 
                    request.FechaInicio, 
                    request.HoraInicio, 
                    request.Cupo, 
                    request.Precio,
                    request.NombreProfesor, 
                    request.ListaEsperaHabilitada);
                    
                return Ok(new { message = "Turno creado con éxito" });
            }
            catch (ValidacionException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (EntidadRepetidaException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("mensual/{id:guid}")]
        public async Task<IActionResult> ModificarTurnoMensual(Guid id, [FromBody] Sportify.Web.DTOs.CrearTurnoMensualRequest request)
        {
            try
            {
                var turnoActual = (await repositorioTurno.ListarTurnos())
                    .FirstOrDefault(t => t.Id == id);

                if (turnoActual == null)
                {
                    return NotFound(new { message = "Turno no encontrado." });
                }

                request.IdDeporte = turnoActual.IdDeporte;
                request.FechaInicio = turnoActual.Fecha.ToString("yyyy-MM-dd");
                request.HoraInicio = turnoActual.horaInicio.ToString("HH:mm");

                await modificacionMensualUseCase.Ejecutar(
                    id,
                    request.IdDeporte, 
                    request.FechaInicio, 
                    request.HoraInicio, 
                    request.Cupo, 
                    request.Precio,
                    request.NombreProfesor, 
                    request.ListaEsperaHabilitada);
                    
                return Ok(new { message = "Turnos modificados con éxito" });
            }
            catch (ValidacionException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (EntidadRepetidaException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> EliminarTurno(Guid id, [FromServices] TurnoBajaUseCase bajaUseCase)
        {
            try
            {
                await bajaUseCase.Ejecutar(id);
                return Ok(new { message = "Turno eliminado con éxito" });
            }
            catch (EntidadNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (EntidadAsociadaException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
