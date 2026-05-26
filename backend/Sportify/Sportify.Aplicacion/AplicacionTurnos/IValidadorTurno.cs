namespace Sportify.Aplicacion.AplicacionTurnos;
using System.Threading.Tasks;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionDeportes;
public interface IValidadorTurno
{
    Task<(bool valido, string mensajeError)> validar(Turno turno, IRepositorioDeporte repositorioDeporte); //firma del metodo para validar un turno, recibe el turno a validar y el repositorio de deportes para validar que el deporte asociado al turno exista
}