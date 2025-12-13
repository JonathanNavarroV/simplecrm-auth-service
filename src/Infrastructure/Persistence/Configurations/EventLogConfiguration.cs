using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;
using Domain.Common;

namespace Infrastructure.Persistence.Configurations;

public class EventLogConfiguration : IEntityTypeConfiguration<EventLog>
{
    public void Configure(EntityTypeBuilder<EventLog> builder)
    {
        builder.ToTable("EventLogs");

        builder.HasKey(e => e.Id);

        // Propiedades obligatorias
        builder.Property(e => e.Action)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(e => e.CreatedByType)
            .IsRequired();

        // Nombres y roles
        builder.Property(e => e.CreatedBySystemName)
            .HasMaxLength(ValidationConstants.NameMaxLength)
            .IsUnicode(false);

        builder.Property(e => e.CreatedByUserName)
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(e => e.CreatedByUserRole)
            .HasMaxLength(ValidationConstants.NameMaxLength);

        // Entidad relacionada
        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength)
            .IsUnicode(false);

        builder.Property(e => e.EntityDisplayName)
            .IsRequired()
            .HasMaxLength(ValidationConstants.NameMaxLength);

        builder.Property(e => e.EntityId)
            .IsRequired()
            .HasMaxLength(ValidationConstants.CodeMaxLength)
            .IsUnicode(false);

        // Campos que contienen JSON - mapear a jsonb en PostgreSQL
        builder.Property(e => e.Changes)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(e => e.BeforeState)
            .HasColumnType("jsonb");

        builder.Property(e => e.AfterState)
            .HasColumnType("jsonb");

        // Índices útiles (incluye CreatedAt para consultas ordenadas por fecha)
        builder.HasIndex(e => new { e.EntityType, e.EntityId, e.CreatedAt });
        builder.HasIndex(e => e.CreatedByUserRun);
        builder.HasIndex(e => e.CreatedAt);
    }
}
