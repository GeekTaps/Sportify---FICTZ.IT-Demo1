    using Microsoft.AspNetCore.Mvc;
    using Sportify.Aplicacion.AplicacionUsuarios;
    using Sportify.Dominio.Usuario;
    using Sportify.Web.DTOs;
    using Sportify.Aplicacion.Excepciones;
    

    namespace Sportify.Web.Controllers;
    

[ApiController]
[Route("api/empleados")]
public class EmpleadosController : ControllerBase
{
    private readonly RegistrarEmpleadoUseCase registrarEmpleadoUseCase;

    public EmpleadosController(RegistrarEmpleadoUseCase registrarEmpleadoUseCase)
    {
        this.registrarEmpleadoUseCase = registrarEmpleadoUseCase;
    }

    [HttpPost]
public async Task<IActionResult> Registrar([FromBody] RegistrarEmpleadoDTO dto)
{
try{    
var usuario = new Usuario(
    dto.NombreCompleto,
    dto.Email,
    dto.Dni,
    "",           // PasswordActual vacío
    dto.Password, // PasswordNueva
    dto.FechaNacimiento
);

    await registrarEmpleadoUseCase.Ejecutar(usuario);

    return Ok("Empleado registrado correctamente.");
}
    catch (ValidacionException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
}