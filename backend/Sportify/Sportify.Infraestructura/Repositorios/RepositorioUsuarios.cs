namespace Sportify.Infraestructura.Repositorios;
using System;
using Microsoft.AspNetCore.Identity;
using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionUsuarios;
using Sportify.Aplicacion.Excepciones;
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
//-----------------------------------------------------------------------------------
//REWORKEAMOS EL ZEGASITORIO AHORA CON ASYNC TASKS
public class RepositorioUsuarios : IRepositorioUsuarios
{

    private readonly UserManager<UsuarioIdentity> userManager;
    public RepositorioUsuarios(UserManager<UsuarioIdentity> userManager)
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
    FechaNacimiento = usuario.FechaNacimiento,
    Dni = usuario.Dni,
    Borrado = false
};
                                    //gpt me dijo que tengo que hacerlo asi :D
    IdentityResult resultado =
        await userManager.CreateAsync(  //al parecer usermanager hace cosas 
            usuarioAAgregar,
            usuario.PasswordNueva
        );

    if (!resultado.Succeeded)
    {
        var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
        throw new ValidacionException($"No se pudo registrar el usuario: {errores}");         //de momento dejo esto aca quiza no combiene que el repositorio maneje instrucciones (Segun la arquitectura cebollal)
    }
}
public async Task RegistrarEmpleado(Usuario usuario)
    {
        UsuarioIdentity usuarioAAgregar = new UsuarioIdentity
{
    NombreCompleto = usuario.NombreCompleto,
    UserName = usuario.Mail,
    Email = usuario.Mail,
    FechaNacimiento = usuario.FechaNacimiento,
    Dni = usuario.Dni,
    Borrado = false,
    EsEmpleado = true
};
                                    //gpt me dijo que tengo que hacerlo asi :D
    IdentityResult resultado =
        await userManager.CreateAsync(  //al parecer usermanager hace cosas 
            usuarioAAgregar,
            usuario.PasswordNueva
        );

    if (!resultado.Succeeded)
    {
        var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
        throw new ValidacionException($"No se pudo registrar el usuario: {errores}");         //de momento dejo esto aca quiza no combiene que el repositorio maneje instrucciones (Segun la arquitectura cebollal)
    }
    }
public async Task ModificarUsuario(string id, Usuario dto)
{
    var usuario = await userManager.FindByIdAsync(id);
    if (usuario == null)
        throw new Exception("Usuario no encontrado");

    


    if (!string.IsNullOrWhiteSpace(dto.NombreCompleto))
        usuario.NombreCompleto = dto.NombreCompleto;

    if (dto.FechaNacimiento != DateTime.MinValue)
{
    usuario.FechaNacimiento = dto.FechaNacimiento;
}

    if (!string.IsNullOrWhiteSpace(dto.Dni))
        usuario.Dni = dto.Dni;

    if (!string.IsNullOrWhiteSpace(dto.Mail))
        usuario.Email = dto.Mail;

    if (!string.IsNullOrWhiteSpace(dto.Mail))
        usuario.UserName = dto.Mail;

    if (!string.IsNullOrWhiteSpace(dto.PasswordNueva))
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(usuario);
        await userManager.ResetPasswordAsync(usuario, token, dto.PasswordNueva);
    }

    await userManager.UpdateAsync(usuario);
}

    public async Task<bool> VerificarPasswordActual(string id, string passwordActual)
    {
        UsuarioIdentity? usuario = await userManager.FindByIdAsync(id);
        if (usuario == null)
        {
            return false;
        }

        return await userManager.CheckPasswordAsync(usuario, passwordActual);
    }

    public async Task BajaLogica(string id)     
{
    UsuarioIdentity? usuarioABorrar = await userManager.FindByIdAsync(id);

    if (usuarioABorrar == null)
    {
        throw new ValidacionException("Usuario no encontrado");
    }

    usuarioABorrar.Borrado = true;          //borrado logico anashe

    await userManager.UpdateAsync(usuarioABorrar);
}
public async Task<Usuario> ObtenerPorId(string id)
{
    UsuarioIdentity usuarioIdentity = await userManager.FindByIdAsync(id);

    if (usuarioIdentity == null)
    {
        throw new ValidacionException("Usuario no encontrado");
    }

   return new Usuario(
    usuarioIdentity.NombreCompleto,
    usuarioIdentity.Email,
    usuarioIdentity.Dni,
    "",
    "",
    usuarioIdentity.FechaNacimiento
    );
}
public async Task<List<Usuario>> ListarUsuarios()
{
    List<UsuarioIdentity> usuariosIdentity =
        await userManager.Users.ToListAsync();

    List<Usuario> usuarios = usuariosIdentity
        .Select(u => new Usuario(
    u.NombreCompleto,
    u.Email,
    u.Dni,
    "",
    "",
    u.FechaNacimiento
))
        .ToList();

    return usuarios;
}
public async Task<Usuario> ObtenerPorMail(string mail)
    {
        UsuarioIdentity? usuarioIdentity =
        await userManager.FindByEmailAsync(mail);

    if (usuarioIdentity == null)
    {
        throw new ValidacionException("Usuario no encontrado");
    }

    return new Usuario(
        usuarioIdentity.NombreCompleto,
        usuarioIdentity.Email,
        usuarioIdentity.Dni,
        "",
        "",
        usuarioIdentity.FechaNacimiento
    );    



    }
    public async Task<bool> ExisteMail(string mail)
{
    return await userManager.FindByEmailAsync(mail) != null;
}
public  async Task ReactivarAlumno(string mail)
{
    UsuarioIdentity? usuarioBuscado =
        await userManager.FindByEmailAsync(mail);
    usuarioBuscado.Suspendido = false;              //:D esto parece muy sencillo

    
}
}
