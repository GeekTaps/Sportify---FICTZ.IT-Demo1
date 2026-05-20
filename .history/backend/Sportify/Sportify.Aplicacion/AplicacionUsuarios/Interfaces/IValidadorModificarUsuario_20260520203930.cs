using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IValidadorModificarUsuario
{
    
public bool validar (Usuario usuario, Guid id);

}