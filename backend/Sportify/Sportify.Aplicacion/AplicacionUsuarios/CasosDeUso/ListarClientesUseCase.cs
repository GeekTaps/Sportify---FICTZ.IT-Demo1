namespace Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Usuario;
public class ListarClientesUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public ListarClientesUseCase(IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<List<Usuario>> Ejecutar()
    {
        return await repositorioUsuarios.ListarClientes();
    }
}