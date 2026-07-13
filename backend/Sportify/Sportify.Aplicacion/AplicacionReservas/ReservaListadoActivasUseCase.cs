using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaListadoActivasUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;

    // devuelve el listado de reservas activas de un usuario
    public ReservaListadoActivasUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
    }

    public async Task<List<Reserva>> Ejecutar(Guid idUsuario)
    {
        List<Reserva> reservas = await repositorioReserva.listarReservasUsuario(idUsuario);
        List<Reserva> reservasActivas = reservas.Where(r => !r.estaEliminada()).ToList();
        if(reservasActivas.Count == 0){
            throw new ListadoVacioException("No Contás Con Reservas Activas");
        }
        return reservasActivas;
    }
}