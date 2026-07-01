namespace Sportify.Dominio.Asistencias;
public class Asistencia
{
    public Guid Id { get; set; }
    public Guid IdUsuario { get; set; }
    public Guid IdTurno { get; set; }
    public bool Presente { get; set; }
}