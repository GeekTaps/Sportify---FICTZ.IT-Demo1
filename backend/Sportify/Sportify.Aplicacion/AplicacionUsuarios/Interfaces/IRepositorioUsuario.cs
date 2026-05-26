using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IRepositorioUsuarios
{
    
public Task RegistrarUsuario(Usuario usuario);
public  Task<bool> BuscarMail(string mail);
public  Task<bool> BuscarId(string id);
 public  Task BajaLogica(string id);
public  Task ModificarUsuario(string id, Usuario usuario);


}