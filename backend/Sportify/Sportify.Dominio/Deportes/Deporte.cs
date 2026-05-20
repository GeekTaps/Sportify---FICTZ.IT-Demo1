namespace Sportify.Dominio.Deportes;
using System;
public class Deporte{
    public Guid id { get; private set; }
    public string nombre { get; private set; }
    public string descripcion { get; private set; }

    public Deporte(string nombre, string descripcion)
    {
        this.id = Guid.NewGuid();
        this.nombre = nombre;
        this.descripcion = descripcion;
    }
}
