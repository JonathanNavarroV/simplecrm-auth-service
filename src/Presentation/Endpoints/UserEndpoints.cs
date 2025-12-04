using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Users.Queries.GetUserByRun;

namespace Presentation.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        // Obtener usuario por RUN (ejemplo básico)
        app.MapGet("/users/{run:int}", async (int run, [FromServices] IMediator mediator) =>
        {
            var user = await mediator.Send(new GetUserByRunQuery(run));
            return user is null ? Results.NotFound() : Results.Ok(user);
        })
        .RequireAuthorization()
        .WithName("GetUserByRun")
        .WithTags("Users");

        return app;
    }
}
