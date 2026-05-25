namespace Sportify.Dominio.Deportes;
using System;
public class Reserva{
    public Guid id { get; private set; }
    public Guid idUsuario { get; private set; }
    public Guid idTurno { get; private set; }
    public DateTime fecha { get; private set; }
    public Boolean pagada { get; private set; }
    public Double monto { get; private set; }



    public Reserva(Guid idUsuario, Guid idTurno, Boolean pagada, Double monto)
    {
        this.id = Guid.NewGuid();
        this.idUsuario = idUsuario;
        this.idTurno = idTurno;
        this.fecha = DateTime.Now;
        this.pagada = pagada;
        this.monto = monto;
    }
}