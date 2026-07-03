using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;
// using Sportify.Aplicacion.AplicacionTurnos;



public class ValidadorListaDeEsperaTurno : IValidadorListaDeEsperaTurno
{
    public async Task<bool> validarAgregarEnEspera(Guid idUsuario, Guid idTurno, IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno) 
    {
        //  chequea que no exista la entrada
        return !await repositorioListaDeEsperaTurno.existeEnEspera(idUsuario, idTurno);
    }

    public async Task<bool> validarSiguienteEnEspera(Guid idTurno, IRepositorioListaDeEsperaTurno repositorioListaDeEsperaTurno) 
    {
        //  chequea si hay alguien esperando ese turno
        return await repositorioListaDeEsperaTurno.siguienteEnEspera(idTurno) != null;
    }

}