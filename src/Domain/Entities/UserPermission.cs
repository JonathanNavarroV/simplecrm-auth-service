using System;

namespace Domain.Entities;

public class UserPermission
{
    // La entidad User usa `Run` (int) como PK, por eso aquí usamos `UserRun` de tipo int.
    public int UserRun { get; set; }
    public User User { get; set; } = null!;

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
