using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class Permissions
{
    public static readonly Permission[] SeedData;

    static Permissions()
    {
        // Mapear módulos por Id para obtener su código
        var modulesById = PermissionModules.SeedData.ToDictionary(m => m.Id, m => m.Code);

        var list = new List<Permission>();

        var counter = 0;
        foreach (var section in PermissionSections.SeedData)
        {
            var moduleCode = modulesById.TryGetValue(section.PermissionModuleId, out var mc) ? mc : "MODULE";

            foreach (var type in PermissionTypes.SeedData)
            {
                var code = $"{moduleCode}:{section.Code}:{type.Code}";
                counter++;
                var id = Guid.Parse($"00000000-0000-0000-0000-{counter:D12}");

                list.Add(new Permission
                {
                    Id = id,
                    Code = code,
                    Description = $"Permiso para {type.Name} en la sección {section.Name}",
                    IsActive = true,
                    PermissionSectionId = section.Id,
                    PermissionTypeId = type.Id
                });
            }
        }

        SeedData = list.ToArray();
    }

    
}
