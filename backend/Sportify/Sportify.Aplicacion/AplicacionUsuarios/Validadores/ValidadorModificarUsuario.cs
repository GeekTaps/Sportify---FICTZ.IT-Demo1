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
    public async Task validar(Usuario usuario, Usuario usuarioactual)
        {
           
        int edad = DateTime.Today.Year - usuario.FechaNacimiento.Year;
        

        
           if (!string.IsNullOrWhiteSpace(usuario.Mail))
    {
        usuario.Mail = usuario.Mail.Trim().ToLower();
    }

    if (!string.IsNullOrWhiteSpace(usuarioactual.Mail))
    {
        usuarioactual.Mail = usuarioactual.Mail.Trim().ToLower();
    }


    if (!string.IsNullOrWhiteSpace(usuario.Mail))
{
    if (!usuario.Mail.Contains("@"))
        throw new ValidacionException("El mail no es válido");

    bool existe = await repositorioUsuarios.BuscarMail(usuario.Mail);

    // SOLO si el mail cambió
    if (existe && usuario.Mail != usuarioactual.Mail)   
    {
        throw new ValidacionException("Ese mail ya está registrado");
    }
}

        if (!string.IsNullOrWhiteSpace(usuario.PasswordNueva))
        {
            if (usuario.PasswordNueva.Length < 6)
                throw new ValidacionException("La contraseña debe tener al menos 6 caracteres");
        }

       if (usuario.FechaNacimiento.Date > DateTime.Today.AddYears(-edad))
        {
         edad--;
        }
        if (edad < 18)
    throw new ValidacionException("Debes ser mayor de 18 años");

            
            

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
            "pito",
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
            "patorusuescupeleche",
            "tutiotelechea"
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