using System;

namespace Domain.Entities;

public class UserRole
{
    // User primary key es `Run` (int)
    public int UserRun { get; set; }
    public User User { get; set; } = null!;

    // Role primary key es `Id` (Guid)
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}
