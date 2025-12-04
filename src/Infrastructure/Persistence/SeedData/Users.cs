using System;
using SimpleCRM.Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class Users
{
    // Datos mock para development / pruebas
        public static readonly User[] SeedData = new[]
    {
        new User
        {
            // Run sin guión según diseño (int)
            Run = 19241027,
            Dv = "4",
            FirstNames = "Jonathan Damián",
            LastNames = "Navarro Vega",
            Email = "jonathan.d.navarro.v@gmail.com",
            IsActive = true,
            IsDeleted = false,
            CreatedAt = new DateTime(2025, 12, 4, 0, 0, 0, DateTimeKind.Utc),
            CreatedByUserRun = null,
            UpdatedAt = new DateTime(2025, 12, 4, 0, 0, 0, DateTimeKind.Utc),
            UpdatedByUserRun = null,
            DeletedAt = null,
            DeletedByUserRun = null
        }
    };
}
