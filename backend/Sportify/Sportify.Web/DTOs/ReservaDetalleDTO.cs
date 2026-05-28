namespace Sportify.Web.DTOs
{
    using System;

    public class ReservaDetalleDTO
    {
        public Guid IdReserva { get; set; }
        public string Actividad { get; set; }
        public string Fecha { get; set; }
        public string Horario { get; set; }
        public string Profesor { get; set; }
        public double HorasAnticipacion { get; set; }
        public int CancelacionesMes { get; set; }
        public bool Suspendido { get; set; }
    }
}
