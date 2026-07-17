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
using Sportify.Dominio.Pagos;
using Sportify.Aplicacion.AplicacionPagos;

namespace Sportify.Aplicacion.AplicacionAbonos
{
    public class AbonarUseCase
    {
        private readonly IRepositorioAbono _repositorioAbono;
        private readonly IRepositorioUsuarios _repositorioUsuarios;
        private readonly IRepositorioTurno _repositorioTurno;
        private readonly ReservaAltaUseCase _reservaAltaUseCase;
        private readonly IRepositorioHorario _repositorioHorario;
        private readonly IRepositorioCreditos _repositorioCreditos;
        private readonly IRepositorioPago _repositorioPago;

        public AbonarUseCase(
            IRepositorioAbono repositorioAbono,
            IRepositorioUsuarios repositorioUsuarios,
            IRepositorioTurno repositorioTurno,
            ReservaAltaUseCase reservaAltaUseCase,
            IRepositorioHorario repositorioHorario,
            IRepositorioCreditos repositorioCreditos,
            IRepositorioPago repositorioPago)
        {
            _repositorioAbono = repositorioAbono;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioTurno = repositorioTurno;
            _reservaAltaUseCase = reservaAltaUseCase;
            _repositorioHorario = repositorioHorario;
            _repositorioCreditos = repositorioCreditos;
            _repositorioPago = repositorioPago;
        }

        public async Task Ejecutar(string email, Guid idTurnoBase)
        {
            // HARDCODEO DE PAGOS: este overload permite registrar el pago con el monto mostrado sin usar Mercado Pago.
            await Ejecutar(email, idTurnoBase, null);
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

            bool estaAbonado = await _repositorioAbono.ExisteAbonoActivo(Guid.Parse(usuario.Id), idHorario);
            if (estaAbonado) throw new ValidacionException("Ya estás abonado a este horario.");



            // Buscar clases que corresponden a este mes y hasta el día 10 del siguiente mes inclusive
            var hoy = DateTime.Now.Date;
            var startOfNextMonth = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(1);
            var maxFecha = startOfNextMonth.AddDays(9); // Hasta el 10 del mes siguiente inclusive

            var todosLosTurnos = await _repositorioTurno.ListarTurnos();
            var turnosDelAbono = todosLosTurnos
                .Where(t => t.IdHorario == idHorario && t.Fecha.Date >= hoy && t.Fecha.Date <= maxFecha)
                .OrderBy(t => t.Fecha)
                .ToList();

            // Calcular precio y descontar creditos
            double precioPorClase = turnoBase.Precio;
            double precioTotalOriginal = turnosDelAbono.Count * precioPorClase;
            double montoAPagar = precioTotalOriginal;
            
            // Consumir créditos del usuario si tiene para el deporte
            var creditoEntity = await _repositorioCreditos.ObtenerCredito(Guid.Parse(usuario.Id), turnoBase.IdDeporte);
            int creditosADescontar = 0;
            if (creditoEntity != null && creditoEntity.Cantidad > 0)
            {
                // El máximo de créditos a descontar es la cantidad de clases a las que se está abonando
                creditosADescontar = Math.Min(creditoEntity.Cantidad, turnosDelAbono.Count);
                
                // Actualizar creditoEntity usando UsarCredito() varias veces
                for (int i = 0; i < creditosADescontar; i++)
                {
                    creditoEntity.UsarCredito();
                }
                await _repositorioCreditos.ModificarCredito(creditoEntity);
                montoAPagar = Math.Max(0, precioTotalOriginal - creditosADescontar * precioPorClase);
            }

            // Crear y guardar el abono
            var abono = new Abono(Guid.Parse(usuario.Id), idHorario);
            await _repositorioAbono.CrearAbono(abono);

            Guid? idPrimeraReserva = null;

            // Generar las reservas para todas las clases y descontar cupos
            foreach (var t in turnosDelAbono)
            {
                if (t.cupo > 0)
                {
                    var nuevaReserva = new Reserva(Guid.Parse(usuario.Id), t.Id, true, t.Precio, t.nombreTurno);
                    nuevaReserva.marcarComoAbonado();
                    nuevaReserva.marcarComoPagada();
                    await _reservaAltaUseCase.Ejecutar(nuevaReserva);

                    if (idPrimeraReserva == null)
                    {
                        idPrimeraReserva = nuevaReserva.id;
                    }

                    t.cupo--;
                    await _repositorioTurno.ModificarTurno(t, t.Id);
                }
            }

            decimal montoFinalPago = montoPago ?? (decimal)montoAPagar;
            if (montoFinalPago > 0 && idPrimeraReserva.HasValue)
            {
                var pago = new Pago(idPrimeraReserva.Value, Guid.Parse(usuario.Id), montoFinalPago);
                await _repositorioPago.registrarPago(pago);
            }
        }
    }
}
