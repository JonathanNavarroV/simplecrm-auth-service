using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class RolePermissions
{
    public static readonly RolePermission[] SeedData;

    static RolePermissions()
    {
        // Usar el Id fijo del rol Administrador (definido en Roles.SeedData)
        var adminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        // Asignar todos los permisos disponibles al rol administrador
        SeedData = Permissions.SeedData
            .Select(p => new RolePermission
            {
                RoleId = adminRoleId,
                PermissionId = p.Id
            })
            .ToArray();
    }
}
