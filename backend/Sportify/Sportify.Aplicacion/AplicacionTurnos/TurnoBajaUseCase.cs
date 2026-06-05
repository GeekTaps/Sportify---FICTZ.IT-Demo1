using System;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.Excepciones;

namespace Sportify.Aplicacion.AplicacionTurnos;

public class TurnoBajaUseCase
{
    private readonly IRepositorioTurno repositorioTurno;
    private readonly IRepositorioReserva repositorioReserva;

    public TurnoBajaUseCase(IRepositorioTurno repositorioTurno, IRepositorioReserva repositorioReserva)
    {
        this.repositorioTurno = repositorioTurno;
        this.repositorioReserva = repositorioReserva;
    }

    public async Task Ejecutar(Guid idTurno)
    {
        var existe = await repositorioTurno.BuscarTurnoPorId(idTurno);
        if (!existe)
        {
            throw new EntidadNotFoundException("El Turno que intenta eliminar no existe.");
        }

        var reservasCount = await repositorioReserva.ContarReservasPorTurno(idTurno);
        if (reservasCount > 0)
        {
            throw new EntidadAsociadaException("No se puede eliminar el turno porque tiene reservas asociadas.");
        }

        await repositorioTurno.BajaTurno(idTurno);
    }
}
