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
    private readonly Sportify.Aplicacion.AplicacionDeportes.IRepositorioDeporte repositorioDeporte;
    private readonly IRepositorioTurno repositorioTurno;

    public TurnosController(
        TurnoListadoUseCase listadoUseCase, 
        TurnoAltaUseCase altaUseCase, 
        TurnoModificacionUseCase modificacionUseCase, 
        Sportify.Aplicacion.AplicacionDeportes.IRepositorioDeporte repositorioDeporte,
        IRepositorioTurno repositorioTurno)
    {
        this.listadoUseCase = listadoUseCase;
        this.altaUseCase = altaUseCase;
        this.modificacionUseCase = modificacionUseCase;
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
    public async Task<IActionResult> CrearTurno([FromBody] Sportify.Web.DTOs.CrearTurnoRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "El request no puede estar vacío." });
        }

        try
        {
            TimeOnly horaInicio;
            TimeOnly horaFin;

            if (!TimeOnly.TryParse(request.HoraInicio, out horaInicio))
            {
                return BadRequest(new { message = "La hora de inicio no tiene un formato válido." });
            }

            if (string.IsNullOrEmpty(request.HoraFin) || !TimeOnly.TryParse(request.HoraFin, out horaFin))
            {
                // Por defecto los turnos duran 1 hora
                horaFin = horaInicio.AddHours(1);
            }

            await altaUseCase.Ejecutar(
                request.IdDeporte,
                request.DiaSemana,
                horaInicio,
                horaFin,
                request.Cupo,
                request.Precio,
                request.NombreProfesor,
                request.ListaEsperaHabilitada
            );
            return Ok(new { message = "Turnos generados exitosamente." });
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
