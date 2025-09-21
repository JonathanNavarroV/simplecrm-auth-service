using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// Representa la conexión a la base de datos y los DbSet (tablas)
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Constructor que recoje las opciones configuradas en Program.cs
    /// </summary>
    /// <param name="options"></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>
    /// Método para configurar reglas adicionales (constraints, índices, relaciones, etc.)
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Llama al comportamiento por defecto de ED Core
        base.OnModelCreating(modelBuilder);
    }
}
