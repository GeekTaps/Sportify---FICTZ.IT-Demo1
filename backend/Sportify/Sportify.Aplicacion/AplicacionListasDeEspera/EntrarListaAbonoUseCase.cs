using System;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionAbonos;
using Sportify.Aplicacion.AplicacionTurnos;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;

public class EntrarListaAbonoUseCase
{
    private readonly IRepositorioListaDeEsperaAbono _repositorioLista;
    private readonly IValidadorListaDeEsperaAbono _validadorLista;
    private readonly IRepositorioTurno _repositorioTurno;
    private readonly ObtenerInfoAbonoUseCase _obtenerInfoAbonoUseCase;

    public EntrarListaAbonoUseCase(
        IRepositorioListaDeEsperaAbono repositorioLista,
        IValidadorListaDeEsperaAbono validadorLista,
        IRepositorioTurno repositorioTurno,
        ObtenerInfoAbonoUseCase obtenerInfoAbonoUseCase)
    {
        _repositorioLista = repositorioLista;
        _validadorLista = validadorLista;
        _repositorioTurno = repositorioTurno;
        _obtenerInfoAbonoUseCase = obtenerInfoAbonoUseCase;
    }

    public async Task Ejecutar(Guid idUsuario, Guid idTurno, string emailUsuario)
    {
        var turno = await _repositorioTurno.ObtenerTurnoPorId(idTurno);
        if (turno == null) throw new EntidadNotFoundException("Turno no encontrado");

        // Usamos la misma logica de abonos para chequear conflictos (si ya está abonado, si hay conflicto de horario, etc)
        var info = await _obtenerInfoAbonoUseCase.Ejecutar(idTurno, emailUsuario);
        
        if (info.IsAlreadySubscribed)
            throw new EntidadNotFoundException("Ya estás abonado a este turno");
            
        if (info.HasConflict)
            throw new EntidadNotFoundException("Ya tenés una reserva en ese horario");

        var entrada = new ListaDeEsperaAbono(idUsuario, turno.IdDeporte);

        if (await _validadorLista.validarAgregarEnEspera(entrada, _repositorioLista))
        {
            await _repositorioLista.agregarEnEspera(entrada);
        }
        else
        {
            throw new EntidadNotFoundException("Ya estás en la lista de espera");
        }
    }
}
