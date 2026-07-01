namespace Sportify.Aplicacion.AplicacionAsistencias;
using System;
using Sportify.Dominio.Asistencias;
public class AsistenciaAlta
{
    private readonly IRepositorioAsistencias repositorioAsistencias;

    public AsistenciaAlta(IRepositorioAsistencias repositorioAsistencias)
    {
        this.repositorioAsistencias = repositorioAsistencias;
    }

    public async Task<bool> Ejecutar(Asistencia asistencia)
    {
        return await repositorioAsistencias.AltaAsistencia(asistencia);
    }
}