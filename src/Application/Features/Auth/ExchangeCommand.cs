using MediatR;
using Domain.Shared;

namespace Application.Features.Auth;

public record ExchangeCommand(string BearerToken) : IRequest<ExchangeResponse>;
