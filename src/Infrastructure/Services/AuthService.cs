using Application.Interfaces;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;
    private readonly IConfiguration _configuration;
    private readonly Application.Interfaces.IUserRepository _userRepository;

    public AuthService(ConfigurationManager<OpenIdConnectConfiguration> configManager, IConfiguration configuration, Application.Interfaces.IUserRepository userRepository)
    {
        _configManager = configManager;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<AuthExchangeOutcome> ExchangeAsync(string bearerToken)
    {
        if (string.IsNullOrWhiteSpace(bearerToken)) return new AuthExchangeOutcome(false, null, "InvalidToken");

        OpenIdConnectConfiguration openIdConfig;
        try
        {
            openIdConfig = await _configManager.GetConfigurationAsync(CancellationToken.None);
        }
        catch
        {
            return new AuthExchangeOutcome(false, null, "ConfigError");
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = _configuration.GetSection("Authentication:EntraId:ValidIssuers").Get<string[]>(),
            ValidateAudience = true,
            ValidAudiences = _configuration.GetSection("Authentication:EntraId:ValidAudiences").Get<string[]>(),
            IssuerSigningKeys = openIdConfig.SigningKeys,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        var handler = new JwtSecurityTokenHandler();
        ClaimsPrincipal principal;
        try
        {
            principal = handler.ValidateToken(bearerToken, validationParameters, out var validatedToken);
        }
        catch
        {
            return new AuthExchangeOutcome(false, null, "InvalidToken");
        }
        // Obtener email (o claim alternativo) del principal
        var email = principal.FindFirstValue("email")
                    ?? principal.FindFirstValue("upn")
                    ?? principal.FindFirstValue("preferred_username");

        if (string.IsNullOrEmpty(email))
        {
            return new AuthExchangeOutcome(false, null, "MissingEmailClaim");
        }

        // Validar que el usuario exista en la base de datos y esté activo
        var user = await _userRepository.GetByEmailAsync(email, includeDeleted: false, includeInactive: false);
        if (user is null)
        {
            return new AuthExchangeOutcome(false, null, "UserNotFound");
        }

        if (!user.IsActive || user.IsDeleted)
        {
            return new AuthExchangeOutcome(false, null, "UserInactiveOrDeleted");
        }

        // Derivar claims necesarios para el token interno
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Run.ToString()),
            new Claim(ClaimTypes.Name, user.FirstNames + " " + user.LastNames),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("provider", "entra")
        };

        var internalToken = CreateInternalJwt(claims);
        var expires = DateTime.UtcNow.AddMinutes(60);

        return new AuthExchangeOutcome(true, new AuthExchangeResult(internalToken, expires), null);
    }

    private string CreateInternalJwt(IEnumerable<Claim> claims)
    {
        var keyBase64 = _configuration["Jwt:SigningKeyBase64"];
        if (string.IsNullOrEmpty(keyBase64)) throw new InvalidOperationException("Jwt:SigningKeyBase64 no configurada");

        var key = new SymmetricSecurityKey(Convert.FromBase64String(keyBase64));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
