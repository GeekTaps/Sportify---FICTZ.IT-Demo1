namespace Sportify.Aplicacion.AplicacionAsistencias;
using Sportify.Dominio.Asistencias;
public class AsistenciaListarAsistenciasDeUsuarioUseCase
{
    private readonly IRepositorioAsistencias repositorioAsistencias;

    public AsistenciaListarAsistenciasDeUsuarioUseCase(IRepositorioAsistencias repositorioAsistencias)
    {
        this.repositorioAsistencias = repositorioAsistencias;
    }

    public async Task<List<Asistencia>> Ejecutar(Guid idUsuario)
    {
        return await repositorioAsistencias.ListarAsistenciasPorUsuario(idUsuario);
    }
}