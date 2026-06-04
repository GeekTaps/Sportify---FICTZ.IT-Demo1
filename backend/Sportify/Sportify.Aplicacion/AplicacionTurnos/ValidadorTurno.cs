namespace Sportify.Aplicacion.AplicacionTurnos;
using System;
using System.Threading.Tasks;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionDeportes;

public class ValidadorTurno : IValidadorTurno
{
    public async Task<(bool valido, string mensajeError)> validar(Turno turno, IRepositorioDeporte reposDeporte)
    {
        string mensajeError = "";
        if (turno.cupo < 0)
        {
            mensajeError = "El cupo del turno no puede ser negativo";
        }
        if (turno.Fecha < DateTime.Now)
        {
            mensajeError += "La fecha del turno no puede ser anterior a la fecha y hora actual";
        }
        /*
        if (turno.horaFin <= turno.horaInicio)
        {
            mensajeError += "La hora de fin del turno debe ser posterior a la hora de inicio";
        }
        if (turno.horaFin - turno.horaInicio != TimeSpan.FromHours(1))
        {
            mensajeError += "La duracion del turno debe ser de 1 hora";
        }
        */
        if (turno.nombreTurno == "")
        {
            mensajeError += "El nombre del turno no puede estar vacio";
        }
        if (turno.nommbreProfesor == "")
        {
            mensajeError += "Debe asignar un profesor al turno";
        }
        if (!await reposDeporte.existeDeporte(turno.IdDeporte))
        {
            mensajeError += "El deporte asociado al turno no existe";
        }
        return (mensajeError == "", mensajeError); //devuelve tupla con booleano y mensaje de error
    }
}