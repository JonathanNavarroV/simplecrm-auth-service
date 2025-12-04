using MediatR;
using Application.Features.Users.Dto;

namespace Application.Features.Users.Queries.GetUserByRun;

public record GetUserByRunQuery(int Run) : IRequest<UserDto?>;
