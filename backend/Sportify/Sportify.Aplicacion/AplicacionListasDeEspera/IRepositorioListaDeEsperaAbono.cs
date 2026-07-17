namespace Sportify.Aplicacion.AplicacionListasDeEspera;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Dominio.Turnos;
using Sportify.Dominio.Usuario;

public interface IRepositorioListaDeEsperaAbono{
    public Task agregarEnEspera(ListaDeEsperaAbono e);
    public Task<bool> eliminarEspera(Guid idUsuario, Guid idDeporte);
    public  Task<bool> existeEnEspera(Guid idUsuario, Guid idDeporte);
    public Task<List<ListaDeEsperaAbono>> listarEntradas(Guid idHorario);
    public Task Modificar(ListaDeEsperaAbono espera);
    public Task<List<ListaDeEsperaAbono>> listarEntradasNotificadas();
    public Task<List<Usuario>> listarUsuarios(Guid idHorario); // lista los usuarios esperando para un horario dado
    public Task<List<Horario>> listarHorarios(Guid idUsuario); // dado un usuario, lista los horarios para los que espera
}
