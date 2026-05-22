using System;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;
//buen dia amigos de vuelta tenemos el problema de que lo que es identity está en el repositorio pero se soluciona con una interfaz
//miren y aprendan
public class CerrarSesionUsuarioUseCase
{ 
private readonly IServicioAutenticacion auth;


    public CerrarSesionUsuarioUseCase(IServicioAutenticacion auth)
    {
        this.auth = auth;
    }

    public async Task Ejecutar()
    {
        await auth.Logout();
    }
}