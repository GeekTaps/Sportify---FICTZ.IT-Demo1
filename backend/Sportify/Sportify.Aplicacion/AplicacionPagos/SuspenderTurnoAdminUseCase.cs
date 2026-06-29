namespace Sportify.Aplicacion.AplicacionPagos;

using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Pagos;

public class SuspenderTurnoAdminUseCase
{
    private readonly IRepositorioTurno repositorioTurno;
    private readonly IRepositorioReserva repositorioReserva;
    private readonly IRepositorioPago repositorioPago;
    private readonly IRepositorioUsuarios repositorioUsuario;

    public async Task<IEnumerable<string>> Ejecutar(Guid idTurno) //deveria devolver los mails de los usuarios para mandar el mail de cancelacion
    { //no chequeo si hay reservas en el turno, porque si no hay reservas no hay mails que devolver y listo
        var turno = await repositorioTurno.TraerTurnoPorId(idTurno);
        turno.mostrarEnHome = false;
        await repositorioTurno.ModificarTurno(turno, turno.Id);
        var reservas = await repositorioReserva.ListarReservasPorTurno(idTurno);

        var mails = await Task.WhenAll(reservas.Select(async r => (await repositorioUsuario.ObtenerPorId(r.idUsuario.ToString())).Mail)); //conseguir todos los mails de los usuarios que tienen reservas en el turno suspendido

        var reservasAbonadas = reservas.Where(r => r.abonado).ToList(); //esto devuelve un credito correspondiente
        reservas = reservas.Where(r => !r.abonado).ToList(); //esto devuelve un reembolso correspondiente

        foreach (var reserva in reservas) //reembolso de reservas no abonadas
        {
            await repositorioReserva.eliminarReserva(reserva.id);
            var pago = await repositorioPago.listarPagosReserva(reserva.id);
            foreach (var p in pago)
            {
                // realizar un reembolso creando un nuevo pago con monto y abonado = true
                //medio que realmente no esta haciendo un reembolso pq no me voy a meter con mercado pago, aguante harcodear cosas :D
                var reembolso = new Pago
                (
                    p.idUsuario,
                    p.idReserva,
                    p.monto
                );
                await repositorioPago.registrarPago(reembolso);
            }
        }

        foreach (var reserva in reservasAbonadas) //credito de reservas abonadas
        {
            await repositorioReserva.eliminarReserva(reserva.id);
            //aca deberia crear un credito correspondiente al deporte del tipo de la reserva cancelada.
        }

        return mails; // Devolver la lista de correos electrónicos
    }


}