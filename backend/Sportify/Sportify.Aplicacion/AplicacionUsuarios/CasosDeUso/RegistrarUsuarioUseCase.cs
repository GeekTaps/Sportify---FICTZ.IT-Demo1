using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionUsuarios;

public class RegistrarUsuarioUseCase
{
private readonly IRepositorioUsuarios repositorioUsuarios;
private readonly IValidadorRegistrarUsuario validadoroRegistrarUsuarios;
public RegistrarUsuarioUseCase(IRepositorioUsuarios repositorioUsuarios, IValidadorRegistrarUsuario validadorUsuario)
    {
        this.repositorioUsuarios=repositorioUsuarios;
        this.validadoroRegistrarUsuarios=validadorUsuario;
    }


public async Task Ejecutar(Usuario usuario)//(voz robotica) valido usuario, agrego usuario, bip, bup, bup, bip
    {
        if (await this.validadoroRegistrarUsuarios.validar(usuario))
        {
            this.repositorioUsuarios.RegistrarUsuario(usuario);
        }

    }

}