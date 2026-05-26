namespace Sportify.Infraestructura.Repositorios;
using System;

using Sportify.Infraestructura.Data;
using Sportify.Dominio;
using Sportify.Aplicacion.AplicacionTurnos;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Sportify.Dominio.Turnos;
public class RepositorioTurno : IRepositorioTurno
{
    private readonly ApplicationDbContext archivo;
    public void AltaTurno (Turno nuevoTurno)
    {
        {
            archivo.Turnos.Add(nuevoTurno); //agrega el nuevo turno a archivo
            archivo.SaveChanges(); //guarda los cambios en la base de datos
        }
    }

    public bool ModificarTurno(Turno nuevoTurno, Guid idTurno) //metodo para modificar un turno
    {
        Turno? turno= archivo.Turnos.Find(idTurno); //busca el turno por su id
        if (turno != null)
        {
            turno= nuevoTurno; //modifica el turno encontrado con los nuevos datos
            archivo.SaveChanges(); //guarda los cambios en la base de datos
        }
        return turno != null; //devuelve true si se modifico el turno, false si no se encontro el turno
    }

    public bool BuscarTurnoPorId(Guid idTurno) //metodo para obtener un turno por su id
    {
        Turno? turno = archivo.Turnos.Find(idTurno); //busca el turno por su id
        return turno != null; //devuelve true si se encuentra el turno, false si no existe
    }
    
}