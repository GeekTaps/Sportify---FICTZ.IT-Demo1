using System;
using Sportify.Dominio.Pagos;

namespace Sportify.Aplicacion.AplicacionPagos;

public class ListarPagosUsuarioUseCase
{
    private readonly IRepositorioPago repositorioPago;

    public ListarPagosUsuarioUseCase(IRepositorioPago repositorioPago)
    {
        this.repositorioPago = repositorioPago;
    }

    public async Task<List<Pago>> Ejecutar(Guid idUsuario)
    {
        return await repositorioPago.listarPagosUsuario(idUsuario);
    }
}