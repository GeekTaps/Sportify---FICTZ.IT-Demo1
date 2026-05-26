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
    private readonly DeporteModificacionUseCase modificacionUseCase;

    public DeportesController(DeporteListadoUseCase listadoUseCase, DeporteModificacionUseCase modificacionUseCase)
    {
        this.listadoUseCase = listadoUseCase;
        this.modificacionUseCase = modificacionUseCase;
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

    [HttpPut("{id:guid}")] //el put es para modificar datos en el backend.
    public async Task<IActionResult> ModificarDeporte(Guid id, [FromBody] Deporte deporte)
    {
        if (deporte == null || string.IsNullOrWhiteSpace(deporte.nombre) || string.IsNullOrWhiteSpace(deporte.descripcion))
        {
            return BadRequest(new { message = "Nombre y descripción son obligatorios." });
        }

        try
        {
            await modificacionUseCase.Ejecutar(id, deporte.nombre.Trim(), deporte.descripcion.Trim());
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