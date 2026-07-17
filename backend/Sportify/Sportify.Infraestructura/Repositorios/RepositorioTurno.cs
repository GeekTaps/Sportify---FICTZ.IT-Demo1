namespace Sportify.Infraestructura.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionTurnos;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Turnos;
using Sportify.Dominio.Asistencias;
using Sportify.Aplicacion.AplicacionAsistencias;


public class RepositorioTurno : IRepositorioTurno
{
    private readonly ApplicationDbContext archivo;

    public RepositorioTurno(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
    }

    public async Task<Turno> TraerTurnoPorId(Guid idTurno)
    {
        Turno? turno = await archivo.Turnos.FindAsync(idTurno);
        if (turno == null)
        {
            throw new Exception("Turno no encontrado");
        }
        return turno;
    }

    public async Task AltaTurno(Turno nuevoTurno)
    {
        await archivo.Turnos.AddAsync(nuevoTurno); //agrega el nuevo turno a archivo
        await archivo.SaveChangesAsync(); //guarda los cambios en la base de datos
    }

    public async Task<bool> ModificarTurno(Turno nuevoTurno, Guid idTurno) //metodo para modificar un turno
    {
        Turno? turno = await archivo.Turnos.FindAsync(idTurno); //busca el turno por su id
        if (turno != null)
        {
            turno.cupo = nuevoTurno.cupo;
            turno.cupoMaximo = nuevoTurno.cupoMaximo;
            turno.IdDeporte = nuevoTurno.IdDeporte;
            turno.nombreTurno = nuevoTurno.nombreTurno;
            turno.nommbreProfesor = nuevoTurno.nommbreProfesor;
            turno.Fecha = nuevoTurno.Fecha;
            turno.horaInicio = nuevoTurno.horaInicio;
            turno.horaFin = nuevoTurno.horaFin;
            turno.Precio = nuevoTurno.Precio;
            turno.ListaEsperaHabilitada = nuevoTurno.ListaEsperaHabilitada;
            turno.mostrarEnHome = nuevoTurno.mostrarEnHome;
            turno.IdHorario = nuevoTurno.IdHorario;
            await archivo.SaveChangesAsync(); //guarda los cambios en la base de datos
            return true;
        }
        return false; //devuelve true si se modifico el turno, false si no se encontro el turno
    }

    public async Task<bool> BuscarTurnoPorId(Guid idTurno) //metodo para obtener un turno por su id
    {
        Turno? turno = await archivo.Turnos.FindAsync(idTurno); //busca el turno por su id
        return turno != null; //devuelve true si se encuentra el turno, false si no existe
    }

    public async Task<Turno?> ObtenerTurnoPorId(Guid idTurno)
    {
        return await archivo.Turnos.FindAsync(idTurno);
    }
    
    public async Task<bool> existeTurnoAsociadoAlDeporte(Guid idDeporte) //metodo para verificar si un deporte tiene turnos asociados
    {
        return await archivo.Turnos.AnyAsync(t => t.IdDeporte == idDeporte); //devuelve true si hay turnos asociados al deporte, false si no hay turnos asociados
    }

    public async Task<List<Turno>> ListarTurnos()
    {
        return await archivo.Turnos.ToListAsync();
    }
    
    public async Task<bool> EncontrarRepetido(Turno nuevoTurno)
    {
        return await archivo.Turnos.AnyAsync(t => t.IdDeporte == nuevoTurno.IdDeporte && t.Fecha == nuevoTurno.Fecha && t.horaInicio == nuevoTurno.horaInicio && t.horaFin == nuevoTurno.horaFin); //devuelve true si hay un turno repetido (mismo deporte, fecha, hora de inicio y hora de fin), false si no hay turnos repetidos
    }

    public async Task<bool> BajaTurno(Guid idTurno)
    {
        Turno? turno = await archivo.Turnos.FindAsync(idTurno);
        if (turno != null)
        {
            archivo.Turnos.Remove(turno);
            await archivo.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task actualizarMostrarEnHome()
    {
        var turnos = await archivo.Turnos.Where(t => t.mostrarEnHome).ToListAsync(); //obtiene todos los turnos que se muestran en home
        foreach (var turno in turnos)
        {
            // Construir la fecha/hora de fin del turno para compararla con el momento actual
            var fechaFinTurno = turno.Fecha.Date.Add(turno.horaFin.ToTimeSpan());
            if (fechaFinTurno <= DateTime.Now) // si el turno ya finalizó
            {
                turno.mostrarEnHome = false;
            }
        }
        await archivo.SaveChangesAsync();
    }
    public async Task<List<Turno>> ListarTurnosPorHorario(Guid idHorario)
    {
        return await archivo.Turnos
            .Where(t => t.IdHorario == idHorario)
            .OrderBy(t => t.Fecha)
            .ToListAsync();
    }

    public async Task<bool> HayLugarParaAbono(Guid idHorario)
    {
        var hoy = DateTime.Now.Date;

        var maxFecha = new DateTime(
            hoy.Year,
            hoy.Month,
            DateTime.DaysInMonth(hoy.Year, hoy.Month));

        if (hoy.Day >= 20)
            maxFecha = maxFecha.AddDays(10);

        var turnos = await archivo.Turnos
            .Where(t =>
                t.IdHorario == idHorario &&
                t.Fecha.Date >= hoy &&
                t.Fecha.Date <= maxFecha)
            .ToListAsync();

        if (!turnos.Any())
            throw new Exception("No existen turnos para ese horario.");

        return turnos.All(t => t.cupo > 0);
    }
    public async Task<List<Asistencia>> FiltrarAsistencias(List<Asistencia> asistencias)
    {
    // 1. Obtenemos las variables de comparación
    DateTime fechaActual = DateTime.Now.Date;
    TimeOnly horaActual = TimeOnly.FromDateTime(DateTime.Now);

    // 2. Extraemos todos los IDs de turnos únicos que tienen las asistencias
    var idsTurnos = asistencias.Select(a => a.IdTurno).Distinct().ToList();

    // 3. Consultamos de un solo viaje todos los turnos que necesitamos
    // (Asumo que tenés un método en tu servicio/repositorio para traer varios turnos, o los traés todos juntos)
    var tareasTurnos = idsTurnos.Select(id => this.TraerTurnoPorId(id));
    var turnosConsultados = await Task.WhenAll(tareasTurnos);

    // 4. Creamos un diccionario (ID -> Turno) para buscar al instante (Complejidad O(1))
    var diccionarioTurnos = turnosConsultados
        .Where(t => t != null)
        .ToDictionary(t => t.Id, t => t);

    // 5. Filtramos la lista usando el diccionario y aplicando la lógica del día/hora
    return asistencias
        .Where(a => 
        {
            // Buscamos el turno en el diccionario en memoria
            if (!diccionarioTurnos.TryGetValue(a.IdTurno, out var turno))
                return false; // Si el turno no existe, lo descartamos

            //retorna una lista de asistencias que corresponden a turnos futuros, es decir, que la fecha del turno sea mayor a la fecha actual o que la fecha del turno sea igual a la fecha actual y la hora de fin del turno sea mayor a la hora actual
            return turno.Fecha > fechaActual 
                   || (turno.Fecha == fechaActual && turno.horaFin > horaActual);
        })
        .ToList();
    }
}