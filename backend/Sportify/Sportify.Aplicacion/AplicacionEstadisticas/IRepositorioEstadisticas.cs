namespace Sportify.Aplicacion.AplicacionEstadisticas;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionEstadisticas.DTOs;

public interface IRepositorioEstadisticas
{
	Task<List<EstadisticaDeporteDto>> ObtenerInscripcionesPorDeporte();
	Task<EstadisticaReservaDto> ObtenerEstadisticasReservas();
	Task<List<EstadisticaTurnoDto>> ObtenerInscripcionesPorTurno();
	Task<List<EstadisticaAsistenciaDto>> ObtenerAsistenciasPorDeporte();
	Task<List<EstadisticaPagoDto>> ObtenerEstadisticasDePagos();
}