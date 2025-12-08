using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Infrastructure.Persistence.SeedData;
using Domain.Common;

namespace Infrastructure.Persistence.Configurations;

public class PermissionModuleConfiguration : IEntityTypeConfiguration<PermissionModule>
{
    public void Configure(EntityTypeBuilder<PermissionModule> builder)
    {
        builder.ToTable("PermissionModules");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Code)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CodeMaxLength);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(ValidationConstants.DescriptionMaxLength);

        builder.HasIndex(m => m.Code).IsUnique();

        // Seed
        builder.HasData(PermissionModules.SeedData);
    }
}
