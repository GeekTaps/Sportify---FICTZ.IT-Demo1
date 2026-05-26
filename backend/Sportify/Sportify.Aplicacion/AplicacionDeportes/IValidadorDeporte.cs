using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionTurnos;

namespace Sportify.Aplicacion.AplicacionDeportes;

public interface IValidadorDeporte
{
    public Task<bool> validarId(Guid idDeporte, IRepositorioDeporte repositorioDeporte);

    public Task<bool> validarNombre(string nombreDeporte, IRepositorioDeporte repositorioDeporte);

    public Task<bool> validarAsociacionConTurnos(Guid idDeporte, IRepositorioTurno repositorioTurno);
}