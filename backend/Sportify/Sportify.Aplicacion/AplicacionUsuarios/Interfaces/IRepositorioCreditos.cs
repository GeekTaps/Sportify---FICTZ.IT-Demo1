namespace Sportify.Aplicacion.AplicacionUsuarios;

using Sportify.Dominio.Usuario;

public interface IRepositorioCreditos
{
    Task<Credito?> ObtenerCredito(Guid usuarioId, Guid deporteId);
    Task AgregarCredito(Credito credito);
    Task ModificarCredito(Credito credito);
}