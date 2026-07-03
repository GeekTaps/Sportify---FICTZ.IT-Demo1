namespace Sportify.Aplicacion.AplicacionUsuarios;

using Sportify.Aplicacion.AplicacionReservas;

public class ListarMailsDeUsuariosConPagosPendientesUseCase
{
    private readonly IRepositorioReserva _repositorioReserva;
    private readonly IRepositorioUsuarios _repositorioUsuarios;

    public ListarMailsDeUsuariosConPagosPendientesUseCase(IRepositorioReserva repositorioReserva, IRepositorioUsuarios repositorioUsuarios)
    {
        _repositorioReserva = repositorioReserva;
        _repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<List<string>> Ejecutar()
    {
        var idsUsuarios = await _repositorioReserva.BuscarUsuariosConPagosPendientes();
        var mails = new List<string>();
        foreach (var id in idsUsuarios)
        {
            var Usuario = await _repositorioUsuarios.ObtenerPorId(id.ToString());
            if (Usuario != null)
            {
                if (!mails.Contains(Usuario.Mail)) // Evita mails duplicados, el recordatorio se enviara una sola vez por usuario
                {
                    mails.Add(Usuario.Mail);
                }
            }
        }
        return mails;
    }
}