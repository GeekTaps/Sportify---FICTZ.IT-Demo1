using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;
using Sportify.Dominio.ListasDeEspera;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;
// using Sportify.Aplicacion.AplicacionTurnos;



public class ValidadorListaDeEsperaAbono : IValidadorListaDeEsperaAbono
{
    public async Task<bool> validarAgregarEnEspera(ListaDeEsperaAbono e, IRepositorioListaDeEsperaAbono repositorioListaDeEsperaAbono) // = chequear si existe
    {
        return !await repositorioListaDeEsperaAbono.existeEnEspera(e);
    }

}