using Microsoft.AspNetCore.Mvc;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Usuario;
using Sportify.Web.DTOs;
using Sportify.Infraestructura.Data;
using System.Linq;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/usuarios")]
public class ModificarUsuarioController : ControllerBase
{
    private readonly modificarUsuarioUseCase actualizarUsuarioUseCase;
    private readonly ApplicationDbContext _context;

    public ModificarUsuarioController(modificarUsuarioUseCase actualizarUsuarioUseCase, ApplicationDbContext context)
    {
        this.actualizarUsuarioUseCase = actualizarUsuarioUseCase;
        this._context = context;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            Usuario usuario = await actualizarUsuarioUseCase.ObtenerPorId(id);
            var userGuid = Guid.Parse(id);

            var creditos = _context.Creditos
                .Where(c => c.UsuarioId == userGuid && c.Cantidad > 0)
                .Join(_context.Deportes, c => c.DeporteId, d => d.id, (c, d) => new { 
                    deporte = d.nombre, 
                    cantidad = c.Cantidad 
                })
                .ToList();

            var abonos = _context.Abonos
                .Where(a => a.IdUsuario == userGuid && a.Activo)
                .Join(_context.Horarios, a => a.IdHorario, h => h.id, (a, h) => new { a, h })
                .Join(_context.Deportes, ah => ah.h.idDeporte, d => d.id, (ah, d) => new { 
                    deporte = d.nombre, 
                    dia = ah.h.diaSemana, 
                    hora = ah.h.hora.ToString("HH:mm") + "hs" 
                })
                .ToList();

            return Ok(new
            {
                nombreCompleto = usuario.NombreCompleto,
                email = usuario.Mail,
                dni = usuario.Dni,
                fechaNacimiento = usuario.FechaNacimiento,
                creditos = creditos,
                abonos = abonos
            });
        }
        catch (ValidacionException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ModificarUsuarioDTO dto)
    {
        try
        {
            Usuario usuario = new Usuario(
                dto.NombreCompleto ?? "",
                dto.Email ?? "",
                dto.Dni ?? "",
                dto.PasswordActual ?? "",
                dto.PasswordNueva ?? "",
                dto.FechaNacimiento ?? DateTime.MinValue
            );

            await actualizarUsuarioUseCase.Ejecutar(id, usuario);

            return Ok(new { message = "Usuario actualizado correctamente" });
        }
        catch (ValidacionException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}