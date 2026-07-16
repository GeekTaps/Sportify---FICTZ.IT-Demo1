using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Dominio.Reservas;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public class SuspenderCuentaUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;
    private readonly IRepositorioReserva repositorioReserva;

    public SuspenderCuentaUseCase(IRepositorioUsuarios repositorioUsuarios, IRepositorioReserva repositorioReserva)
    {
        this.repositorioUsuarios = repositorioUsuarios;
        this.repositorioReserva = repositorioReserva;
    }

    public async Task Ejecutar(string mail, bool cancelarReservas)
    {
        // Suspende permanentemente al usuario
        await this.repositorioUsuarios.SuspenderAlumnoPermanente(mail);

        if (cancelarReservas)
        {
            var usuario = await this.repositorioUsuarios.ObtenerPorMail(mail);
            if (usuario != null && Guid.TryParse(usuario.Id, out Guid idUsuario))
            {
                List<Reserva> reservas = await this.repositorioReserva.listarReservasUsuario(idUsuario);
                foreach (var reserva in reservas)
                {
                    await this.repositorioReserva.eliminarReserva(reserva.id);
                }
            }
        }
    }
}