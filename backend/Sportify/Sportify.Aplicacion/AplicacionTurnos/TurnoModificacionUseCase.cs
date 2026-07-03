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
            await repositorioTurno.BuscarTurnoPorId(idTurno); // Just checking existence
            // Actually, we need to fetch the original turno to calculate inscriptos.
            var turnosExistentes = await repositorioTurno.ListarTurnos();
            var turnoOriginal = turnosExistentes.FirstOrDefault(t => t.Id == idTurno);
            if (turnoOriginal != null)
            {
                turno.Precio = turnoOriginal.Precio;

                int inscriptos = turnoOriginal.cupoMaximo - turnoOriginal.cupo;
                // Since the frontend sends the max capacity in 'cupo' property
                int intendedMax = turno.cupo;
                if (intendedMax < inscriptos)
                {
                    throw new ValidacionException($"El cupo máximo no puede ser menor a la cantidad de inscriptos ({inscriptos})");
                }
                turno.cupoMaximo = intendedMax;
                turno.cupo = intendedMax - inscriptos;
            }
            await repositorioTurno.ModificarTurno(turno, idTurno);
        }
        else
        {
            throw new ValidacionException(mensajeError);
        }
    }
}