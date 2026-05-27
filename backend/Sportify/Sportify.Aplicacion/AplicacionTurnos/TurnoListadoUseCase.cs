namespace Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;

public class TurnoListadoUseCase(IRepositorioTurno repositorioTurno)
{
    public async Task<List<Turno>> Ejecutar()
    {
        return await repositorioTurno.ListarTurnos();
    }
}