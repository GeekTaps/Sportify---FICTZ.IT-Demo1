namespace Sportify.Dominio.ListasDeEspera;
using System;
public class ListaDeEsperaTurno{
    public Guid id { get; private set; }
    public Guid idUsuario { get; private set; }
    public Guid idTurno { get; private set; }
    public DateTime fecha { get; private set; }
    
    public ListaDeEsperaTurno(Guid idUsuario, Guid idTurno)
    {
        this.id = Guid.NewGuid();
        this.idUsuario = idUsuario;
        this.idTurno = idTurno;
        this.fecha = DateTime.Now;
        
    }
}