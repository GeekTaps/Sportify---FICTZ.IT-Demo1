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
public void Ejecutar(Guid idUsuario ,Usuario usuario)       //Pasar usuario o campo por campo? 
                                                            
        {                                                        //si la modificacion tiene las mismas reglas que la alta no deberian reflejarse como reglas de negocio en el taiga? recordemos que van con la listita viendo que se cumplan
                                                         // no se me armé un quilombo mental que se yo
            if (this.validadorModificarUsuario.validar(usuario,idUsuario))
            {
                this.repositorioUsuarios.modificarUsuario(idUsuario, usuario);
            }
        }
    

}

