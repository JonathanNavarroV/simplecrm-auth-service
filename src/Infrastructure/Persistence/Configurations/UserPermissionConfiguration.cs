using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("UserPermissions");

        builder.HasKey(up => new { up.UserRun, up.PermissionId });

        builder.HasOne(up => up.User)
            .WithMany()
            .HasForeignKey(up => up.UserRun)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(up => up.Permission)
            .WithMany()
            .HasForeignKey(up => up.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
