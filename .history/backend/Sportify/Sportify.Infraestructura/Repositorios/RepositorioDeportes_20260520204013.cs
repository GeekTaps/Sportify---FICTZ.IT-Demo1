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

    public bool eliminarDeporte(Guid idDeporte) //eliminar deporte por id
    {
        Deporte? deporte = archivo.Deportes.Find(idDeporte);
        if (deporte == null)
        {
            return false;
        }
        archivo.Deportes.Remove(deporte);
        archivo.SaveChanges();
        return true;
    }

    public bool existeDeporte(Guid idDeporte) //se fija si el deporte con dicho Id existe
    {
        Deporte? deporte = archivo.Deportes.Find(idDeporte);
        if (deporte != null)
        {
            return true; //si el deporte existe, devuelve true
        }

        return false;
    }

    public bool existeDeportePorNombre(string nombreDeporte) //se fija si el deporte con dicho nombre existe
    {
        Deporte? deporte = archivo.Deportes.FirstOrDefault(d => d.nombre == nombreDeporte);
        if (deporte != null)
        {
            return true; //si el deporte con ese nuevo nombre ya existe, devuelve true
        }

        return false;
    }
    
    public bool modificarDeporte(Guid idDeporte, string nuevoNombre, string nuevaDescripcion) //modificar el deporte encontrado por id, cambiando su nombre y descripcion unicamente
    {
        Deporte? deporte = archivo.Deportes.Find(idDeporte);
        if (deporte == null)
        {
            return false;
        }
        deporte.GetType().GetProperty("nombre")!.SetValue(deporte, nuevoNombre);
        deporte.GetType().GetProperty("descripcion")!.SetValue(deporte, nuevaDescripcion);
        archivo.SaveChanges();
        return true;
    }

}
