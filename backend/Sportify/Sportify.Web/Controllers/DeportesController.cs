using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Dominio.Deportes;
using System;

[ApiController]
[Route("api/[controller]")]
public class DeportesController : ControllerBase
{
    private readonly DeporteListadoUseCase listadoUseCase;
    private readonly DeporteAltaUseCase altaUseCase;
    private readonly DeporteModificacionUseCase modificacionUseCase;
    private readonly IRepositorioDeporte repositorioDeporte;

    public DeportesController(DeporteListadoUseCase listadoUseCase, DeporteAltaUseCase altaUseCase, DeporteModificacionUseCase modificacionUseCase, IRepositorioDeporte repositorioDeporte)
    {
        this.listadoUseCase = listadoUseCase;
        this.altaUseCase = altaUseCase;
        this.modificacionUseCase = modificacionUseCase;
        this.repositorioDeporte = repositorioDeporte;
    }

    //EJ de otros tipos:
    //GET     /api/deportes        → listar
    //GET     /api/deportes/{id}   → obtener uno
    //POST    /api/deportes        → crear
    //PUT     /api/deportes/{id}   → actualizar
    //DELETE  /api/deportes/{id}   → eliminar

    [HttpDelete("{id:guid}")] //post es para eliminar datos en el backend.
    public async Task<IActionResult> eliminarDeporte(Guid id)
    {
        try
        {
            DeporteBajaUseCase? bajaUseCase = HttpContext.RequestServices.GetService(typeof(DeporteBajaUseCase)) as DeporteBajaUseCase;
            await bajaUseCase.Ejecutar(id);
            return NoContent();
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

    [HttpGet] //get es para traerte datos del backend. No modifica datos.
    public async Task<IActionResult> ObtenerDeportes()
    {
        var resultado = await listadoUseCase.Ejecutar();

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> CrearDeporte([FromBody] Deporte deporte)
    {
        if (deporte == null || string.IsNullOrWhiteSpace(deporte.nombre) || string.IsNullOrWhiteSpace(deporte.descripcion))
        {
            return BadRequest(new { message = "complete los campos para registrar un deporte" });
        }

        try
        {
            await altaUseCase.Ejecutar(deporte.nombre.Trim(), deporte.descripcion.Trim());
            return Ok(new { message = "deporte registrado correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerDeporte(Guid id)
    {
        try
        {
            var deporte = await repositorioDeporte.obtenerDeportePorId(id);
            if (deporte == null)
            {
                return NotFound(new { message = "Deporte no encontrado." });
            }
            return Ok(deporte);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> ModificarDeporte(Guid id, [FromBody] Deporte deporte)
    {
        if (deporte == null || string.IsNullOrWhiteSpace(deporte.descripcion))
        {
            return BadRequest(new { message = "La descripción es obligatoria." });
        }

        try
        {
            await modificacionUseCase.Ejecutar(id, deporte.descripcion.Trim());
            return NoContent();
        }
        catch (EntidadNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
