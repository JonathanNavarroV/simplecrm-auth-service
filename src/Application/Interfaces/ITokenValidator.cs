using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces;

public record TokenValidationOutcome(bool Success, ClaimsPrincipal? Principal, string? FailureCode);

public interface ITokenValidator
{
    /// <summary>
    /// Valida un token externo (Bearer) usando el proveedor OpenID configurado.
    /// </summary>
    Task<TokenValidationOutcome> ValidateAsync(string token);
}
