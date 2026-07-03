using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Reservas;
using Sportify.Dominio.Abonos;
using Sportify.Aplicacion.Excepciones;

namespace Sportify.Aplicacion.AplicacionAbonos
{
    public class AbonarUseCase
    {
        private readonly IRepositorioAbono _repositorioAbono;
        private readonly IRepositorioUsuarios _repositorioUsuarios;
        private readonly IRepositorioTurno _repositorioTurno;
        private readonly ReservaAltaUseCase _reservaAltaUseCase;
        private readonly IRepositorioHorario _repositorioHorario;

        public AbonarUseCase(
            IRepositorioAbono repositorioAbono,
            IRepositorioUsuarios repositorioUsuarios,
            IRepositorioTurno repositorioTurno,
            ReservaAltaUseCase reservaAltaUseCase,
            IRepositorioHorario repositorioHorario)
        {
            _repositorioAbono = repositorioAbono;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioTurno = repositorioTurno;
            _reservaAltaUseCase = reservaAltaUseCase;
            _repositorioHorario = repositorioHorario;
        }

        public async Task Ejecutar(string email, Guid idTurnoBase)
        {
            var usuario = await _repositorioUsuarios.ObtenerPorMail(email);
            if (usuario == null) throw new ValidacionException("Usuario no encontrado.");

            var turnoBase = await _repositorioTurno.ObtenerTurnoPorId(idTurnoBase);
            if (turnoBase == null || turnoBase.IdHorario == null)
                throw new ValidacionException("Turno base inválido o no es recurrente.");

            Guid idHorario = turnoBase.IdHorario.Value;
            var horario = await _repositorioHorario.ObtenerHorarioPorId(idHorario);
            if (horario == null) throw new ValidacionException("Horario no encontrado.");

            bool estaAbonado = await _repositorioAbono.ExisteAbonoActivo(Guid.Parse(usuario.Id), idHorario);
            if (estaAbonado) throw new ValidacionException("Ya estás abonado a este horario.");

            // Buscar clases que corresponden a este mes (y primeros 10 días del siguiente si estamos después del 20)
            var hoy = DateTime.Now.Date;
            var maxFecha = new DateTime(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month));
            if (hoy.Day >= 20)
            {
                maxFecha = maxFecha.AddDays(10);
            }

            var todosLosTurnos = await _repositorioTurno.ListarTurnos();
            var turnosDelAbono = todosLosTurnos
                .Where(t => t.IdHorario == idHorario && t.Fecha.Date >= hoy && t.Fecha.Date <= maxFecha)
                .OrderBy(t => t.Fecha)
                .ToList();

            // Calcular precio y descontar creditos
            double precioPorClase = turnoBase.Precio;
            double precioTotalOriginal = turnosDelAbono.Count * precioPorClase;
            
            // Consumir créditos del usuario si tiene
            int creditosADescontar = 0;
            if (usuario.Creditos > 0)
            {
                if (usuario.Creditos >= precioTotalOriginal)
                {
                    creditosADescontar = (int)precioTotalOriginal;
                }
                else
                {
                    creditosADescontar = usuario.Creditos;
                }
                await _repositorioUsuarios.DescontarCreditos(usuario.Id, creditosADescontar);
            }

            // Crear y guardar el abono
            var abono = new Abono(Guid.Parse(usuario.Id), idHorario);
            await _repositorioAbono.CrearAbono(abono);

            // Generar las reservas para todas las clases y descontar cupos
            foreach (var t in turnosDelAbono)
            {
                if (t.cupo > 0)
                {
                    var nuevaReserva = new Reserva(Guid.Parse(usuario.Id), t.Id, false, t.Precio, t.nombreTurno);
                    await _reservaAltaUseCase.Ejecutar(nuevaReserva);

                    t.cupo--;
                    await _repositorioTurno.ModificarTurno(t, t.Id);
                }
            }
        }
    }
}
