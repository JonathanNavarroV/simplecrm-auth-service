using System;

namespace Domain.Entities;

public class PermissionSection
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Foreign key hacia PermissionModule
    public Guid PermissionModuleId { get; set; }

    // Navegación inversa
    public PermissionModule PermissionModule { get; set; } = null!;
}
