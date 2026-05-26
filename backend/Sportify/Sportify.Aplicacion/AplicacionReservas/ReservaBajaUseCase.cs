using System;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaAltaUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;

    public ReservaAltaUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
    }

    public void Ejecutar(Guid idReserva) //ejecuta el caso de uso de eliminar una Reserva
    {
        if (validadorReserva.validarId(idReserva, repositorioReserva)) //valida que la Reserva exista antes de eliminarla

            repositorioReserva.eliminarReserva(idReserva);
        }else
        {
            throw new EntidadNotFoundException("La Reserva Que Intenta Eliminar No Existe");
        }
    }
}