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
        private readonly IRepositorioHorario repositorioHorario;
        private readonly Sportify.Aplicacion.AplicacionAbonos.IRepositorioAbono repositorioAbono;
        private readonly Sportify.Aplicacion.AplicacionReservas.ReservaAltaUseCase reservaAltaUseCase;

        public TurnoAltaMensualUseCase(
            IRepositorioTurno repositorioTurno, 
            IRepositorioDeporte repositorioDeporte, 
            IRepositorioHorario repositorioHorario,
            Sportify.Aplicacion.AplicacionAbonos.IRepositorioAbono repositorioAbono,
            Sportify.Aplicacion.AplicacionReservas.ReservaAltaUseCase reservaAltaUseCase)
        {
            this.repositorioTurno = repositorioTurno;
            this.repositorioDeporte = repositorioDeporte;
            this.repositorioHorario = repositorioHorario;
            this.repositorioAbono = repositorioAbono;
            this.reservaAltaUseCase = reservaAltaUseCase;
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

            if (!Enum.TryParse<DayOfWeek>(diaSemanaStr, true, out DayOfWeek diaSemanaObj))
            {
                throw new ValidacionException("Día de la semana inválido");
            }

            // Validar cupo y precio
            if (cupo <= 0)
            {
                throw new ValidacionException("El cupo debe ser mayor a 0");
            }

            // Validar parsing de hora
            if (!TimeOnly.TryParse(horaInicioStr, out TimeOnly horaInicio))
            {
                throw new ValidacionException("Formato de hora inválido");
            }

            TimeOnly horaFin = horaInicio.AddHours(1);



            // Validar deporte existente
            var deportes = await repositorioDeporte.ListarDeportes();
            var deporteObj = deportes.FirstOrDefault(d => d.id == idDeporte);
            if (deporteObj == null)
            {
                throw new ValidacionException("El deporte no existe.");
            }

            if (deporteObj.precio < 0)
            {
                throw new ValidacionException("El precio no puede ser negativo");
            }

            var fechasDelMes = new List<DateTime>();
            
            // CREACIÓN MENSUAL RE-HABILITADA:
            var startDate = DateTime.Today;
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

            if (!fechasDelMes.Any())
            {
                throw new ValidacionException("No se encontraron fechas para el día seleccionado.");
            }

            // Generar los objetos Turno
            var horario = await repositorioHorario.CrearYGuardarHorario(idDeporte, diaSemanaStr, horaInicio);
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
                    Precio = deporteObj.precio,
                    nombreTurno = $"{deporteObj.nombre} - {fechaConHora:dd/MM/yy} - {horaInicio:HH:mm}hs",
                    nommbreProfesor = nombreProfesor,
                    ListaEsperaHabilitada = listaEsperaHabilitada,
                    IdHorario = horario.id
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

            // Obtener los abonados activos para el horario
            var abonadosActivos = await repositorioAbono.ObtenerAbonosActivosPorHorario(horario.id);

            // Guardar todos los turnos y crear reservas para abonados
            foreach (var nuevo in turnosNuevos)
            {
                await repositorioTurno.AltaTurno(nuevo);

                foreach (var abono in abonadosActivos)
                {
                    if (nuevo.cupo > 0)
                    {
                        var nuevaReserva = new Sportify.Dominio.Reservas.Reserva(
                            abono.IdUsuario,
                            nuevo.Id,
                            false, // No está paga
                            nuevo.Precio,
                            nuevo.nombreTurno
                        );
                        nuevaReserva.marcarComoAbonado();
                        
                        // Guardar la reserva
                        await reservaAltaUseCase.Ejecutar(nuevaReserva);

                        // Descontar cupo
                        nuevo.cupo--;
                        await repositorioTurno.ModificarTurno(nuevo, nuevo.Id);
                    }
                }
            }
        }
    }
}
