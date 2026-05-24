using System;

namespace Sportify.Aplicacion.AplicacionDeportes;

public class DeporteBajaUseCase
{
    IRepositorioDeporte repositorioDeporte;
    IValidadorDeporte validadorDeporte;

    public DeporteBajaUseCase(IRepositorioDeporte repositorioDeporte, IValidadorDeporte validadorDeporte)
    {
        this.repositorioDeporte = repositorioDeporte;
        this.validadorDeporte = validadorDeporte;
    }

    public void Ejecutar(Guid idDeporte) //ejecuta el caso de uso de eliminar un deporte
    {
        if (validadorDeporte.validarId(idDeporte, repositorioDeporte)) //valida que el deporte exista antes de eliminarlo
        {
           // if () { //valida que el deporte no esté asociado a ningún turno antes de eliminarlo    
                repositorioDeporte.eliminarDeporte(idDeporte);
            //} else { }
        }else
        {
            throw new EntidadNotFoundException("El Deporte Que Intenta Eliminar No Existe");
        }
    }
}