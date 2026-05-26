using System;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Dominio;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;
//SISI 5000 VALIDACIONES LO SE, PERO BUENO SI DEJAN LAS COSAS VACIAS ESTÁ TODO MAL ASI QUE
public class ValidadorRegistrarUsuario : IValidadorRegistrarUsuario
{
private readonly IRepositorioUsuarios repositorioUsuarios;
public ValidadorRegistrarUsuario ( IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios=repositorioUsuarios;
    }
public async Task<bool> validar (Usuario usuario)   //huele re mal este codigo lo se pero ni a palo le aplico refactorin suckeame un egg
    {
      if (!string.IsNullOrWhiteSpace(usuario.Mail)|| !usuario.Mail.Contains("@"))  //mail valido
      {
        if (await this.repositorioUsuarios.BuscarMail(usuario.Mail))  //mail que no exista ya 
        {
            if (!string.IsNullOrWhiteSpace(usuario.Contraseña)) //contraseña no nula
            {
                if  ( usuario.Contraseña.Length>5)
                 {
                     if (!string.IsNullOrWhiteSpace(usuario.Edad) && int.TryParse(usuario.Edad, out int EdadParseada))
                     {
                         if (EdadParseada > 17)
                         {
                           if (this.validarNombre(usuario.NombreCompleto))
                            {
                              return true;
                            }
                         else throw new Exception("El nombre de usuario no debe ser ofensivo");
                         }
                      else throw new Exception("Debes ser mayor de 18 años para registrarte");
                      }
                else throw new Exception("La edad ingresada no es valida");
                }
                else throw new Exception("La contraseña debe tener como minimo 6 caracteres");
            }
            else throw new Exception ("La contraseña no puede estar vacía");    
        }
        else throw new Exception("Ese mail ya está registrado"); //lo siento pipi la ciberseguridad te la debo
        }
    
    else throw new Exception ("El mail no es valido ");
    }


    public bool validarNombre(String nombre)
    {
        
{
    string[] palabrasProhibidas = //los invito a dejar un insulto nuevo cada vez que pasen por acá
    {
        "puta",
        "mierda",
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