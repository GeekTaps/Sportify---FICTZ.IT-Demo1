namespace Sportify.Aplicacion.Excepciones;
public class UsuarioSuspendidoException : Exception
{
    public UsuarioSuspendidoException(string st) : base(st) { }
}

/*
    Se lanza cuando un usuario suspendido intenta hacer algo que no puede hacer
*/