using System;

namespace ModularMonolith.Platform.SharedKernel.Domain;

public sealed class AuditStamp
{
    public DateTime CreatedAtUtc { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public string? UpdatedBy { get; private set; }

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
