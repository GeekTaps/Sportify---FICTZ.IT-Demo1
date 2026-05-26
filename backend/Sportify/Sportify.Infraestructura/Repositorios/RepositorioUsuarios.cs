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

    public repositorioUsuarios(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }

    public async Task<bool> BuscarId(Guid id) //verifico si existe un usuario con el id parametreado
    {
        UsuarioIdentity? usuarioBuscado = await archivo.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
        return usuarioBuscado != null;
    }

    public async Task<bool> BuscarMail(string mail) //verifico si existe un usuario con el mail parametreado
    {
        UsuarioIdentity? usuarioBuscado = await archivo.Users.FirstOrDefaultAsync(u => u.Email == mail);
        return usuarioBuscado != null;
    }

    public async Task RegistrarUsuario(Usuario usuario)    //explicacion desatallada en Sportify.Dominio.Usuario
                                                //conversion de usuario comun a usuarioIdentity, lo agrego a la db    
    {
        UsuarioIdentity usuarioAAgregar = new UsuarioIdentity(usuario.NombreCompleto, usuario.Contraseña, usuario.Mail, usuario.Edad, usuario.Dni);
        await archivo.AddAsync(usuarioAAgregar);
        await archivo.SaveChangesAsync();
    }

    public async Task modificarUsuario(Guid id, Usuario usuario) //busco usuario con el id parametreado y cambio todos sus campos
    {
        
        UsuarioIdentity usuarioAModificar = await archivo.Users.FindAsync(id);
        usuarioAModificar.GetType().GetProperty("NombreCompleto")!.SetValue(usuarioAModificar, usuario.NombreCompleto);
        usuarioAModificar.GetType().GetProperty("Contraseña")!.SetValue(usuarioAModificar, usuario.Contraseña);
        usuarioAModificar.GetType().GetProperty("Email")!.SetValue(usuarioAModificar, usuario.Mail);
        usuarioAModificar.GetType().GetProperty("Edad")!.SetValue(usuarioAModificar, usuario.Edad);
        usuarioAModificar.GetType().GetProperty("Dni")!.SetValue(usuarioAModificar, usuario.Dni);
        await archivo.SaveChangesAsync();
    }  

    public async Task bajaLogica(Guid id) //busco usuario con el id parametreado y lo marco como borrado
    {
        UsuarioIdentity usuarioABorrar = await archivo.Users.FindAsync(id);
        usuarioABorrar.GetType().GetProperty("Borrado")!.SetValue(usuarioABorrar, true);
        await archivo.SaveChangesAsync();

    }  
}
