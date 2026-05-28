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

    public DeportesController(DeporteListadoUseCase listadoUseCase, DeporteAltaUseCase altaUseCase, DeporteModificacionUseCase modificacionUseCase)
    {
        this.listadoUseCase = listadoUseCase;
        this.altaUseCase = altaUseCase;
        this.modificacionUseCase = modificacionUseCase;
    }

    [HttpGet]
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

    [HttpPut("{id:guid}")]
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
