using System;
using Sportify.Dominio.Pagos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Sportify.Infraestructura.Data;

namespace Sportify.Aplicacion.AplicacionPagos;

public class RepositorioPagos : IRepositorioPago
{
    private readonly ApplicationDbContext archivo;
    
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
        archivo.Pagos.Add(pagoRealizado);
        await archivo.SaveChangesAsync();
    }
}