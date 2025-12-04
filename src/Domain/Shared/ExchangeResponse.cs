using System;

namespace SimpleCRM.Domain.Shared;

public record ExchangeResponse(bool Success, string? Token, DateTime? Expires, string? FailureCode)
{
    public static ExchangeResponse Unauthorized() => new(false, null, null, "Unauthorized");
    public static ExchangeResponse Forbidden() => new(false, null, null, "Forbidden");
    public static ExchangeResponse UserNotFound() => new(false, null, null, "UserNotFound");
    public static ExchangeResponse FromSuccess(string token, DateTime expires) => new(true, token, expires, null);
}
