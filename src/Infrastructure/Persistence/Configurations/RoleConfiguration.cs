using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain.Common;
using Infrastructure.Persistence.SeedData;

namespace Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(ValidationConstants.DescriptionMaxLength);

        // Seed
        builder.HasData(Roles.SeedData);

        // Auditoría: mapear a timestamp with time zone
        builder.Property(r => r.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(r => r.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(r => r.DeletedAt)
            .HasColumnType("timestamp with time zone");
    }
}
