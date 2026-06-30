using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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

    public static async Task SeedUsuariosAdmin(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Sportify.Infraestructura.Identity.UsuarioIdentity>>();

        string[] emails = { "admin@mail.com", "admin2@mail.com", "admin3@mail.com" };
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
}
