namespace Domain.Entities;

public class User
{
    public required int Run { get; set; }
    public required string Dv { get; set; }
    public required string FirstNames { get; set; }
    public required string LastNames { get; set; }
    public required string Email { get; set; }

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
