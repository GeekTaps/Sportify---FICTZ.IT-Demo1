using System;
using Sportify.Dominio.Deportes;

namespace Sportify.Aplicacion.AplicacionDeportes;

public class DeporteListadoUseCase
{
    IRepositorioDeporte repositorioDeporte;
    public DeporteListadoUseCase(IRepositorioDeporte repositorioDeporte)
    {
        this.repositorioDeporte = repositorioDeporte;
    }

    public List<Deporte> Ejecutar()
    {
        return repositorioDeporte.ListarDeportes();
    }
}