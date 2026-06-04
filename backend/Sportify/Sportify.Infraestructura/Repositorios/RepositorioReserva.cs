namespace Sportify.Infraestructura.Repositorios;
using System;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionReservas;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Reservas;

public class RepositorioReserva : IRepositorioReserva
{
    private readonly ApplicationDbContext archivo;

    public RepositorioReserva(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }

    // *-*-*-*-*-*-*-*-*-* AGREGAR *-*-*-*-*-*-*-*-*-*

    public async Task agregarReserva(Reserva r)
    {
        await archivo.Reservas.AddAsync(r);
        await archivo.SaveChangesAsync();
    }

    // *-*-*-*-*-*-*-*-*-* ELIMINAR *-*-*-*-*-*-*-*-*-*

    public async Task<bool> eliminarReserva(Guid idReserva)
    {
        Reserva? reserva = await archivo.Reservas.FindAsync(idReserva);
        if (reserva == null)
        {
          return false;
        }
        archivo.Reservas.Remove(reserva);
        await archivo.SaveChangesAsync();
        return true;
    }

    // *-*-*-*-*-*-*-*-*-* CHEQUEAR *-*-*-*-*-*-*-*-*-*

    public async Task<bool> existeReserva(Guid idReserva)
    {
        Reserva? reserva = await archivo.Reservas.FindAsync(idReserva);
        if (reserva == null)
        {
          return false;
        }
        else return true;
    }

    // *-*-*-*-*-*-*-*-*-* BUSCAR POR USUARIO *-*-*-*-*-*-*-*-*-*

    public async Task<List<Reserva>> listarReservasUsuario(Guid idUsuario)
    {   // si no encuentra nada devuelve lista vacía
        return await archivo.Reservas.Where(r => r.idUsuario == idUsuario).ToListAsync();

    }

    // *-*-*-*-*-*-*-*-*-* BUSCAR *-*-*-*-*-*-*-*-*-*

    public async Task<Reserva> buscarReserva(Guid idReserva)
    {
        // asume que reserva existe
        return await archivo.Reservas.FirstOrDefaultAsync(r => r.id == idReserva);
    }

    public async Task<int> ContarReservasPorTurno(Guid idTurno)
    {
        return await archivo.Reservas.CountAsync(r => r.idTurno == idTurno);
    }
}