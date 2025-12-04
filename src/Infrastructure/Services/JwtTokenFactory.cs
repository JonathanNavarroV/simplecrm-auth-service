using Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class JwtTokenFactory : ITokenFactory
{
    private readonly IConfiguration _configuration;

    public JwtTokenFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<AuthExchangeResult> CreateInternalTokenAsync(IEnumerable<Claim> claims)
    {
        var keyBase64 = _configuration["Jwt:SigningKeyBase64"];
        if (string.IsNullOrEmpty(keyBase64)) throw new InvalidOperationException("Jwt:SigningKeyBase64 no configurada");

        var key = new SymmetricSecurityKey(Convert.FromBase64String(keyBase64));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(60);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: creds
        );

        var written = new JwtSecurityTokenHandler().WriteToken(token);
        var result = new AuthExchangeResult(written, expires);
        return Task.FromResult(result);
    }
}
