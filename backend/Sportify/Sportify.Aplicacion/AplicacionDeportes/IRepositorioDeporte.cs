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

    // Comprueba si existe un deporte con ese nombre. Si se proporciona excludeId,
    // ignora el deporte con ese Id (útil al modificar para permitir mantener el mismo nombre).
    // Devuelve el deporte con el nombre dado o null si no existe.
    public Task<Deporte?> ObtenerPorNombre(string nombreDeporte);

    public Task<bool> modificarDeporte(Guid idDeporte, string nuevoNombre, string nuevaDescripcion);
}
