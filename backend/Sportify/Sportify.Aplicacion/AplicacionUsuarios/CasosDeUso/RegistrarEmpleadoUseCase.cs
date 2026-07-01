namespace Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio.Usuario;
public class RegistrarEmpleadoUseCase
{
 private readonly IRepositorioUsuarios repositorioUsuarios;
private readonly IValidadorRegistrarUsuario validadorRegistrarUsuarios;
    public RegistrarEmpleadoUseCase(IRepositorioUsuarios repositorioUsuarios, IValidadorRegistrarUsuario validadorRegistrarUsuarios)
    {
        this.repositorioUsuarios = repositorioUsuarios;
        this.validadorRegistrarUsuarios = validadorRegistrarUsuarios;
    }

    public async Task Ejecutar(Usuario usuario)
    {
         await this.validadorRegistrarUsuarios.validar(usuario);
        
            await this.repositorioUsuarios.RegistrarEmpleado(usuario);
    }
}





