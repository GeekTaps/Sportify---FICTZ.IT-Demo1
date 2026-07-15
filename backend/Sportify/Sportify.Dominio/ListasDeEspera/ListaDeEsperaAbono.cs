namespace Sportify.Dominio.ListasDeEspera;
using System;
public class ListaDeEsperaAbono{
    public Guid id { get; private set; }
    public Guid idUsuario { get; private set; }
    public Guid idHorario { get; private set; }
    public DateTime fecha { get; private set; }

    public ListaDeEsperaAbono(Guid idUsuario, Guid idHorario)
    {
        this.id = Guid.NewGuid();
        this.idUsuario = idUsuario;
        this.idHorario = idHorario;
        this.fecha = DateTime.Now;
    }
}