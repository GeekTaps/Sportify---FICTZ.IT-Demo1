using System;
using System.Linq;
using Sportify.Dominio.Pagos;
using Sportify.Dominio.Reservas;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Sportify.Infraestructura.Data;

namespace Sportify.Aplicacion.AplicacionPagos;

public class RepositorioPagos : IRepositorioPago
{
    private readonly ApplicationDbContext archivo;

    public RepositorioPagos(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }
    
    public void realizarSeña(Pago pagoRealizado)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Pago>> listarPagosUsuario(Guid idUsuario)
    {
         return await archivo.Pagos.Where(p => p.idUsuario == idUsuario).ToListAsync();
    }

    public async Task registrarPago(Pago pagoRealizado)
    {
        var reserva = await archivo.Reservas.FirstOrDefaultAsync(r => r.id == pagoRealizado.idReserva && r.idUsuario == pagoRealizado.idUsuario);
        if (reserva != null)
        {
            reserva.marcarComoPagada(); 
        }

        archivo.Pagos.Add(pagoRealizado);
        await archivo.SaveChangesAsync();
    }
}