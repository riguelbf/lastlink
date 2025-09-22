using System.Diagnostics;

namespace ModularMonolithTemplate.Billing.Crosscutting.Monitoring;

public static class Observability
{
    public static readonly ActivitySource Activity = new("Billing");
}
