using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Infrastructure.Persistence.SeedData;
using Domain.Common;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Run);

        builder.Property(u => u.Dv)
            .IsRequired()
            .HasMaxLength(ValidationConstants.DvMaxLength)
            .IsUnicode(false);

        builder.Property(u => u.FirstNames)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(u => u.LastNames)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(ValidationConstants.EmailMaxLength);

        builder.HasIndex(u => u.Email).IsUnique(false);

        // Estado por defecto
        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.IsDeleted)
            .IsRequired();

        // Datos de seed (datos de inicialización)
        builder.HasData(Users.SeedData);
    }
}
