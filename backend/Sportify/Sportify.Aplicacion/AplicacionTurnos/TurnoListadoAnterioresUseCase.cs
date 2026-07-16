using Sportify.Dominio.Turnos;

namespace Sportify.Aplicacion.AplicacionTurnos;

public class TurnoListadoAnterioresUseCase
{
    private readonly IRepositorioTurno repositorioTurno;

    public TurnoListadoAnterioresUseCase(IRepositorioTurno repositorioTurno)
    {
        this.repositorioTurno = repositorioTurno;
    }

    public async Task<List<Turno>> Ejecutar()
    {
        var turnos = await repositorioTurno.ListarTurnos();
        var ahora = DateTime.Now;

        return turnos
            .Where(t => t.Fecha.Date.Add(t.horaInicio.ToTimeSpan()) <= ahora)
            .OrderByDescending(t => t.Fecha)
            .ThenByDescending(t => t.horaInicio)
            .ToList();
    }
}
