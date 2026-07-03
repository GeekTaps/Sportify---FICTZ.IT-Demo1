using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;
//buen dia amigos de vuelta tenemos el problema de que lo que es identity está en el repositorio pero se soluciona con una interfaz
//miren y aprendan
public class RecuperarCuentaUseCase
{ 
private readonly IRepositorioUsuarios repositorioUsuarios;


    public RecuperarCuentaUseCase (IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios =repositorioUsuarios;
    }

    
    public async Task Ejecutar(string mail){
           Usuario usuarioABuscar= await this.repositorioUsuarios.ObtenerPorMail(mail);
           if (usuarioABuscar == null)
           {
               throw new Exception("No se encontró el usuario con el mail proporcionado");
           }
           else
        {
            //Mandar mail 
            //mail con un link a pagina de cambio de contraseña
            //link con token de seguridad para cambiar contraseña
            //magia hecha
        }
    }
}