namespace Sportify.Aplicacion.AplicacionAsistencias;
using Sportify.Dominio.Asistencias;
using Sportify.Aplicacion.AplicacionTurnos;
public class AsistenciaListarAsistenciasDeUsuarioUseCase
{
    private readonly IRepositorioAsistencias repositorioAsistencias;
    private readonly IRepositorioTurno repositorioTurno;

    public AsistenciaListarAsistenciasDeUsuarioUseCase(IRepositorioAsistencias repositorioAsistencias, IRepositorioTurno repositorioTurno)
    {
        this.repositorioAsistencias = repositorioAsistencias;
        this.repositorioTurno = repositorioTurno;
    }

    public async Task<List<Asistencia>> Ejecutar(Guid idUsuario)
    {   
        List<Asistencia> asistencias = await repositorioAsistencias.ListarAsistenciasPorUsuario(idUsuario);
        return await repositorioTurno.FiltrarAsistencias(asistencias);
    }
}