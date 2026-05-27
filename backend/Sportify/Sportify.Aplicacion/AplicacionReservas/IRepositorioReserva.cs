namespace Sportify.Aplicacion.AplicacionReservas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Dominio.Reservas;

public interface IRepositorioReserva{
    public Task agregarReserva(Reserva r);
    public Task<bool> eliminarReserva(Guid idReserva);
    public Task<bool> existeReserva(Guid idReserva);
    public Task<List<Reserva>> listarReservasUsuario(Guid idUsuario);
    public Task<Reserva> buscarReserva(Guid idReserva);
}
