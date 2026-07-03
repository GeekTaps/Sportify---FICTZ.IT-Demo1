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

    public async Task<bool> eliminarEspera(ListaDeEsperaAbono e)
    {
        _context.ListaDeEsperaAbono.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> existeEnEspera(ListaDeEsperaAbono e)
    {
        return await _context.ListaDeEsperaAbono.AnyAsync(l => l.idUsuario == e.idUsuario && l.idDeporte == e.idDeporte);
    }

    public async Task<System.Collections.Generic.List<Sportify.Dominio.Usuario.Usuario>> listarUsuarios(Guid idDeporte)
    {
        // Not implemented fully due to not needing it for this specific feature yet
        return new System.Collections.Generic.List<Sportify.Dominio.Usuario.Usuario>();
    }

    public async Task<System.Collections.Generic.List<Sportify.Dominio.Deportes.Deporte>> listarDeportes(Guid idUsuario)
    {
        // Not implemented fully due to not needing it for this specific feature yet
        return new System.Collections.Generic.List<Sportify.Dominio.Deportes.Deporte>();
    }
}
