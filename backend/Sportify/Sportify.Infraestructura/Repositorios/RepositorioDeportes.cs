namespace Sportify.Infraestructura.Repositorios;
using System;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionDeportes;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Deportes;

public class RepositorioDeportes : IRepositorioDeporte
{
    private readonly ApplicationDbContext archivo;

    public RepositorioDeportes(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }

    public async Task<bool> eliminarDeporte(Guid idDeporte) //eliminar deporte por id
    {
        Deporte? deporte = await archivo.Deportes.FindAsync(idDeporte);
        if (deporte == null)
        {
            return false;
        }
        archivo.Deportes.Remove(deporte);
        await archivo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> existeDeporte(Guid idDeporte) //se fija si el deporte con dicho Id existe
    {
        Deporte? deporte = await archivo.Deportes.FindAsync(idDeporte);
        return deporte != null;
    }

    public async Task<bool> existeDeportePorNombre(string nombreDeporte) //se fija si el deporte con dicho nombre existe
    {
        Deporte? deporte = await archivo.Deportes.FirstOrDefaultAsync(d => d.nombre == nombreDeporte);
        return deporte != null;
    }

    public async Task<Deporte?> ObtenerPorNombre(string nombreDeporte)
    {
        return await archivo.Deportes.FirstOrDefaultAsync(d => d.nombre == nombreDeporte);
    }

    public async Task<Deporte?> obtenerDeportePorId(Guid idDeporte)
    {
        return await archivo.Deportes.FindAsync(idDeporte);
    }
    
    public async Task<bool> modificarDeporte(Guid idDeporte, string nuevaDescripcion, double nuevoPrecio) //modificar la descripción y el precio del deporte encontrado por id
    {
        Deporte? deporte = await archivo.Deportes.FindAsync(idDeporte);
        if (deporte == null)
        {
            return false;
        }
        deporte.descripcion = nuevaDescripcion;
        deporte.precio = nuevoPrecio;
        await archivo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> crearDeporte(string nombre, string descripcion, double precio) 
    {
        Deporte deporte = new Deporte(nombre, descripcion, precio);
        await archivo.Deportes.AddAsync(deporte);
        await archivo.SaveChangesAsync();
        return true;
    }

    public async Task<List<Deporte>> ListarDeportes()
    {
        return await archivo.Deportes.ToListAsync();
    }

}
