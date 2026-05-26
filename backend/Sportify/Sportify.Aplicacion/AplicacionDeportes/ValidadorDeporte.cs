using System;
using System.Threading.Tasks;

namespace Sportify.Aplicacion.AplicacionDeportes;
using Sportify.Aplicacion.AplicacionTurnos;


public class ValidadorDeporte : IValidadorDeporte
{
    public async Task<bool> validarId(Guid idDeporte, IRepositorioDeporte repositorio)
    {
        return await repositorio.existeDeporte(idDeporte);
    }

    public async Task<bool> validarNombre(string nombreDeporte, IRepositorioDeporte repositorio)
    {
        if (string.IsNullOrEmpty(nombreDeporte))
        {
            return false;
        }
        return !await repositorio.existeDeportePorNombre(nombreDeporte);
    }

    public async Task<bool> validarNombre(string nombreDeporte, Guid? excludeId, IRepositorioDeporte repositorio)
    {
        if (string.IsNullOrEmpty(nombreDeporte))
        {
            return false;
        }
        var existente = await repositorio.ObtenerPorNombre(nombreDeporte);
        if (existente == null || existente.id == excludeId) // Si no existe o es el mismo deporte que se está modificando, es válido
        {
            return true;
        }
        return false;
    }

    public async Task<bool> validarAsociacionConTurnos(Guid idDeporte, IRepositorioTurno repositorioTurno)
    {
        return !await repositorioTurno.existeTurnoAsociadoAlDeporte(idDeporte);
    }
}