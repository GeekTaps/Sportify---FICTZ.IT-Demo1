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

    // *-*-*-*-*-*-*-*-*-* AGREGAR *-*-*-*-*-*-*-*-*-*

    public void agregarReserva(Reserva r)
    {
        archivo.Reservas.Add(r);
        archivo.SaveChanges();
    }

    // *-*-*-*-*-*-*-*-*-* ELIMINAR *-*-*-*-*-*-*-*-*-*
    
    public bool eliminarReserva(Guid idReserva)
    {
        Reserva? reserva = archivo.Reservas.Find(idReserva);
        if (reserva == null)
        {
          return false;
        }
        archivo.Reservas.Remove(reserva);
        archivo.SaveChanges();
        return true;
    }

    // *-*-*-*-*-*-*-*-*-* CHEQUEAR *-*-*-*-*-*-*-*-*-*
    
    public bool existeReserva(Guid idReserva)
    {
        Reserva? reserva = archivo.Reservas.Find(idReserva);
        if (reserva == null)
        {
          return false;
        }
        else return true;
    }
    
    // *-*-*-*-*-*-*-*-*-* BUSCAR POR USUARIO *-*-*-*-*-*-*-*-*-*

    public List<Reserva> listarReservasUsuario(Guid idUsuario)
    {   // si no encuentra nada devuelve lista vacía
        return archivo.Reservas.Where(r => r.idUsuario == idUsuario).ToList();
        
    }
    
    // *-*-*-*-*-*-*-*-*-* BUSCAR *-*-*-*-*-*-*-*-*-*

    public Reserva? buscarReserva(Guid idReserva)
    {
        return archivo.Reservas.FirstOrDefault(r => r.id == idReserva);
    }

} 