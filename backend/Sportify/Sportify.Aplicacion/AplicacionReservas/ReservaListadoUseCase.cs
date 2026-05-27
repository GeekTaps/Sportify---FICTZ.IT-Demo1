using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaListadoUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;

    // devuelve el listado de reservas de un usuario
    public ReservaListadoUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
    }

    public async Task<List<Reserva>> Ejecutar(Guid idUsuario)
    {
        List<Reserva> reservas = await repositorioReserva.listarReservasUsuario(idUsuario);
        if(reservas.Count == 0){
            throw new ListadoVacioException("No Cuenta Con Reservas Activas Actualmente");
        }
        return reservas;
    }
}