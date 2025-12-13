using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authorization;

/// <summary>
/// Policy provider dinámico que reconoce políticas con prefijo "Permission:".
/// Convierte la parte posterior al requirement correspondiente.
/// </summary>
public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private const string Prefix = "Permission:";
    private readonly DefaultAuthorizationPolicyProvider _fallback;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallback = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
        {
            var permission = policyName.Substring(Prefix.Length);

            // Si el permiso viene en forma corta MODULE:ACTION (2 partes), lo
            // expandimos a MODULE:MODULE:ACTION para evitar repetir el módulo
            // en cada atributo (p. ej. USERS:READ -> USERS:USERS:READ).
            var parts = permission.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                var module = parts[0];
                var action = parts[1];
                permission = string.Join(':', module, module, action);
            }

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(permission))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallback.GetPolicyAsync(policyName);
    }
}
