namespace Sportify.Infraestructura.Repositorios;
using System;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionUsuarios;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Deportes;
using Sportify.Dominio.Usuario;

public class RepositorioCreditos : IRepositorioCreditos
{
    public readonly ApplicationDbContext archivo;

    public RepositorioCreditos(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }

    public async Task<Credito?> ObtenerCredito(Guid usuarioId, Guid deporteId)
    {
        return await archivo.Creditos.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.DeporteId == deporteId);
    }

    public async Task AgregarCredito(Credito credito)
    {
        await archivo.Creditos.AddAsync(credito);
        await archivo.SaveChangesAsync();
    }

    public async Task ModificarCredito(Credito credito)
    {
        archivo.Creditos.Update(credito);
        await archivo.SaveChangesAsync();
    }
}