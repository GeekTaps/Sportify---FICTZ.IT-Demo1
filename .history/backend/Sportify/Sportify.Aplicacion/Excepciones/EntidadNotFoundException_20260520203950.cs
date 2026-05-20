using System;

namespace Sportify.Aplicacion;

public class EntidadNotFoundException : Exception
{
    public EntidadNotFoundException(string st) : base(st) { }
}
/*
Se lanza si se intenta operar con un Id que no existe
(ej: UsuarioId, DeporteId o ReservaId no encontrados) (Consultar a repositorios correspondientes)
*/