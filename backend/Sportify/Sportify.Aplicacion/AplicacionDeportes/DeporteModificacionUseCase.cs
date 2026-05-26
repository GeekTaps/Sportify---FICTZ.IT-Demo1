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

    public async Task Ejecutar(Guid idDeporte, string nuevoNombre, string nuevaDescripcion) //modifica los datos de un deporte existente
    {
        if (await validadorDeporte.validarId(idDeporte, repositorioDeporte)) //valida que el deporte con el ID proporcionado exista antes de modificarlo
        {
            // Comprobar si existe otro deporte con el mismo nombre
            if (await validadorDeporte.validarNombre(nuevoNombre, idDeporte, repositorioDeporte))
            {
                await repositorioDeporte.modificarDeporte(idDeporte, nuevoNombre, nuevaDescripcion);
            }
            else
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