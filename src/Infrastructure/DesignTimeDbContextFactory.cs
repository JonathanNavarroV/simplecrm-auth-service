using System;
using System.IO;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    /// <summary>
    /// Fábrica para crear el `ApplicationDbContext` en tiempo de diseño para las herramientas de EF Core (migraciones).
    /// Intenta leer la cadena de conexión desde la variable de entorno `ConnectionStrings__AuthDb`
    /// y, en su defecto, desde el `appsettings.json` del proyecto `Presentation`.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Preferir la variable de entorno (útil para CI / entornos locales)
            var conn = Environment.GetEnvironmentVariable("ConnectionStrings__AuthDb");

            if (string.IsNullOrEmpty(conn))
            {
                // Como fallback, leer `Presentation/appsettings.json` (al ejecutar desde el repo)
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Presentation");
                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                conn = config.GetConnectionString("AuthDb");
            }

            if (string.IsNullOrEmpty(conn))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión para ApplicationDbContext. Configure 'ConnectionStrings:AuthDb' como variable de entorno o en Presentation/appsettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(conn);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
