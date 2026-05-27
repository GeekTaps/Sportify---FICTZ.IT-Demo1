using System;

namespace Sportify.Aplicacion.Excepciones;

public class EntidadRepetidaException : Exception
{
    public EntidadRepetidaException(string st) : base(st) { }
}
/*
Se lanza si se intenta crear una entidad que ya se encuentra registrada en el sistema (ej: Turno con mismo deporte, fecha, hora de inicio y hora de fin)
*/