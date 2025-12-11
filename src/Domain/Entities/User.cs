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
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserRun { get; set; }

    public DateTime UpdatedAt { get; set; }
    public int UpdatedByUserRun { get; set; }

    public DateTime DeletedAt { get; set; }
    public int DeletedByUserRun { get; set; }
}
