using System;

namespace  Sportify.Dominio.Pagos;
public class Pago
{
    public Guid id { get; set; }
    public Guid idTurno { get; set; }
     
    public Guid idUsuario { get; set; }
    public decimal monto { get; set; }
    public DateTime fecha { get; set; }

    public Pago(Guid idTurno, Guid idUsuario, decimal monto)
    {
        this.id = Guid.NewGuid();
        this.idTurno = idTurno;
        this.idUsuario = idUsuario;
        this.monto = monto;
        this.fecha = DateTime.Now;
    }
}