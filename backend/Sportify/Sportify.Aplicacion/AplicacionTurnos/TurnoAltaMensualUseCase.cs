using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionDeportes;

namespace Sportify.Aplicacion.AplicacionTurnos
{
    public class TurnoAltaMensualUseCase
    {
        private readonly IRepositorioTurno repositorioTurno;
        private readonly IRepositorioDeporte repositorioDeporte;

        public TurnoAltaMensualUseCase(IRepositorioTurno repositorioTurno, IRepositorioDeporte repositorioDeporte)
        {
            this.repositorioTurno = repositorioTurno;
            this.repositorioDeporte = repositorioDeporte;
        }

        public async Task Ejecutar(Guid idDeporte, string fechaInicioStr, string horaInicioStr, int cupo, double precio, string nombreProfesor, bool listaEsperaHabilitada)
        {
            // Validar campos vacíos
            if (idDeporte == Guid.Empty || 
                string.IsNullOrWhiteSpace(fechaInicioStr) || 
                string.IsNullOrWhiteSpace(horaInicioStr) || 
                string.IsNullOrWhiteSpace(nombreProfesor))
            {
                throw new ValidacionException("No puede haber campos en blanco");
            }

            if (!DateTime.TryParse(fechaInicioStr, out DateTime fechaInicio))
            {
                throw new ValidacionException("Fecha de inicio inválida");
            }

            if (fechaInicio.Date < DateTime.Today)
            {
                throw new ValidacionException("No puede elegir un día anterior al actual");
            }

            // Validar cupo y precio
            if (cupo <= 0)
            {
                throw new ValidacionException("El cupo debe ser mayor a 0");
            }
            if (precio < 0)
            {
                throw new ValidacionException("El precio no puede ser negativo");
            }

            // Validar parsing de hora
            if (!TimeOnly.TryParse(horaInicioStr, out TimeOnly horaInicio))
            {
                throw new ValidacionException("Formato de hora inválido");
            }

            TimeOnly horaFin = horaInicio.AddHours(1);

            // Validar si es hoy, que la hora no haya pasado
            var fechaConHoraExacta = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, horaInicio.Hour, horaInicio.Minute, 0);
            if (fechaConHoraExacta < DateTime.Now)
            {
                throw new ValidacionException("La hora del turno debe ser posterior a la hora actual");
            }

            DayOfWeek diaSemanaObj = fechaInicio.DayOfWeek;

            // Validar deporte existente
            var deportes = await repositorioDeporte.ListarDeportes();
            var deporteObj = deportes.FirstOrDefault(d => d.id == idDeporte);
            if (deporteObj == null)
            {
                throw new ValidacionException("El deporte no existe.");
            }

            var fechasDelMes = new List<DateTime>();
            
            // CREACIÓN INDIVIDUAL
            fechasDelMes.Add(fechaInicio.Date);

            /*
            // CREACIÓN MENSUAL COMENTADA:
            var startDate = fechaInicio.Date;
            for (int i = 0; i <= 30; i++)
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
            */

            if (!fechasDelMes.Any())
            {
                throw new ValidacionException("No se encontraron fechas para el día seleccionado.");
            }

            // Generar los objetos Turno
            var turnosNuevos = new List<Turno>();
            foreach (var fecha in fechasDelMes)
            {
                // Combinar la fecha con la hora para guardar
                var fechaConHora = new DateTime(fecha.Year, fecha.Month, fecha.Day, horaInicio.Hour, horaInicio.Minute, 0);

                var turno = new Turno
                {
                    Id = Guid.NewGuid(),
                    IdDeporte = idDeporte,
                    Fecha = fechaConHora,
                    horaInicio = horaInicio,
                    horaFin = horaFin,
                    cupo = cupo,
                    cupoMaximo = cupo,
                    Precio = precio,
                    nombreTurno = $"{deporteObj.nombre} - {fechaConHora:dd/MM/yy} - {horaInicio:HH:mm}hs",
                    nommbreProfesor = nombreProfesor,
                    ListaEsperaHabilitada = listaEsperaHabilitada
                };
                
                turnosNuevos.Add(turno);
            }

            // Chequear repeticiones
            var turnosExistentes = await repositorioTurno.ListarTurnos();
            foreach (var nuevo in turnosNuevos)
            {
                // Un turno se considera repetido si tiene el mismo deporte, fecha y hora de inicio
                bool existe = turnosExistentes.Any(t => 
                    t.IdDeporte == nuevo.IdDeporte && 
                    t.Fecha.Date == nuevo.Fecha.Date && 
                    t.horaInicio == nuevo.horaInicio);

                if (existe)
                {
                    throw new EntidadRepetidaException("Ya hay un turno de ese deporte para ese horario");
                }
            }

            // Guardar todos los turnos
            foreach (var nuevo in turnosNuevos)
            {
                await repositorioTurno.AltaTurno(nuevo);
            }
        }
    }
}
