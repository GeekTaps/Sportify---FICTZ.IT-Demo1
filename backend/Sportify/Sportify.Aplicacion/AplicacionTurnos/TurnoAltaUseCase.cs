namespace Sportify.Aplicacion.AplicacionTurnos;

using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionDeportes;
public class TurnoAltaUseCase(IRepositorioTurno repositorioTurno, IValidadorTurno validadorTurno, IRepositorioDeporte repoDeporte){

    public async Task Ejecutar(Turno turno) //ejecuta el caso de uso de crear un turno
    {
        var (valido, mensajeError) = await validadorTurno.validar(turno, repoDeporte); //valida que el turno cumpla con las reglas de negocio antes de crearlo
        if (valido)
        {
            await repositorioTurno.AltaTurno(turno);
        }
        else
        {
            throw new ValidacionException(mensajeError);
        }
    }
}