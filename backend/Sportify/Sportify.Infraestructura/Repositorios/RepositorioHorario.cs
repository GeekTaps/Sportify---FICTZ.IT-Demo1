namespace Sportify.Infraestructura.Repositorios;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
using Sportify.Infraestructura.Data;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class RepositorioHorario : IRepositorioHorario
{
    private readonly ApplicationDbContext _context;

    public RepositorioHorario(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Horario> CrearYGuardarHorario(Guid idDeporte, string diaSemana, TimeOnly horaInicio)
    {
        var horario = new Horario(idDeporte, diaSemana, horaInicio);
        await _context.Horarios.AddAsync(horario);
        await _context.SaveChangesAsync();
        return horario;
    }

    public async Task<Horario?> ObtenerHorarioPorId(Guid id)
    {
        return await _context.Horarios.FindAsync(id);
    }

    public async Task<bool> ExisteHorario(Guid idDeporte, string diaSemana, TimeOnly horaInicio)
    {
        return await _context.Horarios.AnyAsync(h => h.idDeporte == idDeporte && h.diaSemana == diaSemana && h.hora == horaInicio && !h.eliminado);
    }
}
