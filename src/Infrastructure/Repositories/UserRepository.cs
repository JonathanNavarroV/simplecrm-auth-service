using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Persistence.SeedData;
using System;
using System.Linq;

using System.Collections.Generic;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _db;

	public UserRepository(ApplicationDbContext db)
	=> _db = db;

	public async Task<User?> GetByRunAsync(int run, bool includeDeleted = false, bool includeInactive = false)
	{
		var query = _db.Users.AsNoTracking().Where(u => u.Run == run);

		if (!includeDeleted)
		{
			query = query.Where(u => !u.IsDeleted);
		}

		if (!includeInactive)
		{
			query = query.Where(u => u.IsActive);
		}

		return await query.FirstOrDefaultAsync();
	}

	public async Task<User?> GetByEmailAsync(string email, bool includeDeleted = false, bool includeInactive = false)
	{
		var query = _db.Users.AsNoTracking().Where(u => u.Email.ToLower() == email.ToLower());

		if (!includeDeleted)
		{
			query = query.Where(u => !u.IsDeleted);
		}

		if (!includeInactive)
		{
			query = query.Where(u => u.IsActive);
		}

		return await query.FirstOrDefaultAsync();
	}

	public async Task<string[]> GetPermissionCodesAsync(int run)
	{
		// Permisos directos del usuario
		var userPerms = from up in _db.UserPermissions
						where up.UserRun == run
						join p in _db.Permissions on up.PermissionId equals p.Id
						select p.Code;

		// Permisos vía roles asignados al usuario
		var rolePerms = from ur in _db.UserRoles
						where ur.UserRun == run
						join rp in _db.RolePermissions on ur.RoleId equals rp.RoleId
						join p in _db.Permissions on rp.PermissionId equals p.Id
						select p.Code;

		var all = userPerms.Concat(rolePerms).Distinct();
		return await all.ToArrayAsync();
	}

	public async Task<string[]> GetRoleNamesAsync(int run)
	{
		var roles = from ur in _db.UserRoles
					where ur.UserRun == run
					join r in _db.Roles on ur.RoleId equals r.Id
					select r.Name;

		return await roles.Distinct().ToArrayAsync();
	}
}
