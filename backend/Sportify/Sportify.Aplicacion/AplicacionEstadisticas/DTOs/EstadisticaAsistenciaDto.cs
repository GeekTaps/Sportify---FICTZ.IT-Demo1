using System;

namespace Sportify.Aplicacion.AplicacionEstadisticas.DTOs
{
    public class EstadisticaAsistenciaDto
    {
        public Guid IdDeporte { get; set; }
        public string NombreDeporte { get; set; }
        public int CantidadAsistencias { get; set; }
    }
}
