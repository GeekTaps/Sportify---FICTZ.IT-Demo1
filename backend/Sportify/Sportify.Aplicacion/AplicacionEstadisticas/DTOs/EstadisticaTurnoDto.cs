using System;

namespace Sportify.Aplicacion.AplicacionEstadisticas.DTOs
{
    public class EstadisticaTurnoDto
    {
        public Guid IdTurno { get; set; }
        public string NombreTurno { get; set; }
        public string NombreDeporte { get; set; }
        public int CantidadInscripciones { get; set; }
    }
}
