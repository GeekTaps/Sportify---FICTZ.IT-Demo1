using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IRepositorioUsuarios
{
    
public void RegistrarUsuario(Usuario usuario);
public bool BuscarMail(string mail);
public bool BuscarId(Guid id);
 public void bajaLogica(Guid id);
public void modificarUsuario(Guid id, Usuario usuario);


}