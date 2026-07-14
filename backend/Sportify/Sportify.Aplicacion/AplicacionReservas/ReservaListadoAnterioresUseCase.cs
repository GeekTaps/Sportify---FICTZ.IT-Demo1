using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionAsistencias;
using System.Linq;

namespace Sportify.Aplicacion.AplicacionReservas;

public class ReservaListadoAnterioresUseCase
{
    IRepositorioReserva repositorioReserva;
    IValidadorReserva validadorReserva;
    IRepositorioTurno repositorioTurno;
    IRepositorioAsistencias repositorioAsistencias;

    public ReservaListadoAnterioresUseCase(IRepositorioReserva repositorioReserva, IValidadorReserva validadorReserva, IRepositorioTurno repositorioTurno, IRepositorioAsistencias repositorioAsistencias)
    {
        this.repositorioReserva = repositorioReserva;
        this.validadorReserva = validadorReserva;
        this.repositorioTurno = repositorioTurno;
        this.repositorioAsistencias = repositorioAsistencias;
    }

    public async Task<List<Reserva>> Ejecutar(Guid idUsuario)
    {
        List<Reserva> reservas = await repositorioReserva.listarReservasUsuario(idUsuario);
        var turnos = await repositorioTurno.ListarTurnos();

        List<Reserva> reservasAnteriores = reservas.Where(r => {
            if (r.eliminada) return true;
            var turno = turnos.FirstOrDefault(t => t.Id == r.idTurno);
            if (turno == null) return true;
            var fechaTurno = turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan());
            return fechaTurno.AddHours(1) <= DateTime.Now;
        })
        .OrderByDescending(r => {
            var turno = turnos.FirstOrDefault(t => t.Id == r.idTurno);
            return turno != null ? turno.Fecha.Date.Add(turno.horaInicio.ToTimeSpan()) : r.fecha;
        })
        .ToList();

        if (reservasAnteriores.Count == 0) {
            throw new ListadoVacioException("No Contás Con Reservas Anteriores");
        }

        foreach (var r in reservasAnteriores)
        {
            r.Asistio = await repositorioAsistencias.AsistioATurno(idUsuario, r.idTurno);
        }

        return reservasAnteriores;
    }
}