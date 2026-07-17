using System;
using Sportify.Dominio.Pagos;

namespace Sportify.Aplicacion.AplicacionPagos;

public class RegistrarDevolucionSeñaUseCase
{
    private readonly IRepositorioPago repositorioPago;

    public RegistrarDevolucionSeñaUseCase(IRepositorioPago repositorioPago)
    {
        this.repositorioPago = repositorioPago;
    }

    public async Task Ejecutar(Pago pagoRealizado)
    {
        await repositorioPago.registrarDevolucionSeña(pagoRealizado);
    }
}