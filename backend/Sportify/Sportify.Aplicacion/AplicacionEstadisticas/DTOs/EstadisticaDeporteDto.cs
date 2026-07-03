using System;

namespace Sportify.Aplicacion.AplicacionEstadisticas.DTOs
{
    public class EstadisticaDeporteDto
    {
        public Guid IdDeporte { get; set; }
        public string NombreDeporte { get; set; }
        public int CantidadInscripciones { get; set; }
    }
}
