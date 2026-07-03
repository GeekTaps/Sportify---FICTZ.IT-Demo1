namespace Sportify.Dominio.Deportes;
using System;
public class Deporte{
    public Guid id { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
    public double precio { get; set; } = 2000;

    public Deporte() { }

    public Deporte(string nombre, string descripcion, double precio)
    {
        this.id = Guid.NewGuid();
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.precio = precio;
    }
}
