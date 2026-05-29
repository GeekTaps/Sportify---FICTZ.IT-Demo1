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

        public async Task Ejecutar(Guid idDeporte, string diaSemanaStr, string horaInicioStr, int cupo, double precio, string nombreProfesor, bool listaEsperaHabilitada)
        {
            // Validar campos vacíos
            if (idDeporte == Guid.Empty || 
                string.IsNullOrWhiteSpace(diaSemanaStr) || 
                string.IsNullOrWhiteSpace(horaInicioStr) || 
                string.IsNullOrWhiteSpace(nombreProfesor))
            {
                throw new ValidacionException("No puede haber campos en blanco");
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
                throw new ValidacionException("No puede haber campos en blanco"); // o formato de hora invalido
            }

            TimeOnly horaFin = horaInicio.AddHours(1);

            // Mapear el día de la semana
            DayOfWeek diaSemanaObj;
            switch (diaSemanaStr.ToLower())
            {
                case "lunes": diaSemanaObj = DayOfWeek.Monday; break;
                case "martes": diaSemanaObj = DayOfWeek.Tuesday; break;
                case "miércoles":
                case "miercoles": diaSemanaObj = DayOfWeek.Wednesday; break;
                case "jueves": diaSemanaObj = DayOfWeek.Thursday; break;
                case "viernes": diaSemanaObj = DayOfWeek.Friday; break;
                case "sábado":
                case "sabado": diaSemanaObj = DayOfWeek.Saturday; break;
                case "domingo": diaSemanaObj = DayOfWeek.Sunday; break;
                default: throw new ValidacionException("No puede haber campos en blanco");
            }

            // Validar deporte existente
            var deportes = await repositorioDeporte.ListarDeportes();
            var deporteObj = deportes.FirstOrDefault(d => d.id == idDeporte);
            if (deporteObj == null)
            {
                throw new ValidacionException("El deporte no existe.");
            }

            var fechasDelMes = new List<DateTime>();
            var today = DateTime.Today;
            
            for (int i = 0; i <= 30; i++)
            {
                var dt = today.AddDays(i);
                if (dt.DayOfWeek == diaSemanaObj)
                {
                    // Combinar la fecha con la hora para verificar que no sea en el pasado
                    var fechaConHora = new DateTime(dt.Year, dt.Month, dt.Day, horaInicio.Hour, horaInicio.Minute, 0);
                    if (fechaConHora >= DateTime.Now)
                    {
                        fechasDelMes.Add(dt);
                    }
                }
            }

            if (!fechasDelMes.Any())
            {
                throw new ValidacionException("No se encontraron fechas para ese día en el mes actual.");
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
