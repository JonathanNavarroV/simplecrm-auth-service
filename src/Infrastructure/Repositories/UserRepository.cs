using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using SimpleCRM.Domain.Entities;
using Application.Interfaces;

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
}
