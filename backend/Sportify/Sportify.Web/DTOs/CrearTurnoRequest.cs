using System;

namespace Sportify.Web.DTOs
{
    public class CrearTurnoRequest
    {
        public Guid IdDeporte { get; set; }
        public string DiaSemana { get; set; } = ""; // "Monday", "Tuesday", etc.
        public string HoraInicio { get; set; } = ""; // "18:00"
        public string HoraFin { get; set; } = ""; // "19:00" (or we can just default to 1 hr later)
        public int Cupo { get; set; }
        public double Precio { get; set; }
        public string NombreProfesor { get; set; } = "";
        public bool ListaEsperaHabilitada { get; set; }
    }
}
