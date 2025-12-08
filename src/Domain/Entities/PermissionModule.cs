using System;

namespace Domain.Entities;

public class PermissionModule
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navegación: un módulo puede tener muchas secciones
    public ICollection<PermissionSection> Sections { get; set; } = new List<PermissionSection>();
}
