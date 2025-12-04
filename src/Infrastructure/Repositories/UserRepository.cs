using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using SimpleCRM.Domain.Entities;
using Application.Interfaces;
using Infrastructure.Persistence.SeedData;
using System;

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

		var user = await query.FirstOrDefaultAsync();

		// Fallback to development seed data if DB is empty or user not found (helps local dev without running migrations)
		if (user is null)
		{
			try
			{
				user = Users.SeedData.FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
			}
			catch
			{
				// ignore fallback errors
			}
		}

		return user;
	}
}
