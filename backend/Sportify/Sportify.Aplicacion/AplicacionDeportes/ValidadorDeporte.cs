using System;

namespace Sportify.Aplicacion.AplicacionDeportes;

public class ValidadorDeporte : IValidadorDeporte
{
    public bool validarId(Guid idDeporte, IRepositorioDeporte repositorio)
    {
        return repositorio.existeDeporte(idDeporte);
    }

    public bool validarNombre(string nombreDeporte, IRepositorioDeporte repositorio)
    {
        if (string.IsNullOrEmpty(nombreDeporte))
        {
            return false;
        }
        return repositorio.existeDeportePorNombre(nombreDeporte);
    }
}