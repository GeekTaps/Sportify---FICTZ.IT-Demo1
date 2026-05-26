using System;
using System.Threading.Tasks;
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
public async Task Ejecutar(Guid id)
    {
        if (await this.repositorioUsuarios.BuscarId(id)) //busco existencia del usuario
        {
            await this.repositorioUsuarios.bajaLogica(id);
        }
    }


    
}