namespace Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
public interface IRepositorioTurno
{
    void AltaTurno(Turno turno); //firma del metodo para crear un turno
    bool ModificarTurno(Turno turno, Guid idTurno); //firma del metodo para modificar un turno
    bool BuscarTurnoPorId(Guid idTurno); //firma del metodo para obtener un turno por su id
    }  