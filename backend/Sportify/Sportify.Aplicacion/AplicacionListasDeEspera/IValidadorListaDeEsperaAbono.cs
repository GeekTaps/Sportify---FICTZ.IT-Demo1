using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;

public interface IValidadorListaDeEsperaAbono
{
    public Task<bool> validarAgregarEnEspera(ListaDeEsperaAbono listaDeEsperaAbono, IRepositorioListaDeEsperaAbono repositorioListaDeEsperaAbono); // = chequear si existe
    // public Task<bool> validarEliminarEspera(ListaDeEsperaAbono listaDeEsperaAbono, IRepositorioListaDeEsperaAbono repositorioListaDeEsperaAbono);

}