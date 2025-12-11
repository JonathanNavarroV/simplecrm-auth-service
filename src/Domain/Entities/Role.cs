using System;

namespace Domain.Entities;

public class Role
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    // Estado
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    // Auditoría
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserRun { get; set; }

    public DateTime UpdatedAt { get; set; }
    public int UpdatedByUserRun { get; set; }

    public DateTime DeletedAt { get; set; }
    public int? DeletedByUserRun { get; set; }
}
