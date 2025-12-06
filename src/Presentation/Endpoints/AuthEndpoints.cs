using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Application.Interfaces;
using MediatR;
using Application.Features.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        var auth = group
            .MapGroup("/auth")
            .WithTags("Auth")
            .WithSummary("Autenticación y sesión")
            .WithDescription("Operaciones relacionadas con autenticación: exchange de tokens, login, logout y refresh")
            .WithOpenApi();

        // POST /auth/exchange  (público)
        auth
            .MapPost("/exchange", Exchange)
            .AllowAnonymous()
            .WithName("ExchangeToken")
            .WithSummary("Intercambiar token externo por token interno")
            .WithDescription("Intercambia un token externo por un token interno y establece una cookie HttpOnly para flujos de navegador")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return auth;
    }

    // Handler: Exchange token externo por token interno y setear cookie HttpOnly
    private static async Task<IResult> Exchange(HttpContext ctx, IMediator mediator)
    {
        try
        {
            var authHeader = ctx.Request.Headers["Authorization"].ToString();
            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring(7) : null;
            if (string.IsNullOrEmpty(token)) return Results.Unauthorized();

            var response = await mediator.Send(new ExchangeCommand(token));
            if (!response.Success)
            {
                if (response.FailureCode == "Unauthorized") return Results.Unauthorized();
                if (response.FailureCode == "UserNotFound") return Results.NotFound();
                return Results.StatusCode(StatusCodes.Status403Forbidden);
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = response.Expires is null ? (DateTimeOffset?)null : new DateTimeOffset(response.Expires.Value)
            };

            // Mantener la cookie para flujos basados en navegador
            ctx.Response.Cookies.Append("internal_token", response.Token!, cookieOptions);

            // Exponer el token interno en el header Authorization para que un gateway de confianza lo use.
            ctx.Response.Headers["Authorization"] = "Bearer " + response.Token!;

            return Results.Ok(new { message = "Intercambio de token completado correctamente" });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                title: "Error al intercambiar token",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
