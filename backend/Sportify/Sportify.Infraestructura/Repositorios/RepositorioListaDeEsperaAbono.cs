using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Infraestructura.Data;

namespace Sportify.Infraestructura.Repositorios;

public class RepositorioListaDeEsperaAbono : IRepositorioListaDeEsperaAbono
{
    private readonly ApplicationDbContext _context;

    public RepositorioListaDeEsperaAbono(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task agregarEnEspera(ListaDeEsperaAbono e)
    {
        _context.ListaDeEsperaAbono.Add(e);
        await _context.SaveChangesAsync();
    }

public async Task<bool> eliminarEspera(Guid idUsuario, Guid idDeporte)
{
    var entrada = await _context.ListaDeEsperaAbono
        .FirstOrDefaultAsync(x =>
            x.idUsuario == idUsuario &&
            x.idDeporte == idDeporte);

    if (entrada == null)
        return false;

    _context.ListaDeEsperaAbono.Remove(entrada);
    await _context.SaveChangesAsync();

    return true;
}

    public async Task<bool> existeEnEspera(Guid idUsuario, Guid idDeporte)
{
    return await _context.ListaDeEsperaAbono
        .AnyAsync(l =>
            l.idUsuario == idUsuario &&
            l.idDeporte == idDeporte);
}

    public async Task<List<Sportify.Dominio.Usuario.Usuario>> listarUsuarios(Guid idHorario)
{
    // Traigo las entradas de la lista de espera de ese horario,
    // ordenadas por fecha de inscripción (el primero anotado va primero).
    var entradas = await _context.ListaDeEsperaAbono
        .Where(x => x.idHorario == idHorario)
        .OrderBy(x => x.fecha)
        .ToListAsync();

    var usuarios = new List<Sportify.Dominio.Usuario.Usuario>();

    foreach (var entrada in entradas)
    {
        var usuario = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == entrada.idUsuario.ToString());

        if (usuario != null)
        {
            usuarios.Add(new Sportify.Dominio.Usuario.Usuario(
                usuario.Id,
                usuario.NombreCompleto,
                usuario.Email,
                usuario.Dni,
                "",
                "",
                usuario.FechaNacimiento,
                usuario.Creditos
            ));
        }
    }

    return usuarios;
}

    public async Task<System.Collections.Generic.List<Sportify.Dominio.Turnos.Horario>> listarHorarios(Guid idUsuario)
    {
        // Not implemented fully due to not needing it for this specific feature yet
        return new System.Collections.Generic.List<Sportify.Dominio.Turnos.Horario>();
    }
}
