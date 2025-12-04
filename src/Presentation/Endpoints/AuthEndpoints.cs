using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Application.Interfaces;

namespace Presentation.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/exchange", async (HttpContext ctx, IAuthService authService) =>
        {
            var authHeader = ctx.Request.Headers["Authorization"].ToString();
            var token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring(7) : null;
            if (string.IsNullOrEmpty(token)) return Results.Unauthorized();

            var outcome = await authService.ExchangeAsync(token);
            if (!outcome.Success)
            {
                // Distinguimos razones: InvalidToken/MissingEmail/Config -> 401, UserNotFound/UserInactive -> 403
                if (outcome.FailureCode == "InvalidToken" || outcome.FailureCode == "MissingEmailClaim" || outcome.FailureCode == "ConfigError")
                    return Results.Unauthorized();

                return Results.StatusCode(StatusCodes.Status403Forbidden);
            }

            var result = outcome.Result!;

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = new DateTimeOffset(result.Expires)
            };
            ctx.Response.Cookies.Append("internal_token", result.InternalToken, cookieOptions);

            return Results.Ok(new { message = "Token exchange successful" });
        })
        .WithName("ExchangeToken")
        .WithTags("Auth");

        return app;
    }
}
