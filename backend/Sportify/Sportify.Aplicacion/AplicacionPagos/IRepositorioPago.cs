using System;
using Sportify.Dominio.Pagos;

namespace Sportify.Aplicacion.AplicacionPagos;
public interface IRepositorioPago
{
    public void realizarSeña (Pago pagoRealizado);
}