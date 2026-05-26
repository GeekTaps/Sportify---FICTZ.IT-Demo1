using System;
using System.Threading.Tasks;

namespace Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Aplicacion.AplicacionTurnos;


public class DeporteBajaUseCase
{
    IRepositorioDeporte repositorioDeporte;
    IRepositorioTurno repositorioTurno;
    IValidadorDeporte validadorDeporte;

    public DeporteBajaUseCase(IRepositorioDeporte repositorioDeporte, IRepositorioTurno repositorioTurno, IValidadorDeporte validadorDeporte)
    {
        this.repositorioDeporte = repositorioDeporte;
        this.repositorioTurno = repositorioTurno;
        this.validadorDeporte = validadorDeporte;
    }

    public async Task Ejecutar(Guid idDeporte) //ejecuta el caso de uso de eliminar un deporte
    {
        if (await validadorDeporte.validarId(idDeporte, repositorioDeporte)) //valida que el deporte exista antes de eliminarlo
        {
           if (await validadorDeporte.validarAsociacionConTurnos(idDeporte, repositorioTurno)) //valida que el deporte no esté asociado a ningún turno antes de eliminarlo
            {
                await repositorioDeporte.eliminarDeporte(idDeporte);
            } else
            {
                throw new EntidadAsociadaException("El Deporte Que Intenta Eliminar Está Asociado A Un Turno, Elimine Primero El/ Los Turnos Asociados");
            }
        }else
        {
            throw new EntidadNotFoundException("El Deporte Que Intenta Eliminar No Existe");
        }
    }
}