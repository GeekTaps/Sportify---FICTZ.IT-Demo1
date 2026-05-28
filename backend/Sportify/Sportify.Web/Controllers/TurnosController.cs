using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.Excepciones;
using System;

[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly TurnoListadoUseCase listadoUseCase;
    private readonly TurnoAltaUseCase altaUseCase;
    private readonly TurnoModificacionUseCase modificacionUseCase;
    private readonly Sportify.Aplicacion.AplicacionDeportes.IRepositorioDeporte repositorioDeporte;

    public TurnosController(TurnoListadoUseCase listadoUseCase, TurnoAltaUseCase altaUseCase, TurnoModificacionUseCase modificacionUseCase, Sportify.Aplicacion.AplicacionDeportes.IRepositorioDeporte repositorioDeporte)
    {
        this.listadoUseCase = listadoUseCase;
        this.altaUseCase = altaUseCase;
        this.modificacionUseCase = modificacionUseCase;
        this.repositorioDeporte = repositorioDeporte;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTurnos()
    {
        try
        {
            var resultado = await listadoUseCase.Ejecutar();
            return Ok(resultado);
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
}
