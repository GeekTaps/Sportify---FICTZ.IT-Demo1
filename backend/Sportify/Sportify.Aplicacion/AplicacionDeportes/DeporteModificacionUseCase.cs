using System;

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

    public void Ejecutar(Guid idDeporte, string nuevoNombre, string nuevaDescripcion) //modifica los datos de un deporte existente
    {
        if (validadorDeporte.validarId(idDeporte, repositorioDeporte)) //valida que el deporte con el ID proporcionado exista antes de modificarlo
        {
            if (!validadorDeporte.validarNombre(nuevoNombre, repositorioDeporte)) //valida que el nuevo nombre del deporte no sea el de otro deporte antes de modificarlo
            {
                repositorioDeporte.modificarDeporte(idDeporte, nuevoNombre, nuevaDescripcion);
            }else
            {
                throw new Exception("Ya Existe Un Deporte Con El Nombre Que Intenta Asignar");
            }

        }
        else
        {
            throw new EntidadNotFoundException("El deporte con el ID proporcionado no existe.");
        }
    }
}