using System;

namespace Domain.Entities;

public class PermissionType
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public bool IsActive { get; set; } = true;
    
    // Un type puede aplicarse a muchos permisos
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
