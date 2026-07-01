namespace Sportify.Aplicacion.AplicacionAsistencias;
using System;
using Sportify.Dominio.Asistencias;
public class AsistenciaPasarPresente
{
    private readonly IRepositorioAsistencias repositorioAsistencias;

    public AsistenciaPasarPresente(IRepositorioAsistencias repositorioAsistencias)
    {
        this.repositorioAsistencias = repositorioAsistencias;
    }

    public async Task<bool> Ejecutar(Guid idUsuario, Guid idTurno)
    {
        if (!await repositorioAsistencias.BuscarAsistencia(idUsuario, idTurno))
        {
            throw new EntidadNotFoundException("No se encontró la asistencia para el usuario y turno especificados.");
        }
        return await repositorioAsistencias.PasarAsistencia(idUsuario, idTurno);
    }
}