using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Clase estática con extensión para registrar todos los servicios de la capa Infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Método de extensión que registra EF Core (con Npgsql) y demás servicios de infraestructura.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Obtiene la cadena de conexión "AuthDb" definida en appsettings.json o variables de entorno
        var connectionString = configuration.GetConnectionString("AuthDb");

        // Registra el DbContext principal del AuthService (ApplicationDbContext)
        // Esto permite inyectar ApplicationDbContext en los servicios/controladores
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        // Devuelve el contenedor de servicios actualizado
        return services;
    }
}
