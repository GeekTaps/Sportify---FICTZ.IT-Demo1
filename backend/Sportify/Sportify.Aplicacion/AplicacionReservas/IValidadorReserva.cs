using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;

namespace Sportify.Aplicacion.AplicacionReservas;

public interface IValidadorReserva
{
    public Task<bool> validarId(Guid idReserva, IRepositorioReserva repositorioReserva); // = chequear si existe

    public Task<bool> validarAntelación(Guid idReserva, IRepositorioReserva repositorioReserva); // = chequear si faltan 48hs o más

    public Task<bool> validarReservaPaga(Guid idReserva, IRepositorioReserva repositorioReserva); // = chequear si la reserva está paga

}