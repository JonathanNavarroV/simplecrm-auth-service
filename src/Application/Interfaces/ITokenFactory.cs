using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces;

public record AuthExchangeResult(string InternalToken, DateTime Expires);

public interface ITokenFactory
{
    Task<AuthExchangeResult> CreateInternalTokenAsync(IEnumerable<Claim> claims);
}
