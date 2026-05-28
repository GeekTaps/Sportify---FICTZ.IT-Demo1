using System;
using Sportify.Dominio.Pagos;

namespace Sportify.Aplicacion.AplicacionPagos;

public class RegistrarPagoUseCase
{
    private readonly IRepositorioPago repositorioPago;

    public RegistrarPagoUseCase(IRepositorioPago repositorioPago)
    {
        this.repositorioPago = repositorioPago;
    }

    public async Task Ejecutar(Pago pagoRealizado)
    {
        await repositorioPago.registrarPago(pagoRealizado);
    }
}