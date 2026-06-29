namespace Sportify.Dominio.Reservas;
using System;
public class Reserva{
    public Guid id { get; private set; }
    public Guid idUsuario { get; private set; }
    public Guid idTurno { get; private set; }
    public DateTime fecha { get; private set; }
    public Boolean paga { get; private set; }
    public Double monto { get; private set; }
    public string titulo { get; private set; }
    public bool eliminada { get; private set; } = false;
    public bool abonado { get; private set; } = false;



    public Reserva(Guid idUsuario, Guid idTurno, Boolean paga, Double monto, string titulo)
    {
        this.id = Guid.NewGuid();
        this.idUsuario = idUsuario;
        this.idTurno = idTurno;
        this.fecha = DateTime.Now;
        this.paga = paga;
        this.monto = monto;
        this.titulo = titulo;
    }

    public void marcarComoPagada()
    {
        this.paga = true;
    }

    public void eliminarLogicamente()
    {
        this.eliminada = true;
    }
}