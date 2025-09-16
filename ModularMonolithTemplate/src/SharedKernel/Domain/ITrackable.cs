namespace ModularMonolith.Platform.SharedKernel.Domain;

public interface ITrackable
{
    AuditStamp Audit { get; }
}
