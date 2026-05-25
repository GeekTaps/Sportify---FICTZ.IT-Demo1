namespace Sportify.Aplicacion.AplicacionDeportes;
using System;
using Sportify.Dominio.Deportes;

public interface IRepositorioDeporte{

    public bool crearDeporte(string nombre, string descripcion);

    public List<Deporte> ListarDeportes();
    public bool eliminarDeporte(Guid idDeporte);

    public bool existeDeporte(Guid idDeporte);

    public bool existeDeportePorNombre(string nombreDeporte);

    public bool modificarDeporte(Guid idDeporte, string nuevoNombre, string nuevaDescripcion);
}
