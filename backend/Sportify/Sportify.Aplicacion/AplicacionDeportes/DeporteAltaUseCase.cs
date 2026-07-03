using System;
using System.Threading.Tasks;

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

    public async Task Ejecutar(string nombre, string descripcion, double precio)
    {
        if (await validadorDeporte.validarNombre(nombre, repositorioDeporte)) //valida que el nombre del deporte no sea el de otro deporte antes de crearlo
        {
            await repositorioDeporte.crearDeporte(nombre, descripcion, precio);
        }
        else
        {
            throw new Exception("Ya Existe un Deporte Con El Nombre que Intenta Asignar");
        }
    }
}