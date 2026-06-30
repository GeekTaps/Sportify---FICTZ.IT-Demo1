namespace Sportify.Aplicacion.AplicacionPagos;

using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Pagos;
using Sportify.Dominio.Reservas;

public class SuspenderTurnoAdminUseCase
{
    private readonly IRepositorioTurno repositorioTurno;
    private readonly IRepositorioReserva repositorioReserva;
    private readonly IRepositorioPago repositorioPago;
    private readonly IRepositorioUsuarios repositorioUsuario;

    public SuspenderTurnoAdminUseCase(
    IRepositorioTurno repositorioTurno,
    IRepositorioReserva repositorioReserva,
    IRepositorioPago repositorioPago,
    IRepositorioUsuarios repositorioUsuario)
{
    this.repositorioTurno = repositorioTurno;
    this.repositorioReserva = repositorioReserva;
    this.repositorioPago = repositorioPago;
    this.repositorioUsuario = repositorioUsuario;
}

    public async Task Ejecutar(Guid idTurno) //deveria devolver los mails de los usuarios para mandar el mail de cancelacion
    {
        var turno = await repositorioTurno.TraerTurnoPorId(idTurno);

        if (turno == null)
        { 
            throw new Exception("Turno no encontrado");
        }

        turno.mostrarEnHome = false;
        await repositorioTurno.ModificarTurno(turno, turno.Id);

        var reservas = await repositorioReserva.ListarReservasPorTurno(idTurno);

        if (reservas == null)
        {
            reservas = new List<Reserva>();
        }


        var reservasAbonadas = reservas.Where(r => r.abonado).ToList(); //esto devuelve un credito correspondiente
        reservas = reservas.Where(r => !r.abonado).ToList(); //esto devuelve un reembolso correspondiente

        foreach (var reserva in reservas) //reembolso de reservas no abonadas  
        {
            var pago = await repositorioPago.listarPagosReserva(reserva.id);
            foreach (var p in pago)
            {
                // realizar un reembolso creando un nuevo pago con monto y abonado = true
                //medio que realmente no esta haciendo un reembolso pq no me voy a meter con mercado pago, aguante harcodear cosas :D
                var reembolso = new Pago(p.idReserva, p.idUsuario, p.monto);
                await repositorioPago.registrarPago(reembolso);
            }

            await repositorioReserva.eliminarReserva(reserva.id);
        }

        foreach (var reserva in reservasAbonadas) //credito de reservas abonadas
        {
            await repositorioReserva.eliminarReserva(reserva.id);
            //aca deberia crear un credito correspondiente al deporte del tipo de la reserva cancelada.
        }
    }


}