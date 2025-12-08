using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class PermissionModules
{
    public static readonly PermissionModule[] SeedData = new[]
    {
        new PermissionModule
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Code = "USERS",
            Name = "Usuarios",
            Description = "Gestión de usuarios y autenticación",
            IsActive = true
        }
    };
}
