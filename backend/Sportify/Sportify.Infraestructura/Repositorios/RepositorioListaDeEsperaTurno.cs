namespace Sportify.Infraestructura.Repositorios;
using System;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Dominio.Usuario;
using Sportify.Dominio.Turnos;

public class RepositorioListaDeEsperaTurno : IRepositorioListaDeEsperaTurno
{
    private readonly ApplicationDbContext archivo;

    public RepositorioListaDeEsperaTurno(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }


    public async Task agregarEnEspera(Guid idUsuario, Guid idTurno)
    {
        ListaDeEsperaTurno e = new ListaDeEsperaTurno(idUsuario, idTurno);
        await archivo.ListaDeEsperaTurno.AddAsync(e);
        await archivo.SaveChangesAsync();
    }

    public async Task<Usuario>? siguienteEnEspera(Guid idTurno)
    {
        ListaDeEsperaTurno e = await archivo.ListaDeEsperaTurno.Where(x => x.idTurno == idTurno).OrderBy(x => x.fecha).FirstOrDefaultAsync();
        if(e != null){
            var ui = await archivo.Users.FindAsync(e.idUsuario.ToString());
            Usuario u = null;
            if (ui != null) {
                u = new Usuario(ui.NombreCompleto, ui.Email, ui.Dni, "", "", ui.FechaNacimiento);
            }
            await eliminarEspera(e.idUsuario, e.idTurno);
            return u;
        }else{
            return null;
        }
    }

    public async Task<bool> eliminarEspera(Guid idUsuario, Guid idTurno)
    {
        var e = await archivo.ListaDeEsperaTurno.FirstOrDefaultAsync(x => x.idUsuario == idUsuario && x.idTurno == idTurno);
        if (e != null) {
            archivo.ListaDeEsperaTurno.Remove(e);
            await archivo.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> existeEnEspera(Guid idUsuario, Guid idTurno)
    {
        return await archivo.ListaDeEsperaTurno.AnyAsync(e => e.idUsuario == idUsuario && e.idTurno == idTurno);
    }

    public async Task<List<Usuario>> listarUsuarios(Guid idTurno)
    {
        var entries = await archivo.ListaDeEsperaTurno
            .Where(e => e.idTurno == idTurno)
            .OrderBy(e => e.fecha)
            .ToListAsync();

        var ids = entries.Select(e => e.idUsuario.ToString()).ToList();
        var users = await archivo.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
        var usersById = users.ToDictionary(u => u.Id, u => u);

        return entries
            .Select(e => usersById.TryGetValue(e.idUsuario.ToString(), out var user) ? new Usuario(user.Id, user.NombreCompleto, user.Email, user.Dni, "", "", user.FechaNacimiento, 0) : null)
            .Where(u => u != null)
            .Select(u => u!)
            .ToList();
    }

    public async Task<List<Turno>> listarTurnos(Guid idUsuario)
    {
        var ids = await archivo.ListaDeEsperaTurno.Where(e => e.idUsuario == idUsuario).Select(e => e.idTurno).ToListAsync();
        return await archivo.Turnos.Where(t => ids.Contains(t.Id)).ToListAsync();
    }



}
