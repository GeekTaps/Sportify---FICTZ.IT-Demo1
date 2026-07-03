namespace Sportify.Dominio.Turnos;
public class Horario
{
    public Guid id { get; set; } //identificador unico del horario
    public Guid idDeporte { get; set; } //identificador unico del deporte
    public String diaSemana { get; set; } //dia de la semana del horario    
    public TimeOnly hora { get; set; } //hora
    public bool eliminado { get; set; } //indica si el horario esta eliminado logicamente.
    // public bool mostrarEnHome { get; set; } = true; //indica si el turno se muestra en la pagina principal (home) o no, por defecto se muestra en home.


    public Horario(Guid idDeporte, String diaSemana, TimeOnly hora)
    {
        this.id = Guid.NewGuid();
        this.idDeporte = idDeporte;
        this.diaSemana = diaSemana;
        this.hora = hora;
        this.eliminado = false;
    }

    public void eliminarLogicamente()
    {
        this.eliminado = true;
    }

}