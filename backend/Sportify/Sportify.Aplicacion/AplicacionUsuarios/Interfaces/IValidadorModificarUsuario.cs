using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;
namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IValidadorModificarUsuario
{
    
public  Task<bool> validar(Usuario usuario, string idUsuario);

}