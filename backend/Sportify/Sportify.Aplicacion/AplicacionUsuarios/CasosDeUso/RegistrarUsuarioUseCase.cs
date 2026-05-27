using System;
using System.Threading.Tasks;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionUsuarios;

public class RegistrarUsuarioUseCase
{
private readonly IRepositorioUsuarios repositorioUsuarios;
private readonly IValidadorRegistrarUsuario validadorRegistrarUsuarios;
public RegistrarUsuarioUseCase(IRepositorioUsuarios repositorioUsuarios, IValidadorRegistrarUsuario validadorUsuario)
    {
        this.repositorioUsuarios=repositorioUsuarios;
        this.validadorRegistrarUsuarios=validadorUsuario;
    }


public async Task Ejecutar(Usuario usuario)//(voz robotica) valido usuario, agrego usuario, bip, bup, bup, bip
    {
         await this.validadorRegistrarUsuarios.validar(usuario);
        
            await this.repositorioUsuarios.RegistrarUsuario(usuario);
        

    }

}