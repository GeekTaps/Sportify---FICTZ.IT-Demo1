using System;

namespace  Sportify.Dominio.Pagos;
public class Pago
{
    public Guid id { get; set; }
    public Guid idReserva { get; set; }
     
    public Guid idUsuario { get; set; }
    public decimal monto { get; set; }
    public DateTime fecha { get; set; }

    public Pago(Guid idReserva, Guid idUsuario, decimal monto)
    {
        this.id = Guid.NewGuid();
        this.idReserva = idReserva;
        this.idUsuario = idUsuario;
        this.monto = monto;
        this.fecha = DateTime.Now;
    }
}