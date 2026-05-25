namespace Sportify.Aplicacion.Excepciones;
public class ValidacionException : Exception
{
    public ValidacionException(string st) : base(st) { }
}
/*
Se lanza si se intenta crear o modificar una entidad que no cumple con las reglas de negocio
*/