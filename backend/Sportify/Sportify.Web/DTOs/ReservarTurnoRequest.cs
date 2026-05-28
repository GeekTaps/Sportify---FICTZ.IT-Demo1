namespace Sportify.Web.DTOs
{
    using System;

    public class ReservarTurnoRequest
    {
        public string Email { get; set; }
        public Guid IdTurno { get; set; }
    }
}
