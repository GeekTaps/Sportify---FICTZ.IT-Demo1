using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Reservas;
using Sportify.Dominio.Turnos;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;

public class EntrarListaTurnoUseCase
{
    IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno;
    IValidadorListaDeEsperaTurno validadorListaDeEsperaTurno;
    IRepositorioReserva repositorioReserva;
    IRepositorioTurno repositorioTurno;

    public EntrarListaTurnoUseCase(
        IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno, 
        IValidadorListaDeEsperaTurno validadorListaDeEsperaTurno,
        IRepositorioReserva repositorioReserva,
        IRepositorioTurno repositorioTurno)
    {
        this.repositorioListaDeEsperaTurno = repositorioListaDeEsperaTurno;
        this.validadorListaDeEsperaTurno = validadorListaDeEsperaTurno;
        this.repositorioReserva = repositorioReserva;
        this.repositorioTurno = repositorioTurno;
    }

    // ejecuta el caso de uso de agregar una entrada a la lista de espera de un turno
    public async Task Ejecutar(Guid idUsuario, Guid idTurno) 
    {
        // Validar reservas previas
        var turnoDeseado = await repositorioTurno.ObtenerTurnoPorId(idTurno);
        if (turnoDeseado == null) throw new EntidadNotFoundException("Turno no encontrado");

        var reservasUsuario = await repositorioReserva.listarReservasUsuario(idUsuario);
        foreach (var reserva in reservasUsuario)
        {
            if (reserva.eliminada) continue;

            if (reserva.idTurno == idTurno)
            {
                throw new EntidadNotFoundException("Ya reservaste este turno");
            }

            var turnoReservado = await repositorioTurno.ObtenerTurnoPorId(reserva.idTurno);
            if (turnoReservado != null && turnoReservado.Fecha.Date == turnoDeseado.Fecha.Date)
            {
                // se fija si el horario se superpone con el de otro turno reservado
                if (turnoReservado.horaInicio < turnoDeseado.horaFin && turnoReservado.horaFin > turnoDeseado.horaInicio)
                {
                    throw new EntidadNotFoundException("Ya tenés una reserva en este mismo horario");
                }
            }
        }

        //valida que la entrada no exista
        if (await validadorListaDeEsperaTurno.validarAgregarEnEspera(idUsuario, idTurno, repositorioListaDeEsperaTurno)) 
        {
            await repositorioListaDeEsperaTurno.agregarEnEspera(idUsuario, idTurno);
        }else
        {
            throw new EntidadNotFoundException("Ya estás en la lista de espera");
        }
    }
}