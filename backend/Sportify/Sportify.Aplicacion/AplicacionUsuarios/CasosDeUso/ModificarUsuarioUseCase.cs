using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public class modificarUsuarioUseCase
{
private readonly IRepositorioUsuarios repositorioUsuarios;
private readonly IValidadorModificarUsuario validadorModificarUsuario;

public modificarUsuarioUseCase(IValidadorModificarUsuario validadorUsuario,IRepositorioUsuarios repositorioUsuarios)
    {
        this.validadorModificarUsuario=validadorUsuario;
        this.repositorioUsuarios=repositorioUsuarios;

    }
//esta parte me dejó muchas dudas
public async Task Ejecutar(string idUsuario, Usuario usuario)
    {
        if (await this.validadorModificarUsuario
            .validar(usuario, idUsuario))
        {
            await this.repositorioUsuarios
                .ModificarUsuario(idUsuario, usuario);
        }
    }
    

}

