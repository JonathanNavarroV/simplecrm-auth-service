using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Interfaces;
using Application.Features.Users.Dto;
using System.Collections.Generic;

namespace Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserDto?>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    => _userRepository = userRepository;

    public async Task<CurrentUserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRunAsync(request.Run);
        if (user is null) return null;

        var perms = await _userRepository.GetPermissionCodesAsync(request.Run);
        var roles = await _userRepository.GetRoleNamesAsync(request.Run);

        return new CurrentUserDto
        {
            Run = user.Run,
            Dv = user.Dv,
            FirstNames = user.FirstNames,
            LastNames = user.LastNames,
            Email = user.Email,
            IsActive = user.IsActive,
            IsDeleted = user.IsDeleted,
            Roles = roles is null ? new List<string>() : new List<string>(roles),
            Permissions = perms is null ? new List<string>() : new List<string>(perms)
        };
    }
}
