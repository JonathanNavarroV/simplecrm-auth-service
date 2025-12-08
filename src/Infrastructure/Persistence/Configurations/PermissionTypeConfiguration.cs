using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Infrastructure.Persistence.SeedData;
using Domain.Common;

namespace Infrastructure.Persistence.Configurations;

public class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder.ToTable("PermissionTypes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CodeMaxLength);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(t => t.Description)
            .HasMaxLength(ValidationConstants.DescriptionMaxLength);

        builder.HasIndex(t => t.Code).IsUnique();

        builder.HasData(PermissionTypes.SeedData);
    }
}
