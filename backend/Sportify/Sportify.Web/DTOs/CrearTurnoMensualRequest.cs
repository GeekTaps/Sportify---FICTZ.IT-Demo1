using System;

namespace Sportify.Web.DTOs
{
    public class CrearTurnoMensualRequest
    {
        public Guid IdDeporte { get; set; }
        public string DiaSemana { get; set; } = ""; // "Lunes", "Martes", etc.
        public string HoraInicio { get; set; } = ""; // "18:00"
        public int Cupo { get; set; }
        public double Precio { get; set; }
        public string NombreProfesor { get; set; } = "";
        public bool ListaEsperaHabilitada { get; set; }
    }
}
