using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sportify.Infraestructura.Identity;
using System;

namespace Sportify.Infraestructura.Data;

//conexion a la base de datos, hereda de IdentityDbContext para manejar la autenticacion y autorizacion 
public class ApplicationDbContext : IdentityDbContext<UsuarioIdentity> //esta clase es las coneccion/puente entre c# y SQLite y la clase de la que esta heredando es la que dice que la base de datos va a usar ASP.NET Identity para manejar la autenticacion y autorizacion, el tipo de usuario es ApplicationUser que es una clase personalizada que hereda de IdentityUser y agrega propiedades adicionales para el usuario.
{ //la clase Usuario puede ser cualquiera creada por nosotros, lo importante es que herede de IdentityUser para que pueda ser utilizada por IdentityDbContext y así manejar la autenticacion y autorizacion de los usuarios en la aplicación.
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //aca irian los DbSet de las entidades que queremos que se creen en la base de datos, por ejemplo:
    //public DbSet<Producto> Productos { get; set; }
    //estos crean una tabla en la base de datos por cada DbSet que declaremos, el nombre de la tabla sera el mismo que el nombre del DbSet, en este caso "Productos", y las columnas de la tabla seran las propiedades de la clase Producto.
    public DbSet<Sportify.Dominio.Deportes.Deporte> Deportes { get; set; }
    public DbSet<Sportify.Dominio.Turnos.Turno> Turnos { get; set; }
    public DbSet<Sportify.Dominio.Reservas.Reserva> Reservas { get; set; }
}