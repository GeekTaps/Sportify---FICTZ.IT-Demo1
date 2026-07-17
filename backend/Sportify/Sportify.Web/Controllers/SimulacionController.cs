using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Infraestructura.Data;
using Sportify.Infraestructura.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Sportify.Web.Controllers;

[ApiController]
[Route("api/simulacion")]
public class SimulacionController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UsuarioIdentity> _userManager;

    public SimulacionController(ApplicationDbContext context, UserManager<UsuarioIdentity> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("dia11")]
    public async Task<IActionResult> SimularDia11()
    {
        try
        {
            // 1. Usuarios: Resetear cancelaciones y levantar suspensiones temporales
            var usuarios = await _userManager.Users.ToListAsync();
            foreach (var user in usuarios)
            {
                bool userChanged = false;

                // Resetear cancelaciones y creditos legacy
                user.CancelacionesMes = 0; // Asumiendo que esta es la propiedad en UsuarioIdentity
                user.Creditos = 0;
                userChanged = true;

                // Levantar suspensión temporal
                if (user.Suspendido && !user.SuspendidoPermanente)
                {
                    user.Suspendido = false;
                    userChanged = true;
                }

                if (userChanged)
                {
                    await _userManager.UpdateAsync(user);
                }
            }

            // 2. Créditos: Borrar todos los créditos
            _context.Creditos.RemoveRange(_context.Creditos);

            // 3. Reservas y Abonos: Cancelar abonos no pagados y sus reservas
            var reservasAbonoNoPagas = await _context.Reservas
                .Where(r => r.abonado && !r.paga && !r.eliminada)
                .ToListAsync();

            foreach (var reserva in reservasAbonoNoPagas)
            {
                // Cancelar reserva lógicamente
                reserva.eliminarLogicamente();

                // Obtener el Turno para saber el Horario
                var turno = await _context.Turnos.FindAsync(reserva.idTurno);
                if (turno != null)
                {
                    // Buscar el abono activo para este usuario y horario
                    var abono = await _context.Abonos
                        .FirstOrDefaultAsync(a => a.IdUsuario == reserva.idUsuario && a.IdHorario == turno.IdHorario && a.Activo);

                    if (abono != null)
                    {
                        abono.Cancelar();
                    }
                }
            }

            // 4. Guardar los cambios de DbContext
            await _context.SaveChangesAsync();

            return Ok(new { message = "Simulación del Día 11 completada correctamente." });
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error en la simulación.", details = ex.Message });
        }
    }
}
