namespace Sportify.Infraestructura.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionTurnos;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Turnos;
public class RepositorioTurno : IRepositorioTurno
{
    private readonly ApplicationDbContext archivo;

    public RepositorioTurno(ApplicationDbContext archivo)
    {
        this.archivo = archivo;
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
            turno = nuevoTurno; //modifica el turno encontrado con los nuevos datos
            await archivo.SaveChangesAsync(); //guarda los cambios en la base de datos
        }
        return turno != null; //devuelve true si se modifico el turno, false si no se encontro el turno
    }

    public async Task<bool> BuscarTurnoPorId(Guid idTurno) //metodo para obtener un turno por su id
    {
        Turno? turno = await archivo.Turnos.FindAsync(idTurno); //busca el turno por su id
        return turno != null; //devuelve true si se encuentra el turno, false si no existe
    }
    
    public async Task<bool> existeTurnoAsociadoAlDeporte(Guid idDeporte) //metodo para verificar si un deporte tiene turnos asociados
    {
        return await archivo.Turnos.AnyAsync(t => t.IdDeporte == idDeporte); //devuelve true si hay turnos asociados al deporte, false si no hay turnos asociados
    }
    public async Task<List<Turno>> ListarTurnos()
    {
        return await archivo.Turnos.ToListAsync();
    }
}