using System;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaBusquedaUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;

    public ReservaBusquedaUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
    }

    public Reserva Ejecutar(Guid idReserva) // ejecuta el caso de uso de buscar una Reserva
    {
        if (validadorReserva.validarId(idReserva, repositorioReserva)) // valida que la Reserva exista
            // nota: tmb podria poner el return como opcional para que pueda devolver null 
            repositorioReserva.buscarReserva(idReserva);
        }else
        {
            throw new EntidadNotFoundException("La Reserva Que Intenta buscar No Existe");
        }
    }
}