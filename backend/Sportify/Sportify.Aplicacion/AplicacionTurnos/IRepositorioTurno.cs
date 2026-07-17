namespace Sportify.Aplicacion.AplicacionTurnos;
using System;
using System.Threading.Tasks;
using Sportify.Dominio.Turnos;
public interface IRepositorioTurno
{
    Task AltaTurno(Turno turno); //firma del metodo para crear un turno
    Task<bool> ModificarTurno(Turno turno, Guid idTurno); //firma del metodo para modificar un turno
    Task<bool> BuscarTurnoPorId(Guid idTurno); //firma del metodo para obtener un turno por su id
    Task<Turno?> ObtenerTurnoPorId(Guid idTurno);
    Task<bool> existeTurnoAsociadoAlDeporte(Guid idDeporte); //firma del metodo para verificar si un deporte tiene turnos asociados
    Task<List<Turno>> ListarTurnos();
    Task<List<Turno>> ListarTurnosPorHorario(Guid idHorario);
    Task<bool> EncontrarRepetido(Turno nuevoTurno); //firma del metodo para verificar si hay un turno repetido (mismo deporte, fecha, hora de inicio y hora de fin)
    Task<bool> BajaTurno(Guid idTurno);
    Task<Turno> TraerTurnoPorId(Guid idTurno); //firma del metodo para obtener un turno por su id
    Task actualizarMostrarEnHome(); //firma del metodo para actualizar el campo mostrarEnHome de los turnos, se ejecuta cada vez que se obtiene el listado de turnos, para mostrar solo los turnos que corresponden en la pagina principal (home)
    Task<bool> HayLugarParaAbono(Guid idHorario); //firma del metodo para verificar que todos los turnos de una misma actividad (deporte, dia de semana y horario) tengan cupo disponible, para poder mandarles el mail a los de la lista de espera.
}  