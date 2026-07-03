using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionAsistencias;
using Sportify.Dominio.Asistencias;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaAltaUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;
    IRepositorioAsistencias repositorioAsistencias;

    public ReservaAltaUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva , IRepositorioAsistencias repositorioAsistencias)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
        this.repositorioAsistencias = repositorioAsistencias;
    }

    public async Task Ejecutar(Reserva reserva) //ejecuta el caso de uso de agregar una Reserva
    {
        if (!await validadorReserva.validarId(reserva.id, repositorioReserva)) //valida que la Reserva no exista
        {
            await repositorioReserva.agregarReserva(reserva);
            await repositorioAsistencias.AltaAsistencia(new Asistencia
            {
                IdUsuario = reserva.idUsuario,
                IdTurno = reserva.idTurno,
                Presente = false
            });
        }else
        {
            throw new EntidadNotFoundException("La Reserva Que Intenta Agregar Ya Existe");
        }
    }
}