using System;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Common;

public static class DeterministicGuid
{
    /// <summary>
    /// Crea un Guid determinístico a partir de una cadena usando MD5.
    /// Útil para generar ids estables en seed data y migraciones.
    /// </summary>
    public static Guid Create(string input)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return new Guid(bytes);
    }
}
