using MediatR;
using Application.Features.Users.Dto;

namespace Application.Features.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery(int Run) : IRequest<CurrentUserDto?>;
