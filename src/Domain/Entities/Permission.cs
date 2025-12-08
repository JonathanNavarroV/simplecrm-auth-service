using System;

namespace Domain.Entities;

public class Permission
{
	public Guid Id { get; set; }
	public required string Code { get; set; }
	public required string Description { get; set; }

	public bool IsActive { get; set; } = true;
    
	// Relaciones
	public Guid PermissionSectionId { get; set; }
	public PermissionSection PermissionSection { get; set; } = null!;
    
	public Guid PermissionTypeId { get; set; }
	public PermissionType PermissionType { get; set; } = null!;
}
