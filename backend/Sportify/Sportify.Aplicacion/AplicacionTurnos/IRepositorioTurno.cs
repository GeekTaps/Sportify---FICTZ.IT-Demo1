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
    Task<List<Turno>> ListarTurnos();
    Task<bool> EncontrarRepetido(Turno nuevoTurno); //firma del metodo para verificar si hay un turno repetido (mismo deporte, fecha, hora de inicio y hora de fin)
    Task<bool> BajaTurno(Guid idTurno); //firma del metodo para eliminar un turno por su id
    Task actualizarMostrarEnHome(); //firma del metodo para actualizar el campo mostrarEnHome de los turnos, estableciendo en false aquellos turnos cuya fecha y hora de inicio ya hayan pasado
}  