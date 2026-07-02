using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Sportify.Dominio.Usuario;

namespace Sportify.Aplicacion.AplicacionUsuarios;

public interface IRepositorioUsuarios
{
    
public Task RegistrarUsuario(Usuario usuario);
public Task RegistrarEmpleado(Usuario usuario);
public  Task<bool> BuscarMail(string mail);
public  Task<bool> BuscarId(string id);
public  Task<bool> VerificarPasswordActual(string id, string passwordActual);
public  Task BajaLogica(string id);
public  Task ModificarUsuario(string id, Usuario usuario);
public  Task<Usuario> ObtenerPorId(string id);
public  Task<List<Usuario >> ListarUsuarios();
public Task<Usuario> ObtenerPorMail(string mail);
public  Task<bool> ExisteMail(string mail);

public  Task ReactivarAlumno(string mail);


}