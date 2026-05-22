namespace Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Aplicacion.Excepciones;
public class TurnoModificacionUseCase (IRepositorioTurno repositorioTurno, IRepositorioDeporte repoDeporte, IValidadorTurno validadorTurno)
{
    public void Ejecutar(Turno turno, Guid idTurno) //ejecuta el caso de uso de modificar un turno
    {
        if (validadorTurno.validar(turno, repoDeporte, out string mensajeError)) //valida que el turno cumpla con las reglas de negocio antes de modificarlo
        {
            repositorioTurno.ModificarTurno(turno, idTurno);
        }
        else
        {
            throw new ValidacionException(mensajeError);
        }
    }
}