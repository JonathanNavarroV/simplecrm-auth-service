using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Features.Users.Queries.GetUserByRun;

namespace Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registra servicios de la capa Application (ej. MediatR handlers).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registra MediatR buscando handlers en este ensamblado (Application)
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
