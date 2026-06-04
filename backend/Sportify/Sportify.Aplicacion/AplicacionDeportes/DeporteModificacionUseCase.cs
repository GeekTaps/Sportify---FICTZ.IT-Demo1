using System;
using System.Threading.Tasks;

namespace Sportify.Aplicacion.AplicacionDeportes;

public class DeporteModificacionUseCase
{
    IRepositorioDeporte repositorioDeporte;
    IValidadorDeporte validadorDeporte;

    public DeporteModificacionUseCase(IRepositorioDeporte repositorioDeporte, IValidadorDeporte validadorDeporte)
    {
        this.repositorioDeporte = repositorioDeporte;
        this.validadorDeporte = validadorDeporte;
    }

    public async Task Ejecutar(Guid idDeporte, string nuevaDescripcion) //modifica solo la descripción de un deporte existente
    {
        if (await validadorDeporte.validarId(idDeporte, repositorioDeporte)) //valida que el deporte con el ID proporcionado exista antes de modificarlo
        {
            await repositorioDeporte.modificarDeporte(idDeporte, nuevaDescripcion);
        }
        else
        {
            throw new EntidadNotFoundException("El deporte con el ID proporcionado no existe.");
        }
    }
}