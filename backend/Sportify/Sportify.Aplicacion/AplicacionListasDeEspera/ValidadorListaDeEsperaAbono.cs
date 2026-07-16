using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Dominio.ListasDeEspera;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;
// using Sportify.Aplicacion.AplicacionTurnos;



public class ValidadorListaDeEsperaAbono : IValidadorListaDeEsperaAbono
{
   public async Task<bool> validarAgregarEnEspera(
    ListaDeEsperaAbono e,
    IRepositorioListaDeEsperaAbono repositorioListaDeEsperaAbono)
{
    return !await repositorioListaDeEsperaAbono.existeEnEspera(
        e.idUsuario,
        e.idDeporte
    );
}

}