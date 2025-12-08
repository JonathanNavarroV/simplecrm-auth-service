using System;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class PermissionSections
{
    public static readonly PermissionSection[] SeedData = new[]
    {
        new PermissionSection
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Code = "USERS",
            Name = "Usuarios",
            Description = "Sección para gestión de usuarios",
            IsActive = true,
            PermissionModuleId = Guid.Parse("00000000-0000-0000-0000-000000000001")
        }
        ,
        new PermissionSection
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Code = "ROLES",
            Name = "Roles",
            Description = "Sección para gestión de roles",
            IsActive = true,
            PermissionModuleId = Guid.Parse("00000000-0000-0000-0000-000000000001")
        },
        new PermissionSection
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
            Code = "PERMISSIONS",
            Name = "Permisos",
            Description = "Sección para gestión de permisos",
            IsActive = true,
            PermissionModuleId = Guid.Parse("00000000-0000-0000-0000-000000000001")
        }
    };
}
