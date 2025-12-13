using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Obtiene un usuario por su RUN.
    /// </summary>
    /// <param name="run">RUN del usuario (sin guión)</param>
    /// <param name="includeDeleted">Si es true, incluye usuarios marcados como eliminados (IsDeleted = true)</param>
    /// <param name="includeInactive">Si es true, incluye usuarios inactivos (IsActive = false). Por defecto solo devuelve activos.</param>
    Task<User?> GetByRunAsync(int run, bool includeDeleted = false, bool includeInactive = false);

    /// <summary>
    /// Obtiene un usuario por su email.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, bool includeDeleted = false, bool includeInactive = false);

    /// <summary>
    /// Obtiene los códigos de permiso asociados al usuario (permisos directos y los heredados por roles).
    /// </summary>
    Task<string[]> GetPermissionCodesAsync(int run);
}
