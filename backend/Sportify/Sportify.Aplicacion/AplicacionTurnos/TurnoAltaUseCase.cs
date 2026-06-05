namespace Sportify.Aplicacion.AplicacionTurnos;

using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionDeportes;
public class TurnoAltaUseCase(IRepositorioTurno repositorioTurno, IValidadorTurno validadorTurno, IRepositorioDeporte repoDeporte){

    public async Task Ejecutar(Turno turno) //ejecuta el caso de uso de crear un turno
    {
        turno.cupoMaximo = turno.cupo;
        var (valido, mensajeError) = await validadorTurno.validar(turno, repoDeporte); //valida que el turno cumpla con las reglas de negocio antes de crearlo
        if (!valido)
        {
            throw new ValidacionException(mensajeError);
        }
        if (await repositorioTurno.EncontrarRepetido(turno))
        {
            throw new EntidadRepetidaException("Ya existe un turno con el mismo deporte, fecha, hora de inicio y hora de fin.");
        }
        await repositorioTurno.AltaTurno(turno);
    }
}