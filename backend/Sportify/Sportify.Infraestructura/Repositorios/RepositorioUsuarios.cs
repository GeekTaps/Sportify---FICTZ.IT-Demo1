namespace Sportify.Infraestructura.Repositorios;
using System;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionUsuarios;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Deportes;
using Sportify.Infraestructura.Identity;
using Sportify.Dominio.Usuario;
//SEAN BIENVENIDOS AL ZEGASITORIO HECHO CON ALGUNA QUE OTRA OJEADA AL DE IANN Y MUCHO AMOR PROPIO
//LOS INVITO A DEJAR UN CORAZON (<3)
//
// cata: <3
//
//
public class repositorioUsuarios : IRepositorioUsuarios
{
    private readonly ApplicationDbContext archivo;


public bool BuscarId(Guid id) //verifico si existe un usuario con el id parametreado
    {
        UsuarioIdentity? usuarioBuscado= archivo.Usuarios.FirstOrDefault(u => u.ID == id);
        if (usuarioBuscado != null)
        {
            return true;
        }
        else return false;
    }

    public bool BuscarMail(string mail) //verifico si existe un usuario con el mail parametreado
    {
        UsuarioIdentity? usuarioBuscado= archivo.Usuarios.FirstOrDefault(u => u.Mail == mail);
        if (usuarioBuscado != null)
        {
            return true;
        }
        else return false;
    }
public void RegistrarUsuario(Usuario usuario)    //explicacion desatallada en Sportify.Dominio.Usuario
                                                //conversion de usuario comun a usuarioIdentity, lo agrego a la db    
    {
        UsuarioIdentity usuarioAAgregar = new UsuarioIdentity (usuario.NombreCompleto, usuario.Contraseña, usuario.Mail,usuario.Edad,usuario.Dni);
        archivo.Add (usuarioAAgregar);
        archivo.SaveChanges();
    }
public void modificarUsuario(Guid id, Usuario usuario) //busco usuario con el id parametreado y cambio todos sus campos
    {
        
        UsuarioIdentity usuarioAModificar = archivo.Usuarios.Find(id);
        usuarioAModificar.GetType().GetProperty("NombreCompleto")!.SetValue(usuarioAModificar, usuario.NombreCompleto);
        usuarioAModificar.GetType().GetProperty("Contraseña")!.SetValue(usuarioAModificar, usuario.Contraseña);
        usuarioAModificar.GetType().GetProperty("Mail")!.SetValue(usuarioAModificar, usuario.Mail);
        usuarioAModificar.GetType().GetProperty("Edad")!.SetValue(usuarioAModificar, usuario.Edad);
        usuarioAModificar.GetType().GetProperty("Dni")!.SetValue(usuarioAModificar, usuario.Dni);
        archivo.SaveChanges();
    }  

    public void bajaLogica(Guid id) //busco usuario con el id parametreado y lo marco como borrado
    {
        UsuarioIdentity usuarioABorrar = archivo.Usuarios.Find(id);
       usuarioABorrar.GetType().GetProperty("Borrado")!.SetValue(usuarioABorrar, true);
        archivo.SaveChanges();

    }  
}
