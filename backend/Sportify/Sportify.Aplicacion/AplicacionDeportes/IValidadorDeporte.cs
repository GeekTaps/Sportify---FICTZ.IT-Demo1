using System;
using System.Diagnostics;

namespace Sportify.Aplicacion.AplicacionDeportes;

public interface IValidadorDeporte
{
    public bool validarId(Guid idDeporte, IRepositorioDeporte repositorioDeporte);

    public bool validarNombre(string nombreDeporte, IRepositorioDeporte repositorioDeporte);
}