using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Sportify.Infraestructura.Data;

public static class RepositoriosSQLites
{
    // Aplica migraciones y crea la base de datos si es necesario.
    public static void Inicializar(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        var loggerFactory = services.GetService<ILoggerFactory>();
        var logger = loggerFactory?.CreateLogger("RepositoriosSQLites");

        try
        {
            var db = services.GetRequiredService<ApplicationDbContext>();
            /*
            // Aplica migraciones pendientes. Si preferís EnsureCreated(), cambiar aquí.
            db.Database.Migrate();
            */
            // Utilizamos EnsureCreated en lugar de Migrate porque el historial de migraciones
            // a veces no sincroniza bien en dev con SQLite.
            db.Database.EnsureCreated();
            AsegurarColumnaAbonado(db);
        }
        catch (InvalidOperationException ex) when (ex.Message?.IndexOf("pending changes", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                   ex.Message?.IndexOf("PendingModelChangesWarning", StringComparison.OrdinalIgnoreCase) >= 0)
        {
            // Cuando el modelo cambió y hay migraciones pendientes, como fallback en dev
            // intentamos EnsureCreated() para evitar bloquear la aplicación.
            logger?.LogWarning(ex, "Pending model changes detected. Falling back to EnsureCreated(). Consider adding a migration.");
            try
            {
                var db = services.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            }
            catch (Exception innerEx)
            {
                logger?.LogError(innerEx, "Fallback EnsureCreated() also failed");
                throw;
            }
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error inicializando la base de datos SQLite");
            throw;
        }
    }

    private static void AsegurarColumnaAbonado(ApplicationDbContext db)
    {
        var connection = db.Database.GetDbConnection();
        var estabaAbierta = connection.State == System.Data.ConnectionState.Open;

        if (!estabaAbierta)
        {
            connection.Open();
        }

        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA table_info('Reservas')";

        using var reader = command.ExecuteReader();
        var columnas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (reader.Read())
        {
            columnas.Add(reader.GetString(1));
        }

        if (!columnas.Contains("abonado"))
        {
            using var alterCommand = connection.CreateCommand();
            alterCommand.CommandText = "ALTER TABLE \"Reservas\" ADD COLUMN \"abonado\" BOOLEAN NOT NULL DEFAULT 0";
            alterCommand.ExecuteNonQuery();
        }

        if (!estabaAbierta)
        {
            connection.Close();
        }
    }

    public static async Task SeedUsuariosAdmin(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Sportify.Infraestructura.Identity.UsuarioIdentity>>();

        string[] emails = { "admin@mail.com", "admin2@mail.com", "admin3@mail.com", "adminsportify@gmail.com" };
        string password = "123456";

        foreach (var email in emails)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new Sportify.Infraestructura.Identity.UsuarioIdentity
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    NombreCompleto = "admin",
                    Dni = "00000000",
                    EsAdmin = true
                };

                await userManager.CreateAsync(newUser, password);
            }
            else
            {
                // Ensure existing users have admin rights just in case
                if (!user.EsAdmin)
                {
                    user.EsAdmin = true;
                    await userManager.UpdateAsync(user);
                }
            }
        }
    }

    public static async Task SeedUsuariosNormales(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Sportify.Infraestructura.Identity.UsuarioIdentity>>();

        string[] emails = { "usuario1@mail.com", "usuario2@mail.com", "usuario3@mail.com" };
        string password = "123456";

        foreach (var email in emails)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var newUser = new Sportify.Infraestructura.Identity.UsuarioIdentity
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    NombreCompleto = email == "usuario1@mail.com" ? "Usuario Uno" : (email == "usuario2@mail.com" ? "Usuario Dos" : "Usuario Tres"),
                    Dni = email == "usuario1@mail.com" ? "11111111" : (email == "usuario2@mail.com" ? "22222222" : Guid.NewGuid().ToString().Substring(0, 8)),
                    EsAdmin = false
                };

                try 
                {
                    await userManager.CreateAsync(newUser, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating user {email}: {ex}");
                    throw;
                }
            }
        }
    }

    public static async Task SeedDeportes(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<ApplicationDbContext>();

        string[] nombres = { "Futbol", "Tenis", "Voley", "Paddle" };
        foreach (var nombre in nombres)
        {
            if (!await db.Deportes.AnyAsync(d => d.nombre == nombre))
            {
                var deporte = new Sportify.Dominio.Deportes.Deporte(nombre, $"Deporte: {nombre}", 2000);
                await db.Deportes.AddAsync(deporte);
            }
        }
        await db.SaveChangesAsync();
    }

    public static async Task SeedCreditos(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Sportify.Infraestructura.Identity.UsuarioIdentity>>();

        var usuario3 = await userManager.FindByEmailAsync("usuario3@mail.com");
        if (usuario3 != null)
        {
            var deportes = await db.Deportes.ToListAsync();
            foreach (var deporte in deportes)
            {
                var credito = await db.Creditos.FirstOrDefaultAsync(c => c.UsuarioId == Guid.Parse(usuario3.Id) && c.DeporteId == deporte.id);
                if (credito == null)
                {
                    credito = new Sportify.Dominio.Usuario.Credito(Guid.Parse(usuario3.Id), deporte.id);
                    credito.Cantidad = 1000;
                    await db.Creditos.AddAsync(credito);
                }
                else
                {
                    credito.Cantidad = 1000;
                }
            }
            await db.SaveChangesAsync();
        }
    }
}
