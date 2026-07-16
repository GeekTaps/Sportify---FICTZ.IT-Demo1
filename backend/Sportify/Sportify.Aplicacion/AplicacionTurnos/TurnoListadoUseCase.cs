namespace Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
public class TurnoListadoUseCase(IRepositorioTurno repositorioTurno)
{
    public async Task<List<Turno>> Ejecutar()
    {
        var turnos = await repositorioTurno.ListarTurnos();
        return turnos.OrderBy(t => t.Fecha).ThenBy(t => t.horaInicio).ToList();
    }
}