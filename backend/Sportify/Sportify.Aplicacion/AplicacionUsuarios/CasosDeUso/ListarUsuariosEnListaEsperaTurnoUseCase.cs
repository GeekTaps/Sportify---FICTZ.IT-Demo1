// ListarUsuariosEnListaEsperaTurnoUseCase.cs
namespace Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Usuario;

public class ListarUsuariosEnListaEsperaTurnoUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public ListarUsuariosEnListaEsperaTurnoUseCase(IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<List<Usuario>> Ejecutar(Guid idTurno)
    {
        return await this.repositorioUsuarios.ListarUsuariosEnListaEsperaTurno(idTurno);
    }
}