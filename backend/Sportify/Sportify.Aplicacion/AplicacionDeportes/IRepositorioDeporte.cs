namespace Sportify.Aplicacion.AplicacionDeportes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Deportes;

public interface IRepositorioDeporte{

    public Task<bool> crearDeporte(string nombre, string descripcion);

    public Task<List<Deporte>> ListarDeportes();
    public Task<bool> eliminarDeporte(Guid idDeporte);

    public Task<bool> existeDeporte(Guid idDeporte);

    public Task<bool> existeDeportePorNombre(string nombreDeporte);

    public Task<bool> modificarDeporte(Guid idDeporte, string nuevoNombre, string nuevaDescripcion);
}
