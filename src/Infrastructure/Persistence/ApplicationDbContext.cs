using Microsoft.EntityFrameworkCore;
using SimpleCRM.Domain.Entities;

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
        // Aplicar configuraciones separadas de la carpeta Configurations
        modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());

        // Llama al comportamiento por defecto de EF Core
        base.OnModelCreating(modelBuilder);
    }

    // DbSets (tablas)
    public DbSet<User> Users { get; set; }
}
