using System;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;
using Sportify.Aplicacion.Excepciones;
namespace Sportify.Aplicacion.AplicacionUsuarios;
//SISI 5000 VALIDACIONES LO SE, PERO BUENO SI DEJAN LAS COSAS VACIAS ESTÁ TODO MAL ASI QUE
public class ValidadorModificarUsuario : IValidadorModificarUsuario
{
private readonly IRepositorioUsuarios repositorioUsuarios;
public ValidadorModificarUsuario ( IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios=repositorioUsuarios;
    }
public async Task validar(Usuario usuario, string idUsuario)   //huele re mal este codigo lo se pero ni a palo le aplico refactorin suckeame un egg
    {
    
    if (string.IsNullOrWhiteSpace(usuario.Mail) || !usuario.Mail.Contains("@"))
        throw new ValidacionException("El mail no es válido");

    if (await repositorioUsuarios.BuscarMail(usuario.Mail))
        throw new ValidacionException("Ese mail ya está registrado");

    if (string.IsNullOrWhiteSpace(usuario.Contraseña))
        throw new ValidacionException("La contraseña no puede estar vacía");

    if (usuario.Contraseña.Length < 6)
        throw new ValidacionException("La contraseña debe tener al menos 6 caracteres");

       if (string.IsNullOrWhiteSpace(usuario.Edad))
        throw new ValidacionException("La edad ingresada no es válida");

    if (!int.TryParse(usuario.Edad, out int edad))
        throw new ValidacionException("La edad ingresada no es válida");    

    if (string.IsNullOrWhiteSpace(usuario.Dni))
        throw new ValidacionException("El DNI Ingresado no es válido");

    if ( !int.TryParse(usuario.Dni, out int dni))
        throw new ValidacionException("El DNI Ingresado no es válido");  
    if (edad <= 17)
        throw new ValidacionException("Debes ser mayor de 18 años");

    if (!validarNombre(usuario.NombreCompleto))
        throw new ValidacionException("El nombre contiene palabras prohibidas");

   
}


    public bool validarNombre(String nombre)
    {
        
{
    string[] palabrasProhibidas = //los invito a dejar un insulto nuevo cada vez que pasen por acá
    {
        "puta",
        "mierda",
        "joder",
        "idiota",
        "pelotudo",
        "maricon",
        "tarado",
        "ano",
        "pene",
        "culo",
        "teta",
        "nazi",
        "pedofilo",
        "mogolico",
        "pelotudo",
        "boludo",
        "down",
        "chota",
        "poronga",
        "verga",
        "puto",
        "carajo",
        "imbécil",
        "gilipollas",
        "polla",
        "culero",
        "pinga",
        "forro",
        "nigga",
        "violador",
        "boluda",
        "cogeburras",
        "cogeburros",
        "comegordas",
        "comegordos",
        "zoofilico",
        "zoofilica",
        "pederasta",
        "jugadordegenshinimpact",
        "jugadordelol",
        "groomer",
        "mrbeast"
    };

    nombre = nombre.ToLower();

    foreach (string palabra in palabrasProhibidas)
    {
        if (nombre.Contains(palabra))
        {
            return false;
        }
    }

    return true;
}
    }
}