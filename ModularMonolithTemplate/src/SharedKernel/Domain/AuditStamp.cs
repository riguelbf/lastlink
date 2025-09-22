using System;

namespace ModularMonolithTemplate.SharedKernel.Domain;

public sealed class AuditStamp
{
    public string? CreatedBy { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public string? UpdatedBy { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }

    private AuditStamp()
    {
    } // EF

    public static AuditStamp New(string? user)
        => new AuditStamp { CreatedAtUtc = DateTime.UtcNow, CreatedBy = user };

    public AuditStamp Touch(string? user)
        => new AuditStamp
        {
            CreatedAtUtc = CreatedAtUtc, CreatedBy = CreatedBy,
            UpdatedAtUtc = DateTime.UtcNow, UpdatedBy = user
        };
}
