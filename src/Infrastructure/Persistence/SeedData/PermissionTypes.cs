using System;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class PermissionTypes
{
    public static readonly PermissionType[] SeedData = new[]
    {
        new PermissionType
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Code = "CREATE",
            Name = "Creación",
            Description = "Permiso para crear recursos",
            IsActive = true
        },
        new PermissionType
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            Code = "READ",
            Name = "Lectura",
            Description = "Permiso para leer recursos",
            IsActive = true
        },
        new PermissionType
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
            Code = "WRITE",
            Name = "Escritura",
            Description = "Permiso para modificar recursos",
            IsActive = true
        },
        new PermissionType
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
            Code = "DELETE",
            Name = "Eliminación",
            Description = "Permiso para eliminar recursos",
            IsActive = true
        }
    };
}
