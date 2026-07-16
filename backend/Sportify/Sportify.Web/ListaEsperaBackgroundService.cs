using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sportify.Infraestructura.Repositorios;
using Sportify.Aplicacion.AplicacionListasDeEspera;
using Sportify.Web.Controllers;

namespace Sportify.Web;

public class ListaEsperaBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ListaEsperaBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var repoTurno = scope.ServiceProvider
                .GetRequiredService<IRepositorioListaDeEsperaTurno>();

            var repoAbono = scope.ServiceProvider
                .GetRequiredService<IRepositorioListaDeEsperaAbono>();

            var controllerNotificador = scope.ServiceProvider
                .GetRequiredService<ReservasController>();


            await RevisarListaTurnos(
                repoTurno,
                controllerNotificador
            );


            await RevisarListaAbonos(
                repoAbono,
                controllerNotificador
            );


            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken
            );
        }
    }

    private async Task RevisarListaTurnos(
        IRepositorioListaDeEsperaTurno repo,
        ReservasController notificador)
    {
        var entradas = await repo.listarEntradasNotificadas();

        foreach (var entrada in entradas)
        {
            if (!entrada.Notificado)
                continue;

            if (entrada.FechaNotificacion.Value.AddHours(2) > DateTime.Now)
                continue;


            await repo.eliminarEspera(
                entrada.idUsuario,
                entrada.idTurno
            );


            var siguientes = await repo.listarUsuarios(entrada.idTurno);

            if (siguientes.Any())
            {
                await notificador.NotificarSiguienteEnEspera(
                    entrada.idTurno,
                    "Turno liberado"
                );
            }
        }
    }

    private async Task RevisarListaAbonos(
        IRepositorioListaDeEsperaAbono repo,
        ReservasController notificador)
    {
        var entradas = await repo.listarEntradasNotificadas();

        foreach (var entrada in entradas)
        {
            if (!entrada.Notificado)
                continue;

            if (entrada.FechaNotificacion.Value.AddHours(2) > DateTime.Now)
                continue;


            await repo.eliminarEspera(
                entrada.idUsuario,
                entrada.idDeporte
            );


            var siguientes = await repo.listarUsuarios(
                entrada.idHorario
            );


            if (siguientes.Any())
            {
                await notificador.NotificarSiguienteEnEsperaAbono(
                    entrada.idHorario,
                    "Horario disponible"
                );
            }
        }
    }
}