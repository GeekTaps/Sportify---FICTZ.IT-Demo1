using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;
 //FALTA IMPLEMENTAR QUE AVISE SI EL USUARIO ESTÁ ANOTADO A CLASES TODAVIA 
public class BajaLogicaUsuarioUseCase
{
    private readonly IRepositorioUsuarios repositorioUsuarios;


public BajaLogicaUsuarioUseCase(IRepositorioUsuarios repositorioUsuarios)
    {
        
        this.repositorioUsuarios=repositorioUsuarios;

    }
public async Task Ejecutar(string id)
{
    if (await this.repositorioUsuarios.BuscarId(id))
    {
        await this.repositorioUsuarios.BajaLogica(id);
    }
}


    
}