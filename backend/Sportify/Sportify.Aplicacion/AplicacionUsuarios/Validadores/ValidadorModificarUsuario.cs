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
    if (!string.IsNullOrWhiteSpace(usuario.Mail))
    {
        if (!usuario.Mail.Contains("@"))
            throw new ValidacionException("El mail no es válido");

        if (await repositorioUsuarios.BuscarMail(usuario.Mail))
            throw new ValidacionException("Ese mail ya está registrado");
    }

    if (!string.IsNullOrWhiteSpace(usuario.Contraseña))
    {
        if (usuario.Contraseña.Length < 6)
            throw new ValidacionException("La contraseña debe tener al menos 6 caracteres");
    }

    if (!string.IsNullOrWhiteSpace(usuario.Edad))
    {
        if (!int.TryParse(usuario.Edad, out int edad) || edad < 18)
            throw new ValidacionException("La edad ingresada no es válida o debes ser mayor de 18 años");    
    }

    if (!string.IsNullOrWhiteSpace(usuario.Dni))
    {
        if (!int.TryParse(usuario.Dni, out int dni))
            throw new ValidacionException("El DNI Ingresado no es válido");  
    }

    if (!string.IsNullOrWhiteSpace(usuario.NombreCompleto))
    {
        if (!validarNombre(usuario.NombreCompleto))
            throw new ValidacionException("El nombre contiene palabras prohibidas");
    }
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
        "mrbeast",
        "patorusuescupeleche"
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