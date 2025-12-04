using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Interfaces;
using Application.Features.Users.Dto;

namespace Application.Features.Users.Queries.GetUserByRun;

public class GetUserByRunQueryHandler : IRequestHandler<GetUserByRunQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByRunQueryHandler(IUserRepository userRepository)
    =>_userRepository = userRepository;

    public async Task<UserDto?> Handle(GetUserByRunQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRunAsync(request.Run);
        if (user is null) return null;

        return new UserDto
        {
            Run = user.Run,
            Dv = user.Dv,
            FirstNames = user.FirstNames,
            LastNames = user.LastNames,
            Email = user.Email,
            IsActive = user.IsActive,
            IsDeleted = user.IsDeleted
        };
    }
}
