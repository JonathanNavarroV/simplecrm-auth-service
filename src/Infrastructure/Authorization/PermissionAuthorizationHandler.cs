using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

/// <summary>
/// Handler que valida si el usuario tiene el claim `permission` correspondiente.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.Identity is not { IsAuthenticated: true })
        {
            return Task.CompletedTask;
        }

        var has = context.User.Claims
            .Where(c => c.Type == "permission")
            .Select(c => c.Value)
            .Any(p => string.Equals(p, requirement.Permission, StringComparison.OrdinalIgnoreCase));

        if (has)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
