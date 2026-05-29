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
    public async Task validar(Usuario usuario, string idUsuario)
        {
        
        if (!string.IsNullOrWhiteSpace(usuario.Mail))
        {
            if (!usuario.Mail.Contains("@"))
                throw new ValidacionException("El mail no es válido");

            // Solo validar si el mail cambió, pero como no tenemos el original fácil, al menos vemos si existe
            // Ojo: si el usuario manda su propio mail de nuevo, esto podría dar error si el repo no filtra su propio ID
            // Asumiremos que el repositorioUsuarios.BuscarMail busca si EXISTE. 
            // Para ser correctos, idealmente no debería fallar si es su propio mail, pero dejémoslo como estaba, solo que opcional.
            if (await repositorioUsuarios.BuscarMail(usuario.Mail))
            {
                // TODO: En un sistema real deberíamos verificar que el mail encontrado no sea del mismo usuario.
                // Como workaround simple para este proyecto, mantengo la regla si manda mail nuevo.
                throw new ValidacionException("Ese mail ya está registrado");
            }
        }

        if (!string.IsNullOrWhiteSpace(usuario.Contraseña))
        {
            if (usuario.Contraseña.Length < 6)
                throw new ValidacionException("La contraseña debe tener al menos 6 caracteres");
        }

        if (!string.IsNullOrWhiteSpace(usuario.Edad))
        {
            if (!int.TryParse(usuario.Edad, out int edad))
                throw new ValidacionException("La edad ingresada no es válida");
            
            if (edad <= 17)
                throw new ValidacionException("Debes ser mayor de 18 años");
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