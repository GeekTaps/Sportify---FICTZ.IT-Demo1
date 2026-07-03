namespace Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Dominio.Turnos;
using System;
using System.Threading.Tasks;

public interface IRepositorioHorario
{
    Task<Horario> CrearYGuardarHorario(Guid idDeporte, string diaSemana, TimeOnly horaInicio);
    Task<Horario?> ObtenerHorarioPorId(Guid id);
    Task<bool> ExisteHorario(Guid idDeporte, string diaSemana, TimeOnly horaInicio);
}
