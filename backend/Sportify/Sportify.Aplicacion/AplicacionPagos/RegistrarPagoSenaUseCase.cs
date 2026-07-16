using System;
using Sportify.Dominio.Pagos;

namespace Sportify.Aplicacion.AplicacionPagos;

public class RegistrarPagoSenaUseCase
{
    private readonly IRepositorioPago repositorioPago;

    public RegistrarPagoSenaUseCase(IRepositorioPago repositorioPago)
    {
        this.repositorioPago = repositorioPago;
    }

    public async Task Ejecutar(Pago pagoRealizado)
    {
        await repositorioPago.registrarPagoSena(pagoRealizado);
    }
}