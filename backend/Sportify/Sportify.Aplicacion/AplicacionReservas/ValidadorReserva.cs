using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;

namespace Sportify.Aplicacion.AplicacionReservas;
// using Sportify.Aplicacion.AplicacionTurnos;



public class validadorReserva : IValidadorReserva
{
    public async Task<bool> validarId(Guid idReserva, IRepositorioReserva repositorioReserva) // = chequear si existe
    {
        return await repositorioReserva.existeReserva(idReserva);
    }

    public async Task<bool> validarAntelación(Guid idReserva, IRepositorioReserva repositorioReserva) // = chequear si faltan 48hs o más
    {
        // asume que la reserva existe sí o sí
        Reserva r =  await repositorioReserva.buscarReserva(idReserva);
        TimeSpan diferencia = DateTime.Now - r.fecha;
        return Math.Abs(diferencia.TotalHours) >= 48;
    }

    public async Task<bool> validarReservaPaga(Guid idReserva, IRepositorioReserva repositorioReserva) // = chequear si la reserva está paga
    {
        // asume que la reserva existe sí o sí
        Reserva r =  await repositorioReserva.buscarReserva(idReserva);
        return r.paga;
    }

}