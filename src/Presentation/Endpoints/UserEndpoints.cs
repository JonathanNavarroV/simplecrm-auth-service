using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Users.Queries.GetUserByRun;
using System.Security.Claims;

namespace Presentation.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // Agrupar endpoints de usuarios. Al aplicar RequireAuthorization al grupo,
        // todos los endpoints dentro heredarán la política de autorización.
        // Grupo principal: rutas bajo /users (el frontend consulta `/users/me`)
        var users = app.MapGroup("/users")
            .RequireAuthorization()
            .WithTags("Users");

        // Obtener usuario por RUN (ejemplo básico)
        users
            .MapGet("/{run:int}", GetUserByRun)
            .WithMetadata(new Attributes.PermissionsAttribute("USERS:READ"))
            .WithName("GetUserByRun")
            .WithSummary("Obtener usuario por RUN")
            .WithDescription("Obtiene la información de un usuario dado su RUN")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Obtener información del usuario autenticado usando el RUN presente en las claims del token
        users
            .MapGet("/me", GetCurrentUser)
            .WithName("GetCurrentUser")
            .WithSummary("Obtener usuario autenticado")
            .WithDescription("Obtiene la información del usuario autenticado a partir de las claims del token")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    // Handler: obtener usuario por RUN
    private static async Task<IResult> GetUserByRun(int run, IMediator mediator)
    {
        try
        {
            var user = await mediator.Send(new GetUserByRunQuery(run));
            return user is null ? Results.NotFound() : Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                title: "Error al obtener usuario",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    // Handler: obtener usuario autenticado (extrae RUN de las claims)
    private static async Task<IResult> GetCurrentUser(HttpContext http, IMediator mediator)
    {
        try
        {
            var claim = http.User.FindFirst("run")
                        ?? http.User.FindFirst(ClaimTypes.NameIdentifier)
                        ?? http.User.FindFirst("sub");

            if (claim is null)
            {
                return Results.Unauthorized();
            }

            if (!int.TryParse(claim.Value, out var run))
            {
                return Results.BadRequest("Claim 'run' inválida");
            }

            var user = await mediator.Send(new GetUserByRunQuery(run));
            return user is null ? Results.NotFound() : Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                title: "Error al obtener información del usuario autenticado",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
