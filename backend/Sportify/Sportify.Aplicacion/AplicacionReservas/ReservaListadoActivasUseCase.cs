using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionTurnos;
using System.Linq;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaListadoActivasUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;
    IRepositorioTurno repositorioTurno;

    public ReservaListadoActivasUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva, IRepositorioTurno repositorioTurno)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
        this.repositorioTurno = repositorioTurno;
    }

    public async Task<List<Reserva>> Ejecutar(Guid idUsuario)
    {
        List<Reserva> reservas = await repositorioReserva.listarReservasUsuario(idUsuario);
        var turnos = await repositorioTurno.ListarTurnos();
        List<Reserva> reservasActivas = reservas.Where(r => {
            if (r.eliminada) return false;
            var turno = turnos.FirstOrDefault(t => t.Id == r.idTurno);
            if (turno == null) return false;
            var fechaTurno = turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan());
            return fechaTurno.AddHours(1) > DateTime.Now;
        })
        .OrderBy(r => {
            var turno = turnos.First(t => t.Id == r.idTurno);
            return turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan());
        })
        .ToList();
        return reservasActivas;
    }
}