using System;
using Domain.Entities;

namespace Infrastructure.Persistence.SeedData;

public static class Users
{
    public static readonly User[] SeedData = new[]
    {
        new User
        {
            Run = 19241027,
            Dv = "4",
            FirstNames = "Jonathan Damián",
            LastNames = "Navarro Vega",
            Email = "jonathan.d.navarro.v@gmail.com"
        }
    };
}
