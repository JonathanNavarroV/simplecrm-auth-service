using System;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class Roles
{
	public static readonly Role[] SeedData = new[]
	{
		new Role
		{
			Id = new Guid("00000000-0000-0000-0000-000000000001"),
			Name = "Administrador",
			Description = "Rol con todos los permisos del sistema"
		}
	};
}

