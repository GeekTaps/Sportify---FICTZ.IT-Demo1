namespace Sportify.Aplicacion.AplicacionListasDeEspera;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Dominio.Deportes;
using Sportify.Dominio.Usuario;

public interface IRepositorioListaDeEsperaAbono{
    public Task agregarEnEspera(ListaDeEsperaAbono e);
    public Task<bool> eliminarEspera(Guid idUsuario, Guid idDeporte);
     public  Task<bool> existeEnEspera(Guid idUsuario, Guid idDeporte);
    public Task<List<Usuario>> listarUsuarios(Guid idDeporte); // lista los usuarios esperando para un deporte dado
    public Task<List<Deporte>> listarDeportes(Guid idUsuario); // dado un usuario, lista los deportes para los que espera
}
