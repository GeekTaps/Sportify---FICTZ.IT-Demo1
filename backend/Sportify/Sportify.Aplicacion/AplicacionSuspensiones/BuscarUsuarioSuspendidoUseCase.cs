using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;
//buen dia amigos de vuelta tenemos el problema de que lo que es identity está en el repositorio pero se soluciona con una interfaz
//miren y aprendan
public class BuscarUsuarioSuspendidoUseCase
{ 
private readonly IRepositorioUsuarios repositorioUsuarios;


    public BuscarUsuarioSuspendidoUseCase (IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios =repositorioUsuarios;
    }

    
    public async Task Ejecutar(String mail){
         await this.repositorioUsuarios.ObtenerPorMail(mail);
    }
}