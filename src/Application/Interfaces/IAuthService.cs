using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Intercambia un token externo (Bearer) por un JWT interno firmado por el servicio.
    /// </summary>
    /// <param name="bearerToken">Token externo (sin prefijo "Bearer ").</param>
    /// <returns>Devuelve un objeto con el resultado o la razón del fallo para permitir distinguir 401/403.</returns>
    Task<AuthExchangeOutcome> ExchangeAsync(string bearerToken);
}

public record AuthExchangeResult(string InternalToken, DateTime Expires);
public record AuthExchangeOutcome(bool Success, AuthExchangeResult? Result, string? FailureCode);
