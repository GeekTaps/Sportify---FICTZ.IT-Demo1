using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public class BajaLogicaUsuarioUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;


public BajaLogicaUsuarioUseCase(IRepositorioUsuarios repositorioUsuarios)
    {
        
        this.repositorioUsuarios=repositorioUsuarios;

    }
public void Ejecutar(Guid id)
    {
        if (this.repositorioUsuarios.BuscarId(id)) //busco existencia del usuario
        {
            this.repositorioUsuarios.bajaLogica(id);
        }
    }


    
}