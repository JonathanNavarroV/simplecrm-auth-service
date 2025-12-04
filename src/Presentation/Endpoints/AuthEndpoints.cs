using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Application.Interfaces;
using MediatR;
using Application.Features.Auth;

namespace Presentation.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/exchange", async (HttpContext ctx, IMediator mediator) =>
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
                Expires = new DateTimeOffset(response.Expires!.Value)
            };
            // Keep cookie for browser-based flows
            ctx.Response.Cookies.Append("internal_token", response.Token!, cookieOptions);

            // Expose the internal token in the response Authorization header so a trusted
            // gateway can consume it and replace the incoming Authorization header.
            // Note: do NOT return the token in the response body (avoids storing token in JS-accessible places).
            ctx.Response.Headers["Authorization"] = "Bearer " + response.Token!;
            return Results.Ok(new { message = "Token exchange successful" });
        })
        .WithName("ExchangeToken")
        .WithTags("Auth");

        return app;
    }
}
