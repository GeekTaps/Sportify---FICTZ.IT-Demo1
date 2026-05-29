using System;

namespace Sportify.Web.DTOs
{
    public class CrearTurnoMensualRequest
    {
        public Guid IdDeporte { get; set; }
        public string FechaInicio { get; set; } = ""; // "2026-05-30"
        public string DiaSemana { get; set; } = ""; // compatible con versiones anteriores
        public string HoraInicio { get; set; } = ""; // "18:00"
        public int Cupo { get; set; }
        public double Precio { get; set; }
        public string NombreProfesor { get; set; } = "";
        public bool ListaEsperaHabilitada { get; set; }
    }
}
