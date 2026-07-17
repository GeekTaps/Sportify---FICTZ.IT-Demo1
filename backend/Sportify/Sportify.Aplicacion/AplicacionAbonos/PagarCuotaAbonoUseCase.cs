using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Reservas;
using Sportify.Aplicacion.Excepciones;
using Sportify.Dominio.Pagos;
using Sportify.Aplicacion.AplicacionPagos;
using Sportify.Aplicacion.AplicacionAbonos;

namespace Sportify.Aplicacion.AplicacionAbonos
{
    public class PagarCuotaAbonoUseCase
    {
        private readonly IRepositorioUsuarios _repositorioUsuarios;
        private readonly IRepositorioTurno _repositorioTurno;
        private readonly IRepositorioHorario _repositorioHorario;
        private readonly IRepositorioCreditos _repositorioCreditos;
        private readonly IRepositorioPago _repositorioPago;
        private readonly IRepositorioReserva _repositorioReserva;

        public PagarCuotaAbonoUseCase(
            IRepositorioUsuarios repositorioUsuarios,
            IRepositorioTurno repositorioTurno,
            IRepositorioHorario repositorioHorario,
            IRepositorioCreditos repositorioCreditos,
            IRepositorioPago repositorioPago,
            IRepositorioReserva repositorioReserva)
        {
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioTurno = repositorioTurno;
            _repositorioHorario = repositorioHorario;
            _repositorioCreditos = repositorioCreditos;
            _repositorioPago = repositorioPago;
            _repositorioReserva = repositorioReserva;
        }

        public async Task Ejecutar(string email, Guid idTurnoBase, decimal? montoPago)
        {
            var usuario = await _repositorioUsuarios.ObtenerPorMail(email);
            if (usuario == null) throw new ValidacionException("Usuario no encontrado.");

            var turnoBase = await _repositorioTurno.ObtenerTurnoPorId(idTurnoBase);
            if (turnoBase == null || turnoBase.IdHorario == null)
                throw new ValidacionException("Turno base inválido o no es recurrente.");

            Guid idHorario = turnoBase.IdHorario;
            var horario = await _repositorioHorario.ObtenerHorarioPorId(idHorario);
            if (horario == null) throw new ValidacionException("Horario no encontrado.");

            // Buscar clases que corresponden a este mes y hasta el día 10 del siguiente mes inclusive
            var hoy = DateTime.Now.Date;
            var startOfNextMonth = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(1);
            var maxFecha = startOfNextMonth.AddDays(9); // Hasta el 10 del mes siguiente inclusive

            var todosLosTurnos = await _repositorioTurno.ListarTurnos();
            var turnosDelAbono = todosLosTurnos
                .Where(t => t.IdHorario == idHorario && t.Fecha.Date >= hoy && t.Fecha.Date <= maxFecha)
                .OrderBy(t => t.Fecha)
                .ToList();

            var reservasUsuario = await _repositorioReserva.listarReservasUsuario(Guid.Parse(usuario.Id));

            // Calcular precio y descontar creditos
            double precioPorClase = turnoBase.Precio;
            double precioBase = 0;

            foreach (var t in turnosDelAbono)
            {
                var reservaExistente = reservasUsuario.FirstOrDefault(r => r.idTurno == t.Id && !r.eliminada);
                if (reservaExistente != null)
                {
                    if (!reservaExistente.paga && !reservaExistente.pagoSeña)
                        precioBase += precioPorClase;
                    else if (reservaExistente.pagoSeña && !reservaExistente.paga)
                        precioBase += (precioPorClase * 0.5);
                }
                else
                {
                    precioBase += precioPorClase;
                }
            }

            double montoAPagar = precioBase;
            
            // Consumir créditos del usuario si tiene para el deporte
            var creditoEntity = await _repositorioCreditos.ObtenerCredito(Guid.Parse(usuario.Id), turnoBase.IdDeporte);
            int creditosADescontar = 0;
            if (creditoEntity != null && creditoEntity.Cantidad > 0)
            {
                // El máximo de créditos a descontar es la cantidad de clases pendientes
                creditosADescontar = Math.Min(creditoEntity.Cantidad, turnosDelAbono.Count);
                
                for (int i = 0; i < creditosADescontar; i++)
                {
                    creditoEntity.UsarCredito();
                }
                await _repositorioCreditos.ModificarCredito(creditoEntity);
                montoAPagar = Math.Max(0, precioBase - creditosADescontar * precioPorClase);
            }

            // Aplicamos descuento si correspondía al total
            montoAPagar = montoAPagar * 0.80;

            Guid? idPrimeraReserva = null;

            // Marcar las reservas pendientes del abono como pagadas
            foreach (var t in turnosDelAbono)
            {
                var reservaExistente = reservasUsuario.FirstOrDefault(r => r.idTurno == t.Id && !r.eliminada);

                if (reservaExistente != null)
                {
                    reservaExistente.marcarComoAbonado();
                    reservaExistente.marcarComoPagada();
                    
                    if (idPrimeraReserva == null)
                    {
                        idPrimeraReserva = reservaExistente.id;
                    }
                }
            }

            decimal montoFinalPago = montoPago ?? (decimal)montoAPagar;
            if (idPrimeraReserva.HasValue)
            {
                var pago = new Pago(idPrimeraReserva.Value, Guid.Parse(usuario.Id), montoFinalPago);
                await _repositorioPago.registrarPago(pago);
            }
        }
    }
}
