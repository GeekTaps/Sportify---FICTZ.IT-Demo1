using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Turnos;
using Sportify.Aplicacion.AplicacionDeportes;

namespace Sportify.Aplicacion.AplicacionTurnos
{
    public class TurnoModificacionMensualUseCase
    {
        private readonly IRepositorioTurno repositorioTurno;
        private readonly IRepositorioDeporte repositorioDeporte;

        public TurnoModificacionMensualUseCase(IRepositorioTurno repositorioTurno, IRepositorioDeporte repositorioDeporte)
        {
            this.repositorioTurno = repositorioTurno;
            this.repositorioDeporte = repositorioDeporte;
        }

        public async Task Ejecutar(Guid idOriginal, Guid idDeporteNuevo, string fechaInicioStr, string horaInicioStr, int cupo, double precio, string nombreProfesor, bool listaEsperaHabilitada)
        {
            // Validar campos vacíos
            if (idOriginal == Guid.Empty ||
                idDeporteNuevo == Guid.Empty || 
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

            if (fechaInicio.Date <= DateTime.Today)
            {
                throw new ValidacionException("La fecha de inicio debe ser posterior a la fecha actual.");
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

            DayOfWeek diaSemanaObj = fechaInicio.DayOfWeek;

            // Validar deporte existente
            var deportes = await repositorioDeporte.ListarDeportes();
            var deporteObj = deportes.FirstOrDefault(d => d.id == idDeporteNuevo);
            if (deporteObj == null)
            {
                throw new ValidacionException("El deporte no existe.");
            }

            // Buscar el turno original para encontrar el patrón de la serie
            var turnosExistentes = await repositorioTurno.ListarTurnos();
            var turnoOriginal = turnosExistentes.FirstOrDefault(t => t.Id == idOriginal);
            if (turnoOriginal == null)
            {
                throw new ValidacionException("Turno original no encontrado.");
            }

            var deporteOriginalId = turnoOriginal.IdDeporte;
            var diaSemanaOriginal = turnoOriginal.Fecha.DayOfWeek;
            var horaInicioOriginal = turnoOriginal.horaInicio;
            var startDate = fechaInicio.Date;

            var fechaLimite = startDate.AddDays(30);

            // Encontrar todos los turnos de los próximos 30 días que pertenecen a esta "serie"
            var today = DateTime.Today;
            var turnosSerie = turnosExistentes.Where(t => 
                t.IdDeporte == deporteOriginalId &&
                t.Fecha.Date >= today &&
                t.Fecha.Date <= today.AddDays(30) &&
                t.Fecha.DayOfWeek == diaSemanaOriginal &&
                t.horaInicio == horaInicioOriginal).ToList();

            // Calcular fechas de los próximos 30 días que caen en el NUEVO día de la semana
            var fechasNuevasDelMes = new List<DateTime>();
            
            for (int i = 0; i <= 30; i++)
            {
                var dt = startDate.AddDays(i);
                if (dt.DayOfWeek == diaSemanaObj)
                {
                    var fechaConHora = new DateTime(dt.Year, dt.Month, dt.Day, horaInicio.Hour, horaInicio.Minute, 0);
                    if (fechaConHora >= DateTime.Now)
                    {
                        fechasNuevasDelMes.Add(dt);
                    }
                }
            }

            // Si cambiaron las propiedades clave, hay que verificar conflictos
            bool cambioClave = (deporteOriginalId != idDeporteNuevo || diaSemanaOriginal != diaSemanaObj || horaInicioOriginal != horaInicio);

            if (cambioClave)
            {
                foreach (var fecha in fechasNuevasDelMes)
                {
                    bool existe = turnosExistentes.Any(t => 
                        !turnosSerie.Contains(t) && // Ignorar los que vamos a modificar
                        t.IdDeporte == idDeporteNuevo && 
                        t.Fecha.Date == fecha.Date && 
                        t.horaInicio == horaInicio);

                    if (existe)
                    {
                        throw new EntidadRepetidaException("Ya hay un turno de ese deporte para ese horario");
                    }
                }
            }

            // Modificar los turnos de la serie (o recrearlos si las fechas no coinciden en cantidad, 
            // pero para simplificar, borramos la serie actual y creamos una nueva serie en su lugar, 
            // manteniendo el ID del primero si es posible o simplemente borrando y creando).
            // Lo más seguro es borrar la serie en el mes actual y crear la nueva.
            
            // Borrado
            foreach (var t in turnosSerie)
            {
                await repositorioTurno.BajaTurno(t.Id);
            }

            // Creación
            var turnosNuevos = new List<Turno>();
            foreach (var fecha in fechasNuevasDelMes)
            {
                var fechaConHora = new DateTime(fecha.Year, fecha.Month, fecha.Day, horaInicio.Hour, horaInicio.Minute, 0);

                var turno = new Turno
                {
                    Id = Guid.NewGuid(),
                    IdDeporte = idDeporteNuevo,
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

            foreach (var nuevo in turnosNuevos)
            {
                await repositorioTurno.AltaTurno(nuevo);
            }
        }
    }
}
