using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain.Common;

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

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(ValidationConstants.DescriptionMaxLength);

        builder.Property(r => r.IsActive).IsRequired();
        builder.Property(r => r.IsDeleted).IsRequired();

        // Auditoría
        builder.Property(r => r.CreatedAt).IsRequired();
        builder.Property(r => r.CreatedByUserRun);

        builder.Property(r => r.UpdatedAt).IsRequired();
        builder.Property(r => r.UpdatedByUserRun);

        builder.Property(r => r.DeletedAt);
        builder.Property(r => r.DeletedByUserRun);

        builder.HasIndex(r => r.Name).IsUnique();
    }
}
