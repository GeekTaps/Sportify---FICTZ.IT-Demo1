using System;
using System.Threading.Tasks;
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
<<<<<<< HEAD
public async Task Ejecutar(string id)
{
    if (await this.repositorioUsuarios.BuscarId(id))
    {
        await this.repositorioUsuarios.BajaLogica(id);
=======
public async Task Ejecutar(Guid id)
    {
        if (await this.repositorioUsuarios.BuscarId(id)) //busco existencia del usuario
        {
            await this.repositorioUsuarios.bajaLogica(id);
        }
>>>>>>> origin/development
    }
}


    
}