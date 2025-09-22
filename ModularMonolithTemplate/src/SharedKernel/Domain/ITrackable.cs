namespace ModularMonolithTemplate.SharedKernel.Domain;

public interface ITrackable
{
    AuditStamp Audit { get; }
}
