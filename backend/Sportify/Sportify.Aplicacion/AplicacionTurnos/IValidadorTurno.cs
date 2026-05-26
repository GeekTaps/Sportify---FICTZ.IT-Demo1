namespace Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionDeportes;
public interface IValidadorTurno
{
    bool validar(Turno turno, IRepositorioDeporte repositorioDeporte, out string mensajeError); //firma del metodo para validar un turno, recibe el turno a validar, el repositorio de deportes para validar que el deporte asociado al turno exista, y un parametro de salida para devolver un mensaje de error en caso de que la validacion falle
}