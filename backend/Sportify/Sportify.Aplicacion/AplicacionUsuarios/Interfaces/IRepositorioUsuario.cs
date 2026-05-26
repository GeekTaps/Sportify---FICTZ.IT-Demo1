using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IRepositorioUsuarios
{
    Task RegistrarUsuario(Usuario usuario);
    Task<bool> BuscarMail(string mail);
    Task<bool> BuscarId(Guid id);
    Task bajaLogica(Guid id);
    Task modificarUsuario(Guid id, Usuario usuario);
}