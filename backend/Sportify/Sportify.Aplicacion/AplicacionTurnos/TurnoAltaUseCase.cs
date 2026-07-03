using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionDeportes;

namespace Sportify.Aplicacion.AplicacionTurnos;

public class TurnoAltaUseCase(
    IRepositorioTurno repositorioTurno, 
    IValidadorTurno validadorTurno, 
    IRepositorioDeporte repoDeporte,
    IRepositorioHorario repositorioHorario,
    IValidadorHorario validadorHorario)
{
    public async Task Ejecutar(
        Guid idDeporte, 
        string diaSemana, 
        TimeOnly horaInicio, 
        TimeOnly horaFin, 
        int cupo, 
        double precio, 
        string nombreProfesor, 
        bool listaEsperaHabilitada)
    {
        // Validar que el Horario no exista previamente
        await validadorHorario.ValidarCreacionHorario(idDeporte, diaSemana, horaInicio, repositorioHorario);

        // Delegar la creación del Horario al repositorio
        var horario = await repositorioHorario.CrearYGuardarHorario(idDeporte, diaSemana, horaInicio);

        // Calcular la primera fecha que coincida con diaSemana
        var dayOfWeekMap = new Dictionary<string, DayOfWeek>
        {
            { "Sunday", DayOfWeek.Sunday },
            { "Monday", DayOfWeek.Monday },
            { "Tuesday", DayOfWeek.Tuesday },
            { "Wednesday", DayOfWeek.Wednesday },
            { "Thursday", DayOfWeek.Thursday },
            { "Friday", DayOfWeek.Friday },
            { "Saturday", DayOfWeek.Saturday }
        };

        if (!dayOfWeekMap.TryGetValue(diaSemana, out DayOfWeek diaSemanaObj))
        {
            throw new ValidacionException("El día de la semana ingresado no es válido.");
        }

        var startDate = DateTime.Today;
        while (startDate.DayOfWeek != diaSemanaObj)
        {
            startDate = startDate.AddDays(1);
        }

        var fechasDelMes = new List<DateTime>();

        for (int i = 0; i < 30; i++)
        {
            var dt = startDate.AddDays(i);
            if (dt.DayOfWeek == diaSemanaObj)
            {
                var fechaConHora = new DateTime(dt.Year, dt.Month, dt.Day, horaInicio.Hour, horaInicio.Minute, 0);
                if (fechaConHora >= DateTime.Now)
                {
                    fechasDelMes.Add(dt);
                }
            }
        }

        if (!fechasDelMes.Any())
        {
            throw new ValidacionException("No se encontraron fechas válidas para el día seleccionado en los próximos 30 días.");
        }

        var turnosNuevos = new List<Turno>();

        var deporte = await repoDeporte.obtenerDeportePorId(idDeporte);
        if (deporte == null) throw new ValidacionException("El deporte seleccionado no existe.");

        foreach (var fecha in fechasDelMes)
        {
            var fechaConHora = new DateTime(fecha.Year, fecha.Month, fecha.Day, horaInicio.Hour, horaInicio.Minute, 0);

            var nuevoTurno = new Turno
            {
                Id = Guid.NewGuid(),
                IdDeporte = idDeporte,
                Fecha = fechaConHora,
                horaInicio = horaInicio,
                horaFin = horaFin,
                cupo = cupo,
                cupoMaximo = cupo,
                Precio = precio,
                nombreTurno = $"{deporte.nombre} - {fechaConHora:dd/MM/yy} - {horaInicio:HH:mm}hs",
                nommbreProfesor = nombreProfesor,
                ListaEsperaHabilitada = listaEsperaHabilitada,
                mostrarEnHome = true,
                IdHorario = horario.id
            };
            
            var (valido, mensajeError) = await validadorTurno.validar(nuevoTurno, repoDeporte);
            if (!valido)
            {
                throw new ValidacionException(mensajeError);
            }

            turnosNuevos.Add(nuevoTurno);
        }

        var turnosExistentes = await repositorioTurno.ListarTurnos();
        foreach (var nuevo in turnosNuevos)
        {
            bool existe = turnosExistentes.Any(t => 
                t.IdDeporte == nuevo.IdDeporte && 
                t.Fecha.Date == nuevo.Fecha.Date && 
                t.horaInicio == nuevo.horaInicio);

            if (existe)
            {
                throw new EntidadRepetidaException("Ya existe un turno de ese deporte para ese horario");
            }
        }

        foreach (var nuevo in turnosNuevos)
        {
            await repositorioTurno.AltaTurno(nuevo);
        }
    }
}