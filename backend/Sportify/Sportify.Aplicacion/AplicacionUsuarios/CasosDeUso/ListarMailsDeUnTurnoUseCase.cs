namespace Sportify.Aplicacion.AplicacionUsuarios;

using Sportify.Aplicacion.AplicacionPagos;
using Sportify.Aplicacion.AplicacionTurnos;
using Sportify.Aplicacion.AplicacionReservas;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Pagos;
using Sportify.Dominio.Reservas;

public class ListarMailsDeUnTurnoUseCase
{
    private readonly IRepositorioTurno repositorioTurno;
    private readonly IRepositorioReserva repositorioReserva;
    private readonly IRepositorioPago repositorioPago;
    private readonly IRepositorioUsuarios repositorioUsuario;

    public ListarMailsDeUnTurnoUseCase(
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

    public async Task<List<String>> Ejecutar(Guid idTurno) //deveria devolver los mails de los usuarios para mandar el mail de cancelacion
    {

        var reservas = await repositorioReserva.ListarReservasPorTurno(idTurno);

        if (reservas == null)
        {
            reservas = new List<Reserva>();
        }

        //conseguir todos los mails de los usuarios que tienen reservas en el turno suspendido
        var mails = new List<string>();
        foreach (var reserva in reservas)   
        {
            var usuario = await repositorioUsuario.ObtenerPorId(reserva.idUsuario.ToString());
            if (usuario != null && !string.IsNullOrEmpty(usuario.Mail))
            {
                mails.Add(usuario.Mail);
            }
        }

        return mails; // Devolver la lista de correos electrónicos
    }


}