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

        builder.Property(r => r.IsActive)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .IsRequired();

        builder.HasIndex(r => r.Name).IsUnique();
    }
}
