namespace Sportify.Infraestructura.Repositorios;
using System;
using Sportify.Infraestructura.Data;
using Sportify.Dominio.Asistencias;
using Sportify.Aplicacion.AplicacionAsistencias;
using Microsoft.EntityFrameworkCore;
public class RepositorioAsistencias : IRepositorioAsistencias
{
    private readonly ApplicationDbContext archivo;

    public RepositorioAsistencias(ApplicationDbContext context)
    {
        this.archivo = context;
    }

    public async Task<bool> AltaAsistencia(Asistencia asistencia)
    {
        await archivo.Asistencias.AddAsync(asistencia);
        return await archivo.SaveChangesAsync() > 0;
    }

    public async Task<bool> PasarAsistencia(Guid idUsuario, Guid idTurno)
    {
        var asistencia = await archivo.Asistencias
            .FirstOrDefaultAsync(a => a.IdUsuario == idUsuario && a.IdTurno == idTurno);

        if (asistencia != null)
        {
            asistencia.Presente = true;
            await archivo.SaveChangesAsync();
        }

        return asistencia != null;
    }
    public async Task<bool> BuscarAsistencia(Guid idUsuario, Guid idTurno)
    {
        var asistencia = await archivo.Asistencias
            .FirstOrDefaultAsync(a => a.IdUsuario == idUsuario && a.IdTurno == idTurno);

        return asistencia != null;
    }
}