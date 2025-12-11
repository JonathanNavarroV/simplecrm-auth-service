using System;

namespace Domain.Entities;

public class Role
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // Estado
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    // Auditoría
    public DateTime? CreatedAt { get; set; } = null;
    public int? CreatedByUserRun { get; set; } = null;

    public DateTime? UpdatedAt { get; set; } = null;
    public int? UpdatedByUserRun { get; set; } = null;

    public DateTime? DeletedAt { get; set; } = null;
    public int? DeletedByUserRun { get; set; } = null;
}
