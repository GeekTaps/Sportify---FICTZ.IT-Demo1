namespace Sportify.Infraestructura.Repositorios;
using System;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionUsuarios;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Deportes;
using Sportify.Infraestructura.Identity;
using Sportify.Dominio.Usuario;
using Identity;
//SEAN BIENVENIDOS AL ZEGASITORIO HECHO CON ALGUNA QUE OTRA OJEADA AL DE IANN Y MUCHO AMOR PROPIO
//LOS INVITO A DEJAR UN CORAZON (<3)
//
//
//
//
//-----------------------------------------------------------------------------------
//REWORKEAMOS EL ZEGASITORIO AHORA CON ASYNC TASKS
public class repositorioUsuarios : IRepositorioUsuarios
{

<<<<<<< HEAD
    private readonly UserManager<UsuarioIdentity> userManager;
    public repositorioUsuarios(UserManager<UsuarioIdentity> userManager)
{
    this.userManager = userManager;
}
    
public async Task<bool> BuscarId(string id)
{
    UsuarioIdentity? usuarioBuscado =
        await userManager.FindByIdAsync(id);

    return usuarioBuscado != null;
}

    public async Task<bool> BuscarMail(string mail)
{
    UsuarioIdentity? usuarioBuscado =
        await userManager.FindByEmailAsync(mail);

    return usuarioBuscado != null;
}
public async Task RegistrarUsuario(Usuario usuario)
{                                                           // hola buenas esto registra un usuario
    UsuarioIdentity usuarioAAgregar = new UsuarioIdentity
    {
        NombreCompleto = usuario.NombreCompleto,
        UserName = usuario.Mail,
        Email = usuario.Mail,
        Edad = usuario.Edad,
        Dni = usuario.Dni,
        Borrado = false
    };
                                    //gpt me dijo que tengo que hacerlo asi :D
    IdentityResult resultado =
        await userManager.CreateAsync(  //al parecer usermanager hace cosas 
            usuarioAAgregar,
            usuario.Contraseña
        );

    if (!resultado.Succeeded)
    {
        throw new Exception("No se pudo registrar el usuario");         //de momento dejo esto aca quiza no combiene que el repositorio maneje instrucciones (Segun la arquitectura cebollal)
=======
    public repositorioUsuarios(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
>>>>>>> origin/development
    }
}
public async Task ModificarUsuario(string id, Usuario usuario)
{
    UsuarioIdentity? usuarioAModificar =
        await userManager.FindByIdAsync(id);

<<<<<<< HEAD
    if (usuarioAModificar == null)
    {
        throw new Exception("Usuario no encontrado");
    }

    usuarioAModificar.NombreCompleto =
        usuario.NombreCompleto;

    usuarioAModificar.Email =
        usuario.Mail;
                                                //esto modifica los datos del usuario

    usuarioAModificar.Edad =
        usuario.Edad;

    usuarioAModificar.Dni =
        usuario.Dni;
    string token =                                                  //para cambiar la contraseña usoe ste metodo full nazi 
    await userManager.GeneratePasswordResetTokenAsync(usuarioAModificar);

    await userManager.ResetPasswordAsync(
    usuarioAModificar,
    token,
    usuario.Contraseña);
    await userManager.UpdateAsync(usuarioAModificar);
} 

    public async Task BajaLogica(string id)     
{
    UsuarioIdentity? usuarioABorrar =   //busco usuariopor id
        await userManager.FindByIdAsync(id);

    if (usuarioABorrar == null)
    {
        throw new Exception("Usuario no encontrado");
    }

    usuarioABorrar.Borrado = true;          //borrado logico anashe
=======
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
>>>>>>> origin/development

    await userManager.UpdateAsync(usuarioABorrar);
}
}
