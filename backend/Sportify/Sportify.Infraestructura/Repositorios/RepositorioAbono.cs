using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sportify.Aplicacion.AplicacionAbonos;
using Sportify.Infraestructura.Data;

namespace Sportify.Infraestructura.Repositorios;

public class RepositorioAbono : IRepositorioAbono
{
    private readonly ApplicationDbContext _context;

    public RepositorioAbono(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteAbonoActivo(Guid idUsuario, Guid idHorario)
    {
        return await _context.Abonos.AnyAsync(a => a.IdUsuario == idUsuario && a.IdHorario == idHorario && a.Activo && !a.Eliminado);
    }

    public async Task CrearAbono(Sportify.Dominio.Abonos.Abono abono)
    {
        _context.Abonos.Add(abono);
        await _context.SaveChangesAsync();
    }
}
