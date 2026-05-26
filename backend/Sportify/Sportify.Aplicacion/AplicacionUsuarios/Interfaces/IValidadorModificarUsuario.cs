using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IValidadorModificarUsuario
{
    
<<<<<<< HEAD
public  Task<bool> validar(Usuario usuario, string idUsuario);
=======
	Task<bool> validar (Usuario usuario, Guid id);
>>>>>>> origin/development

}