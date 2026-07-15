namespace Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Usuario;

public class ListarUsuariosEnListaEsperaAbonoUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public ListarUsuariosEnListaEsperaAbonoUseCase(IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<List<Usuario>> Ejecutar(Guid idDeporte)
    {
        return await this.repositorioUsuarios.ListarUsuariosEnListaEsperaAbono(idDeporte);
    }
}