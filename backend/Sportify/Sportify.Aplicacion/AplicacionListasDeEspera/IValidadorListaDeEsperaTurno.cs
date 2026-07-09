using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;

public interface IValidadorListaDeEsperaTurno
{
    public Task<bool> validarAgregarEnEspera(Guid idUsuario, Guid idTurno, IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno); // = chequear que no exista
    public Task<bool> validarSiguienteEnEspera(Guid idTurno, IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno); // = chequear si hay alguien esperando
    // public Task<bool> validarEliminarEspera(ListaDeEsperaTurno listaDeEsperaTurno, IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno);

}