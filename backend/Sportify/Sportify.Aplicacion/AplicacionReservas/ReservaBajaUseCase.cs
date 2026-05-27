using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaBajaUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;

    public ReservaBajaUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
    }

    public async Task Ejecutar(Guid idReserva) //ejecuta el caso de uso de eliminar una Reserva
    {
        if (await validadorReserva.validarId(idReserva, repositorioReserva)) //valida que la Reserva exista antes de eliminarla

            await repositorioReserva.eliminarReserva(idReserva);
        else
        {
            throw new EntidadNotFoundException("La Reserva Que Intenta Eliminar No Existe");
        }
    }
}