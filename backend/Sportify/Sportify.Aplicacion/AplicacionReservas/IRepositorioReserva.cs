namespace Sportify.Aplicacion.AplicacionReservas;
using System;
public interface IRepositorioReserva{
    public void agregarReserva(Reserva r);
    public bool eliminarReserva(Guid idReserva);
    public bool existeReserva(Guid idReserva);
    public List<Reserva>? listarReservasUsuario(Guid idUsuario);
    public Reserva buscarReserva(Guid idReserva);
}
