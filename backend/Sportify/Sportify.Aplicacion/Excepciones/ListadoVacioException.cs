using System;

namespace Sportify.Aplicacion;

public class ListadoVacioException : Exception
{
    public ListadoVacioException(string st) : base(st) { }
}
/*
Se lanza si se intenta mostrar un listado pero está vacío
*/