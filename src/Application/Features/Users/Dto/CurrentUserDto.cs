using System.Collections.Generic;

namespace Application.Features.Users.Dto;

public class CurrentUserDto
{
    public int Run { get; set; }
    public string? Dv { get; set; }
    public string FirstNames { get; set; } = default!;
    public string LastNames { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    // Roles y permisos para el cliente
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}
