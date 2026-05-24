using System;
using System.Diagnostics;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IValidadorRegistrarUsuario
{
    
public bool validar(Usuario usuario);

}