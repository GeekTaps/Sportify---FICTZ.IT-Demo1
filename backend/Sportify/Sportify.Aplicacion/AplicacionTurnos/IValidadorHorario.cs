using System;
using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;

namespace Sportify.Aplicacion.AplicacionTurnos;

public interface IValidadorHorario
{
    Task ValidarCreacionHorario(Guid idDeporte, string diaSemana, TimeOnly horaInicio, IRepositorioHorario repositorioHorario);
}
