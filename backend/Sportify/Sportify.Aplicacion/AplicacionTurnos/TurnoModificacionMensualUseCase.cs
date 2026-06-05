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

            // Validar deporte existente
            var deportes = await repositorioDeporte.ListarDeportes();
            var deporteObj = deportes.FirstOrDefault(d => d.id == idDeporteNuevo);
            if (deporteObj == null)
            {
                throw new ValidacionException("El deporte no existe.");
            }

            // Buscar el turno original
            var turnosExistentes = await repositorioTurno.ListarTurnos();
            var turnoOriginal = turnosExistentes.FirstOrDefault(t => t.Id == idOriginal);
            if (turnoOriginal == null)
            {
                throw new ValidacionException("Turno original no encontrado.");
            }

            // Chequear conflicto
            bool existe = turnosExistentes.Any(t => 
                t.Id != idOriginal && 
                t.IdDeporte == idDeporteNuevo && 
                t.Fecha.Date == fechaConHoraExacta.Date && 
                t.horaInicio == horaInicio);

            if (existe)
            {
                throw new EntidadRepetidaException("Ya hay un turno de esa actividad en ese horario");
            }

            /*
            LOGICA MENSUAL COMENTADA:
            var turnosSerie = ...
            */

            int inscriptos = turnoOriginal.cupoMaximo - turnoOriginal.cupo;
            
            if (cupo < inscriptos)
            {
                throw new ValidacionException($"El cupo máximo no puede ser menor a la cantidad de inscriptos ({inscriptos})");
            }

            turnoOriginal.IdDeporte = idDeporteNuevo;
            turnoOriginal.Fecha = fechaConHoraExacta;
            turnoOriginal.horaInicio = horaInicio;
            turnoOriginal.horaFin = horaFin;
            turnoOriginal.cupoMaximo = cupo;
            turnoOriginal.cupo = cupo - inscriptos;
            turnoOriginal.Precio = precio;
            turnoOriginal.nombreTurno = $"{deporteObj.nombre} - {fechaConHoraExacta:dd/MM/yy} - {horaInicio:HH:mm}hs";
            turnoOriginal.nommbreProfesor = nombreProfesor;
            turnoOriginal.ListaEsperaHabilitada = listaEsperaHabilitada;

            await repositorioTurno.ModificarTurno(turnoOriginal, idOriginal);
        }
    }
}
