namespace Sportify.Aplicacion.AplicacionAsistencias;
using System;
using Sportify.Dominio.Asistencias;
public class ListarAsistenciasDeClase
{
    private readonly IRepositorioAsistencias repositorioAsistencias;

    public ListarAsistenciasDeClase(IRepositorioAsistencias repositorioAsistencias)
    {
        this.repositorioAsistencias = repositorioAsistencias;
    }

    public async Task<List<Asistencia>> Ejecutar(Guid idTurno)
    {
        return await repositorioAsistencias.ListarAsistenciasPorClase(idTurno);
    }
}