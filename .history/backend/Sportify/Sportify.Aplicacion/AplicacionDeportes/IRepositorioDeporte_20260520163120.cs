namespace Sportify.Aplicacion.AplicacionDeportes;
using System;
public interface IRepositorioDeporte{
    public bool eliminarDeporte(Guid idDeporte);

    public bool existeDeporte(Guid idDeporte);

    public bool existeDeportePorNombre(string nombreDeporte);

    public bool modificarDeporte(Guid idDeporte, string nuevoNombre, string nuevaDescripcion);
}
