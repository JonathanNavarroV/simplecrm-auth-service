using System;

namespace Application.Features.Users.Dto;

public record UserDto
{
    // Identificador — puede ser null si el handler no quiere exponerlo
    public int? Run { get; init; }
    public string? Dv { get; init; }
    public string? FirstNames { get; init; }
    public string? LastNames { get; init; }
    public string? Email { get; init; }

    // Estado
    public bool? IsActive { get; init; }
    public bool? IsDeleted { get; init; }

    // Auditoría
    public DateTime? CreatedAt { get; init; }
    public int? CreatedByUserRun { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public int? UpdatedByUserRun { get; init; }
    public DateTime? DeletedAt { get; init; }
    public int? DeletedByUserRun { get; init; }
}
