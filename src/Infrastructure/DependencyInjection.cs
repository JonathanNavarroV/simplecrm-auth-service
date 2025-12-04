using Infrastructure.Persistence;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Infrastructure.Services;

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

        // Registrar repositorios de infraestructura
        services.AddScoped<IUserRepository, UserRepository>();

        // Registrar ConfigurationManager<OpenIdConnectConfiguration> para cachear JWKS
        var authority = configuration["Authentication:EntraId:Authority"];
        if (!string.IsNullOrEmpty(authority))
        {
            var metadataAddress = authority.TrimEnd('/') + "/.well-known/openid-configuration";
            services.AddSingleton(sp => new ConfigurationManager<OpenIdConnectConfiguration>(metadataAddress, new OpenIdConnectConfigurationRetriever()));
        }

        // Registrar implementaciones infra para token handling
        services.AddScoped<Application.Interfaces.ITokenValidator, OpenIdTokenValidator>();
        services.AddScoped<Application.Interfaces.ITokenFactory, JwtTokenFactory>();

        // Registrar servicios de la capa Application (handlers, MediatR, etc.)
        services.AddApplication();

        // Devuelve el contenedor de servicios actualizado
        return services;
    }
}
