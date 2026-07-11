namespace Sportify.Aplicacion.AplicacionEstadisticas;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionEstadisticas.DTOs;

public interface IRepositorioEstadisticas
{
	Task<List<EstadisticaDeporteDto>> ObtenerInscripcionesPorDeporte();
	Task<List<EstadisticaTurnoDto>> ObtenerInscripcionesPorTurno();
}