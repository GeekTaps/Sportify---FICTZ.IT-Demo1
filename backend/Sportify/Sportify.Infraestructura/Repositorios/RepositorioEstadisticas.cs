namespace Sportify.Infraestructura.Repositorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sportify.Infraestructura.Data;
using Sportify.Aplicacion.AplicacionEstadisticas;
using Sportify.Aplicacion.AplicacionEstadisticas.DTOs;

public class RepositorioEstadisticas: IRepositorioEstadisticas
{
	private readonly ApplicationDbContext archivo;

	public RepositorioEstadisticas(ApplicationDbContext archivo)
	{
		this.archivo = archivo;
	}

	public async Task<List<EstadisticaDeporteDto>> ObtenerInscripcionesPorDeporte()
	{
		var stats = await (from r in archivo.Reservas
						   where !r.eliminada
						   join t in archivo.Turnos on r.idTurno equals t.Id
						   join d in archivo.Deportes on t.IdDeporte equals d.id
						   group r by new { d.id, d.nombre } into g
						   select new EstadisticaDeporteDto
						   {
							   IdDeporte = g.Key.id,
							   NombreDeporte = g.Key.nombre,
							   CantidadInscripciones = g.Count()
						   }).ToListAsync();

		return stats;
	}
	public async Task<List<EstadisticaTurnoDto>> ObtenerInscripcionesPorTurno()
	{
		var stats = await (from r in archivo.Reservas
					   where !r.eliminada
					   join t in archivo.Turnos on r.idTurno equals t.Id
					   join d in archivo.Deportes on t.IdDeporte equals d.id
					   group r by new { t.Id, t.nombreTurno, d.nombre } into g
					   select new EstadisticaTurnoDto
					   {
						IdTurno = g.Key.Id,
						NombreTurno = string.IsNullOrEmpty(g.Key.nombreTurno) ? g.Key.nombre : g.Key.nombreTurno,
						NombreDeporte = g.Key.nombre,
						CantidadInscripciones = g.Count()
					   }).ToListAsync();

		return stats;
	}

	public async Task<List<EstadisticaAsistenciaDto>> ObtenerAsistenciasPorDeporte()
	{
		var stats = await (from a in archivo.Asistencias
					   where a.Presente
					   join t in archivo.Turnos on a.IdTurno equals t.Id
					   join d in archivo.Deportes on t.IdDeporte equals d.id
					   group a by new { d.id, d.nombre } into g
					   select new EstadisticaAsistenciaDto
					   {
						IdDeporte = g.Key.id,
						NombreDeporte = g.Key.nombre,
						CantidadAsistencias = g.Count()
					   }).ToListAsync();

		return stats;
	}

	public async Task<List<EstadisticaPagoDto>> ObtenerEstadisticasDePagos()
	{
		// Para cada reserva (no eliminada) sumar los pagos registrados y comparar con el monto de la reserva.
		var pagosPorReserva = await (from r in archivo.Reservas
					 where !r.eliminada
					 join p in archivo.Pagos on r.id equals p.idReserva into pagosGroup
					 select new
					 {
						Reserva = r,
						TotalPagos = pagosGroup.Sum(pg => (decimal?)pg.monto) ?? 0m
					 }).ToListAsync();

		int pagadoCompleto = pagosPorReserva.Count(x => x.TotalPagos >= (decimal)x.Reserva.monto);
		int seniaPendiente = pagosPorReserva.Count(x => x.TotalPagos > 0 && x.TotalPagos < (decimal)x.Reserva.monto);

		var result = new List<EstadisticaPagoDto>();
		result.Add(new EstadisticaPagoDto { Categoria = "Pagado completo", Cantidad = pagadoCompleto });
		result.Add(new EstadisticaPagoDto { Categoria = "Seña pendiente", Cantidad = seniaPendiente });

		return result;
	}
}