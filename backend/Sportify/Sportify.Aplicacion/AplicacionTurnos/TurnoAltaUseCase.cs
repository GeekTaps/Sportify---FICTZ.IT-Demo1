namespace Sportify.Aplicacion.AplicacionTurnos;

using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionDeportes;   
public class TurnoAltaUseCase(IRepositorioTurno repositorioTurno, IValidadorTurno validadorTurno, IRepositorioDeporte repoDeporte){

    public void Ejecutar(Turno turno) //ejecuta el caso de uso de crear un turno
    {
        if (validadorTurno.validar(turno, repoDeporte, out string mensajeError)) //valida que el turno cumpla con las reglas de negocio antes de crearlo
        {
            repositorioTurno.AltaTurno(turno);
        }else
        {
            throw new ValidacionException("mensajeError");
        }
    }
}