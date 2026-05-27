using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;

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

    public async Task Ejecutar(Reserva reserva) //ejecuta el caso de uso de agregar una Reserva
    {
        if (!await validadorReserva.validarId(reserva.id, repositorioReserva)) //valida que la Reserva no exista
        {
            await repositorioReserva.agregarReserva(reserva);
        }else
        {
            throw new EntidadNotFoundException("La Reserva Que Intenta Agregar Ya Existe");
        }
    }
}