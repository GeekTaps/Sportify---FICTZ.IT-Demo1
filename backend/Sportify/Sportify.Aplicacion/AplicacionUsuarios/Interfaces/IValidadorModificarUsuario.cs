using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IValidadorModificarUsuario
{
    
	Task<bool> validar (Usuario usuario, Guid id);

}