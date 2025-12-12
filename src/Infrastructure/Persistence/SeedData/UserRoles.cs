using System;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class UserRoles
{
    public static readonly UserRole[] SeedData = new[]
    {
        new UserRole
        {
            UserRun = 19241027,
            RoleId = Guid.Parse("00000000-0000-0000-0000-000000000001")
        }
    };
}
