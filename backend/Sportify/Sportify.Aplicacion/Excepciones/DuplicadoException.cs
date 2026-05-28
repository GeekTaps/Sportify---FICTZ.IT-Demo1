namespace Sportify.Aplicacion.Excepciones;
public class DuplicadoException : Exception
{
    public DuplicadoException(string st) : base(st) { }
}
/*
    Se lanza cuando se intenta crear o modificar una entidad con los identificadores
    de otra
*/