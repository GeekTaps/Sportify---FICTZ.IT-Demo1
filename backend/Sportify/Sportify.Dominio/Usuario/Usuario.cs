namespace Sportify.Dominio.Usuario;
using System;
public class Usuario{ //Aca esta usuario, si lo se debí sacarlo, pero la interfaz no conoce las cosas del repositorio (o sea todo lo relacionado con identity)
                      //Y necesito que implemente bien los metodos (que reciben un usuario)
                      //por lo tanto implemento una clase usuario asi tranqui
                      //la cual las interfaces van a conocer
                      //y en el repositorio yo transformo el usuario comun a un usuarioidentity
                      //o sea o movemos identity a la aplicacion (Se pudre todo)
                      //o hago que los metodos que tengan que ver con un usuario manden campo por campo(una paja pero se puede hacer no hay problem)
public string NombreCompleto { get; set; } = "";

    public string Id { get; set; } = "";
    public int Creditos { get; set; } = 0;
    
    public string Mail { get; set; }
    public string Dni { get; set; }
    public string PasswordActual { get; set; } = "";
    public string PasswordNueva { get; set; } = "";
    public bool Suspendido { get; set; } = false;
    public bool SuspendidoPermanente { get; set; } = false;
    public DateTime FechaNacimiento { get; set; }
    
    public Usuario(string nombre, string Mail, string Dni, string Contraseña, string? passwordNueva, DateTime fechaNacimiento)
    {
        this.Suspendido = false;
        this.SuspendidoPermanente = false;
        this.NombreCompleto = nombre;
        this.Mail = Mail;
        this.Dni= Dni;
        this.PasswordActual = Contraseña;
        this.PasswordNueva = passwordNueva;
        this.FechaNacimiento= fechaNacimiento;
    }

    public Usuario(string id, string nombre, string Mail, string Dni, string Contraseña, string? passwordNueva, DateTime fechaNacimiento, int creditos)
    {
        this.Id = id;
        this.NombreCompleto = nombre;
        this.Mail = Mail;
        this.Dni= Dni;
        this.PasswordActual = Contraseña;
        this.PasswordNueva = passwordNueva;
        this.FechaNacimiento= fechaNacimiento;
        this.Creditos = creditos;
    }
}
