using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public class ListarUsuariosSuspendidosUseCase
{ 
private readonly IRepositorioUsuarios repositorioUsuarios;


    public ListarUsuariosSuspendidosUseCase (IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios =repositorioUsuarios;
    }

    
    public async Task<List<Usuario >> Ejecutar(){
        return await this.repositorioUsuarios.ListarUsuariosSuspendidos();
    }
}