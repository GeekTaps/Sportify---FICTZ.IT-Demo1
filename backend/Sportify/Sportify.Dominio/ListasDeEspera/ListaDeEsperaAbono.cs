namespace Sportify.Dominio.ListasDeEspera;
using System;
public class ListaDeEsperaAbono{
    public Guid id { get; private set; }
    public Guid idUsuario { get; private set; }
    public Guid idDeporte { get; private set; } //esto esta al pedo, no lo borro por si las duda
    public DateTime fecha { get; private set; }
    public Guid idHorario { get; private set; }
    public bool Notificado { get; private set; } = false;
    public DateTime? FechaNotificacion { get; private set; }

    public void MarcarComoNotificado()
    {
        Notificado = true;
        FechaNotificacion = DateTime.Now;
    }

    public ListaDeEsperaAbono(Guid idUsuario, Guid idDeporte, Guid idHorario)
    {
        this.id = Guid.NewGuid();
        this.idUsuario = idUsuario;
        this.idDeporte = idDeporte;
        this.fecha = DateTime.Now;
        this.idHorario = idHorario;
    }
}
