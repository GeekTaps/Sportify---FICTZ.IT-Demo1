using System;

namespace Sportify.Dominio.Abonos;

public class Abono
{
    public Guid Id { get; private set; }
    public Guid IdUsuario { get; private set; }
    public Guid IdHorario { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public bool Activo { get; private set; }
    public bool Eliminado { get; private set; }

    public Abono(Guid idUsuario, Guid idHorario)
    {
        Id = Guid.NewGuid();
        IdUsuario = idUsuario;
        IdHorario = idHorario;
        FechaInicio = DateTime.Now;
        Activo = true;
        Eliminado = false;
    }

    public void Cancelar()
    {
        Activo = false;
    }

    public void Eliminar()
    {
        Eliminado = true;
        Activo = false;
    }
}
