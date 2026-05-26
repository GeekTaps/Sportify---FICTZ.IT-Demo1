namespace Sportify.Aplicacion.AplicacionTurnos;
using System;
using System.Threading.Tasks;
using Sportify.Dominio.Turnos;
public interface IRepositorioTurno
{
    Task AltaTurno(Turno turno); //firma del metodo para crear un turno
    Task<bool> ModificarTurno(Turno turno, Guid idTurno); //firma del metodo para modificar un turno
    Task<bool> BuscarTurnoPorId(Guid idTurno); //firma del metodo para obtener un turno por su id
    Task<bool> existeTurnoAsociadoAlDeporte(Guid idDeporte); //firma del metodo para verificar si un deporte tiene turnos asociados
}  