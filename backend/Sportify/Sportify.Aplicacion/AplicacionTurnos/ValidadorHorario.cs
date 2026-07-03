using System;
using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;

namespace Sportify.Aplicacion.AplicacionTurnos;

public class ValidadorHorario : IValidadorHorario
{
    public async Task ValidarCreacionHorario(Guid idDeporte, string diaSemana, TimeOnly horaInicio, IRepositorioHorario repositorioHorario)
    {
        bool existe = await repositorioHorario.ExisteHorario(idDeporte, diaSemana, horaInicio);
        if (existe)
        {
            throw new EntidadRepetidaException("Ya existe un turno de ese deporte para ese horario");
        }
    }
}
