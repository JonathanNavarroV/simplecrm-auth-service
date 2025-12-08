using System;

namespace Domain.Entities;

public class PermissionType
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
