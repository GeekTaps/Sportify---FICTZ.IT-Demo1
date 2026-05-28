using System;

namespace Sportify.Aplicacion;

public class EntidadAsociadaException : Exception
{
    public EntidadAsociadaException(string st) : base(st) { }
}
/*
Se lanza si se intenta eliminar una entidad que está asociada a otra
Por ejemplo, si se intenta eliminar un deporte que tiene turnos asociados.
*/