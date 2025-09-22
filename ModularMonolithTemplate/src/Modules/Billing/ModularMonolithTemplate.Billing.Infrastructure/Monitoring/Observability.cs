using System.Diagnostics;

namespace ModularMonolithTemplate.Billing.Infrastructure.Monitoring;

public static class Observability
{
    public static readonly ActivitySource Activity = new("Billing");
}
