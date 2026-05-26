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

        try
        {
            var db = services.GetRequiredService<ApplicationDbContext>();
            // Aplica migraciones pendientes. Si preferís EnsureCreated(), cambiar aquí.
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            var loggerFactory = services.GetService<ILoggerFactory>();
            var logger = loggerFactory?.CreateLogger("RepositoriosSQLites");
            logger?.LogError(ex, "Error inicializando la base de datos SQLite");
            throw;
        }
    }
}
