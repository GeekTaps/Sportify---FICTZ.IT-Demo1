namespace Sportify.Aplicacion.AplicacionListasDeEspera;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Dominio.Usuario;
using Sportify.Dominio.Turnos;

public interface IRepositorioListaDeEsperaTurno{
    public Task agregarEnEspera(Guid idUsuario, Guid idTurno);
    public Task<Usuario>? siguienteEnEspera(Guid idTurno);
    public Task<bool> eliminarEspera(Guid idUsuario, Guid idTurno);
    public Task<bool> existeEnEspera(Guid idUsuario, Guid idTurno);
    public Task<List<Usuario>> listarUsuarios(Guid idTurno); // lista los usuarios esperando para un turno dado
    public Task<List<Turno>> listarTurnos(Guid idUsuario); // dado un usuario, lista los turnos para los que espera
}
