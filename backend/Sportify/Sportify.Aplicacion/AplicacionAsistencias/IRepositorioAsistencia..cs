namespace Sportify.Aplicacion.AplicacionAsistencias;
using System;
using System.Threading.Tasks;
using Sportify.Dominio.Asistencias;

public interface IRepositorioAsistencias
{
    Task<bool> AltaAsistencia(Asistencia asistencia);
    Task<bool> PasarAsistencia(Guid idUsuario, Guid idTurno);
    Task<bool> BuscarAsistencia(Guid idUsuario, Guid idTurno);
    Task<bool> AsistioATurno(Guid idUsuario, Guid idTurno);
    Task<List<Asistencia>> ListarAsistenciasPorClase(Guid idTurno);
}
