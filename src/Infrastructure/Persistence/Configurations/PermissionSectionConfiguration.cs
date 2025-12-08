using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Infrastructure.Persistence.SeedData;
using Domain.Common;

namespace Infrastructure.Persistence.Configurations;

public class PermissionSectionConfiguration : IEntityTypeConfiguration<PermissionSection>
{
    public void Configure(EntityTypeBuilder<PermissionSection> builder)
    {
        builder.ToTable("PermissionSections");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CodeMaxLength);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(s => s.Description)
            .HasMaxLength(ValidationConstants.DescriptionMaxLength);

        builder.HasOne(s => s.PermissionModule)
            .WithMany(m => m.Sections)
            .HasForeignKey(s => s.PermissionModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed
        builder.HasData(PermissionSections.SeedData);
    }
}
