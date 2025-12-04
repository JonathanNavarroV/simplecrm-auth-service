using MediatR;
using Application.Interfaces;
using System.Security.Claims;
using SimpleCRM.Domain.Entities;
using SimpleCRM.Domain.Shared;

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

        // (No logging here to keep Application project lightweight)

        // Prefer external subject (sub/oid) if present
        var externalId = principal.FindFirst("sub")?.Value ?? principal.FindFirst("oid")?.Value;

        User? user = null;
        // Note: currently there's no ExternalId column in users; fallback to email lookup
        var email = principal.FindFirst("email")?.Value ?? principal.FindFirst("upn")?.Value ?? principal.FindFirst("preferred_username")?.Value;
        Console.WriteLine($"Exchange: external token email claim = '{email}'");
        if (!string.IsNullOrEmpty(email))
        {
            user = await _userRepository.GetByEmailAsync(email, includeDeleted: false, includeInactive: false);
            Console.WriteLine(user is null ? $"Exchange: no user found for email '{email}'" : $"Exchange: found user Run={user.Run} Email={user.Email}");
        }

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

        var result = await _tokenFactory.CreateInternalTokenAsync(claims);
        return ExchangeResponse.FromSuccess(result.InternalToken, result.Expires);
    }
}
