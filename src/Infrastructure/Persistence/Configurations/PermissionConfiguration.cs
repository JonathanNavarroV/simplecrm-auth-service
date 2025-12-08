using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain.Common;
using Infrastructure.Persistence.SeedData;

namespace Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CodeMaxLength);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(ValidationConstants.DescriptionMaxLength);

        builder.HasOne(p => p.PermissionSection)
            .WithMany(s => s.Permissions)
            .HasForeignKey(p => p.PermissionSectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.PermissionType)
            .WithMany(t => t.Permissions)
            .HasForeignKey(p => p.PermissionTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed
        builder.HasData(Permissions.SeedData);
    }
}
