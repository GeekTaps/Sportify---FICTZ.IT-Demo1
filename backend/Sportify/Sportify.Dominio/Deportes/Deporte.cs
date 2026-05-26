namespace Sportify.Dominio.Deportes;
using System;
public class Deporte{
    public Guid id { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }

    public Deporte() { }

    public Deporte(string nombre, string descripcion)
    {
        this.id = Guid.NewGuid();
        this.nombre = nombre;
        this.descripcion = descripcion;
    }
}
