using System;

namespace Sportify.Aplicacion.AplicacionDeportes;
public class DeporteAltaUseCase
{
    IRepositorioDeporte repositorioDeporte;
    IValidadorDeporte validadorDeporte;
    public DeporteAltaUseCase(IRepositorioDeporte repositorioDeporte, IValidadorDeporte validadorDeporte)
    {
        this.repositorioDeporte = repositorioDeporte;
        this.validadorDeporte = validadorDeporte;
    }

    public void Ejecutar(string nombre, string descripcion)
    {
        
        if (!validadorDeporte.validarNombre(nombre, repositorioDeporte)) //valida que el nombre del deporte no sea el de otro deporte antes de crearlo
        {
            repositorioDeporte.crearDeporte(nombre, descripcion);
        }
        else
        {
            throw new Exception("Ya Existe Un Deporte Con El Nombre Que Intenta Asignar");
        }
    }
}