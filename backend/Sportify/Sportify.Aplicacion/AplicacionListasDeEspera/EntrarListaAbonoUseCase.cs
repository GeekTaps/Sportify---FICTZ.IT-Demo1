using System;
using System.Threading.Tasks;
using Sportify.Dominio.ListasDeEspera;
using Sportify.Aplicacion;
using Sportify.Aplicacion.AplicacionAbonos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.Mails;

namespace Sportify.Aplicacion.AplicacionListasDeEspera;

public class EntrarListaAbonoUseCase
{
    private readonly IRepositorioListaDeEsperaAbono _repositorioLista;
    private readonly IValidadorListaDeEsperaAbono _validadorLista;
    private readonly IRepositorioTurno _repositorioTurno;
    private readonly ObtenerInfoAbonoUseCase _obtenerInfoAbonoUseCase;
    private readonly IServicioEmail _servicioEmail;

    public EntrarListaAbonoUseCase(
        IRepositorioListaDeEsperaAbono repositorioLista,
        IValidadorListaDeEsperaAbono validadorLista,
        IRepositorioTurno repositorioTurno,
        ObtenerInfoAbonoUseCase obtenerInfoAbonoUseCase,
        IServicioEmail servicioEmail)
    {
        _repositorioLista = repositorioLista;
        _validadorLista = validadorLista;
        _repositorioTurno = repositorioTurno;
        _obtenerInfoAbonoUseCase = obtenerInfoAbonoUseCase;
        _servicioEmail = servicioEmail;
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

        var entrada = new ListaDeEsperaAbono(idUsuario, turno.IdDeporte,turno.IdHorario);

        if (await _validadorLista.validarAgregarEnEspera(entrada, _repositorioLista))
        {
            await _repositorioLista.agregarEnEspera(entrada);
            
                var entradas = await _repositorioLista.listarEntradas(turno.IdHorario);
                Console.WriteLine($"[DEBUG] Entradas en lista de espera de abono para horario {turno.IdHorario}: {entradas.Count}");
                
                if (entradas.Count == 10)
                {
                    string subject = "¡Alta demanda en Lista de Espera de Abonos!";
                    string body = $"El horario '{turno.nombreTurno}' ha alcanzado 10 usuarios en su lista de espera para abonados.";
                    
                    Console.WriteLine($"[DEBUG] Enviando mail a adminsportify@gmail.com por alcanzar 10 usuarios");
                    await _servicioEmail.MandarMail("adminsportify@gmail.com", subject, body);
                }
        }
        else
        {
            throw new EntidadNotFoundException("Ya estás en la lista de espera");
        }
    }
}
