using MediatR;
using Application.Interfaces;
using System.Security.Claims;
using Domain.Entities;
using Domain.Shared;

namespace Application.Features.Auth;

public class ExchangeCommandHandler : IRequestHandler<ExchangeCommand, ExchangeResponse>
{
    private readonly ITokenValidator _tokenValidator;
    private readonly IUserRepository _userRepository;
    private readonly ITokenFactory _tokenFactory;

    public ExchangeCommandHandler(ITokenValidator tokenValidator, IUserRepository userRepository, ITokenFactory tokenFactory)
    {
        _tokenValidator = tokenValidator;
        _userRepository = userRepository;
        _tokenFactory = tokenFactory;
    }

    public async Task<ExchangeResponse> Handle(ExchangeCommand request, CancellationToken cancellationToken)
    {
        var validate = await _tokenValidator.ValidateAsync(request.BearerToken);
        if (!validate.Success)
            return ExchangeResponse.Unauthorized();


        var principal = validate.Principal!;

        User? user = null;

        // Buscar al usuario únicamente por email (claims comunes). Si no hay email, fallamos.
        var email = principal.FindFirst("email")?.Value
                    ?? principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                    ?? principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value
                    ?? principal.FindFirst("upn")?.Value
                    ?? principal.FindFirst("preferred_username")?.Value
                    ?? principal.FindFirst("name")?.Value;

        if (string.IsNullOrEmpty(email))
        {
            // No tenemos email en las claims -> no podemos identificar al usuario
            return ExchangeResponse.UserNotFound();
        }

        user = await _userRepository.GetByEmailAsync(email, includeDeleted: false, includeInactive: false);

        if (user is null)
            return ExchangeResponse.UserNotFound();

        if (!user.IsActive || user.IsDeleted)
            return ExchangeResponse.Forbidden();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Run.ToString()),
            new Claim(ClaimTypes.Name, user.FirstNames + " " + user.LastNames),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("provider", "entra")
        };

        // Añadir permisos del usuario como claims 'permission'
        var permissionCodes = await _userRepository.GetPermissionCodesAsync(user.Run);
        foreach (var code in permissionCodes)
        {
            claims.Add(new Claim("permission", code));
        }

        var result = await _tokenFactory.CreateInternalTokenAsync(claims);
        return ExchangeResponse.FromSuccess(result.InternalToken, result.Expires);
    }
}
