using System;
using System.Diagnostics;

namespace Sportify.Aplicacion.AplicacionReservas;

public interface IValidadorReserva
{
    public bool validarId(Guid idReserva, IRepositorioReserva repositorioReserva); // = chequear si existe

    public bool validarAntelación(Guid idReserva, IRepositorioReserva repositorioReserva, DateTime fecha); // = chequear si faltan 48hs o más

}