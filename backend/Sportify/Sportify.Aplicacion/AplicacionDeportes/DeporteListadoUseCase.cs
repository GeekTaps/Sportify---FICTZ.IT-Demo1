using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Deportes;

namespace Sportify.Aplicacion.AplicacionDeportes;

public class DeporteListadoUseCase
{
    IRepositorioDeporte repositorioDeporte;
    public DeporteListadoUseCase(IRepositorioDeporte repositorioDeporte)
    {
        this.repositorioDeporte = repositorioDeporte;
    }

    public async Task<List<Deporte>> Ejecutar()
    {
        return await repositorioDeporte.ListarDeportes();
    }
}