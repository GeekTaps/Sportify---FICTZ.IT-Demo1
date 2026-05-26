namespace Sportify.Aplicacion.AplicacionTurnos;
using System.Threading.Tasks;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Aplicacion.Excepciones;
public class TurnoModificacionUseCase (IRepositorioTurno repositorioTurno, IRepositorioDeporte repoDeporte, IValidadorTurno validadorTurno)
{
    public async Task Ejecutar(Turno turno, Guid idTurno) //ejecuta el caso de uso de modificar un turno
    {
        var (valido, mensajeError) = await validadorTurno.validar(turno, repoDeporte); //valida que el turno cumpla con las reglas de negocio antes de modificarlo
        if (valido)
        {
            await repositorioTurno.ModificarTurno(turno, idTurno);
        }
        else
        {
            throw new ValidacionException(mensajeError);
        }
    }
}