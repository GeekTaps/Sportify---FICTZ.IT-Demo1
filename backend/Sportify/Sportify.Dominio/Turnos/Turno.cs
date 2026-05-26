namespace Sportify.Dominio.Turnos;
public class Turno
{
    public Guid Id { get; set; } //identificador unico del turno
    public DateTime Fecha { get; set; } //fecha y hora del turno    
    public int cupo { get; set; } //cantidad de personas que pueden asistir al turno
    public Guid IdDeporte { get; set; } //identificador de la actividad asociada al turno
    public String nombreTurno { get; set; } //nombre del turno (opcional, chequear con front si es realmente necesario)
    public String nommbreProfesor { get; set; } //nombre del profesor que dicta el turno (opcional, chequear con front si es realmente necesario)
    public TimeOnly horaInicio { get; set; } //hora de inicio del turno
    public TimeOnly horaFin { get; set; } //hora de fin del turno

}