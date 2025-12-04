using Application.Interfaces;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services;

public class OpenIdTokenValidator : ITokenValidator
{
    private readonly ConfigurationManager<OpenIdConnectConfiguration> _configManager;
    private readonly IConfiguration _configuration;

    public OpenIdTokenValidator(ConfigurationManager<OpenIdConnectConfiguration> configManager, IConfiguration configuration)
    {
        _configManager = configManager;
        _configuration = configuration;
    }

    public async Task<TokenValidationOutcome> ValidateAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return new TokenValidationOutcome(false, null, "InvalidToken");

        OpenIdConnectConfiguration openIdConfig;
        try
        {
            openIdConfig = await _configManager.GetConfigurationAsync(CancellationToken.None);
        }
        catch
        {
            return new TokenValidationOutcome(false, null, "ConfigError");
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
        try
        {
            var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
            return new TokenValidationOutcome(true, principal, null);
        }
        catch
        {
            return new TokenValidationOutcome(false, null, "InvalidToken");
        }
    }
}
